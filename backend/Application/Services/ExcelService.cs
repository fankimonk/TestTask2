using Application.Enums;
using Application.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using OfficeOpenXml;

namespace Application.Services
{
    //Сервис для импорта excel файла с расширением .xlsx в БД
    public class ExcelService : IExcelService
    {
        private readonly IBanksRepository _banksRepository;
        private readonly IPeriodsRepository _periodsRepository;
        private readonly IFilesRepository _filesRepository;
        private readonly IClassesRepository _classesRepository;
        private readonly IGlobalTotalsRepository _globalTotalsRepository;
        private readonly IClassTotalsRepository _classTotalsRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IGroupTotalsRepository _groupTotalsRepository;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IBalanceSheetRecordsRepository _balanceSheetRecordsRepository;

        public ExcelService(
            IBanksRepository banksRepository,
            IPeriodsRepository periodsRepository,
            IFilesRepository filesRepository,
            IClassesRepository classesRepository,
            IGlobalTotalsRepository globalTotalsRepository,
            IClassTotalsRepository classTotalsRepository,
            IGroupsRepository groupsRepository,
            IGroupTotalsRepository groupTotalsRepository,
            IAccountsRepository accountsRepository,
            IBalanceSheetRecordsRepository balanceSheetRecordsRepository)
        {
            _banksRepository = banksRepository;
            _periodsRepository = periodsRepository;
            _filesRepository = filesRepository;
            _classesRepository = classesRepository;
            _globalTotalsRepository = globalTotalsRepository;
            _classTotalsRepository = classTotalsRepository;
            _groupsRepository = groupsRepository;
            _groupTotalsRepository = groupTotalsRepository;
            _accountsRepository = accountsRepository;
            _balanceSheetRecordsRepository = balanceSheetRecordsRepository;
        }

        public async Task<FilesUploadResult> Upload(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (!File.Exists(filePath))
                return FilesUploadResult.Failed;

            using var package = new ExcelPackage(filePath);

            var worksheet = package.Workbook.Worksheets[0];
            if (worksheet == null) return FilesUploadResult.Failed;

            var parsedBank = ParseBank(worksheet);
            if (parsedBank == null) return FilesUploadResult.Failed;

            var bank = await _banksRepository.AddOrGetExistingAsync(parsedBank);
            if (bank == null) return FilesUploadResult.Failed;

            var parsedPeriod = ParsePeriod(worksheet);
            if (parsedPeriod == null) return FilesUploadResult.Failed;

            var period = await _periodsRepository.AddOrGetExistingAsync(parsedPeriod);
            if (period == null) return FilesUploadResult.Failed;

            var publicationDate = ParsePublicationDate(worksheet);
            if (publicationDate == new DateTime(1, 1, 1)) return FilesUploadResult.Failed;

            var sheetFile = new SheetFile
            {
                FileName = Path.GetFileName(filePath),
                PublicationDate = publicationDate,
                BankId = bank.BankId,
                PeriodId = period.PeriodId
            };
            var uploadedFile = await _filesRepository.AddAsync(sheetFile);
            if (uploadedFile == null) return FilesUploadResult.FileExists;

            int rowIndex = 9;
            bool checkedGlobalTotal = false;

            var currentGroupAccounts = new List<Account>();
            var currentGroupRecords = new List<BalanceSheetRecord>();

            int currentClassId = 0;

            while (!checkedGlobalTotal)
            {
                var currentCellText = worksheet.Cells["A" + rowIndex.ToString()].Text;

                if (currentCellText.Length == 4) //Account
                {
                    var parsedAccount = ParseAccount(currentCellText);
                    if (parsedAccount == null) return FilesUploadResult.Failed;

                    currentGroupAccounts.Add(parsedAccount);

                    var parsedRecord = ParseBalanceSheetRecord(worksheet, rowIndex);
                    if (parsedRecord == null) return FilesUploadResult.Failed;

                    currentGroupRecords.Add(parsedRecord);
                }
                else if (currentCellText.Length == 2) //GroupTotal
                {
                    var parsedGroup = new Group { GroupNumber = currentCellText, ClassId = currentClassId };
                    var group = await _groupsRepository.AddAsync(parsedGroup);
                    if (group == null) return FilesUploadResult.Failed;

                    for (int i = 0; i < currentGroupRecords.Count; i++)
                    {
                        var parsedAccount = currentGroupAccounts[i];
                        parsedAccount.GroupId = group.GroupId;

                        var account = await _accountsRepository.AddAsync(parsedAccount);
                        if (account == null) return FilesUploadResult.Failed;

                        var parsedRecord = currentGroupRecords[i];
                        parsedRecord.AccountId = account.AccountId;

                        var record = await _balanceSheetRecordsRepository.AddAsync(parsedRecord);
                        if (record == null) return FilesUploadResult.Failed;
                    }
                    currentGroupAccounts.Clear();
                    currentGroupRecords.Clear();

                    var parsedGroupTotal = ParseGroupTotal(worksheet, rowIndex);
                    if (parsedGroupTotal == null) return FilesUploadResult.Failed;
                    parsedGroupTotal.GroupId = group.GroupId;

                    var groupTotal = await _groupTotalsRepository.AddAsync(parsedGroupTotal);
                    if (groupTotal == null) return FilesUploadResult.Failed;
                }
                else if (currentCellText == "ПО КЛАССУ") //ClassTotal
                {
                    var parsedClassTotal = ParseClassTotal(worksheet, rowIndex);
                    if (parsedClassTotal == null) return FilesUploadResult.Failed;
                    parsedClassTotal.ClassId = currentClassId;

                    var classTotal = await _classTotalsRepository.AddAsync(parsedClassTotal);
                    if (classTotal == null) return FilesUploadResult.Failed;
                }
                else if (currentCellText == "БАЛАНС") //GlobalTotal
                {
                    var parsedGlobalTotal = ParseGlobalTotal(worksheet, rowIndex);
                    if (parsedGlobalTotal == null) return FilesUploadResult.Failed;
                    parsedGlobalTotal.FileId = uploadedFile.FileId;

                    var globalTotal = await _globalTotalsRepository.AddAsync(parsedGlobalTotal);
                    if (globalTotal == null) return FilesUploadResult.Failed;

                    checkedGlobalTotal = true;
                }
                else //Class
                {
                    var parsedClass = ParseClass(worksheet, currentCellText);
                    if (parsedClass == null) return FilesUploadResult.Failed;
                    parsedClass.FileId = uploadedFile.FileId;

                    var accountClass = await _classesRepository.AddAsync(parsedClass);
                    if (accountClass == null) return FilesUploadResult.Failed;

                    currentClassId = accountClass.ClassId;
                }

                rowIndex++;
            }

            return FilesUploadResult.Uploaded;
        }

        private Bank? ParseBank(ExcelWorksheet worksheet)
        {
            var bankName = worksheet.Cells["A1"].Text;
            if (string.IsNullOrEmpty(bankName)) return null;
            return new Bank { BankName = bankName };
        }

        private Account? ParseAccount(string accountString)
        {
            if (!int.TryParse(accountString, out var parseResult) && (parseResult < 1000 || parseResult > 9999))
                return null;
            return new Account { AccountNumber = accountString };
        }

        private Period? ParsePeriod(ExcelWorksheet worksheet)
        {
            var periodString = worksheet.Cells["A3"].Text;
            var periodStringParts = periodString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!DateOnly.TryParse(periodStringParts[3], out var startDate))
                return null;
            if (!DateOnly.TryParse(periodStringParts[5], out var endDate))
                return null;

            return new Period { StartDate = startDate, EndDate = endDate };
        }

        private DateTime ParsePublicationDate(ExcelWorksheet worksheet)
        {
            var publicationDateString = worksheet.Cells["A6"].Text;
            if (!DateTime.TryParse(publicationDateString, out var publicationDate)) return new DateTime(1, 1, 1);

            return publicationDate;
        }

        private Class? ParseClass(ExcelWorksheet worksheet, string classString)
        {
            var parts = classString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return null;

            var classNumber = parts[1];
            if (!int.TryParse(classNumber, out var parseResult))
                return null;

            var className = string.Join(" ", parts, 2, parts.Length - 2);

            return new Class { ClassNumber = classNumber, ClassName = className };
        }

        private decimal[]? ParseRecord(ExcelWorksheet worksheet, int rowIndex)
        {
            var recordValues = new decimal[6];

            if (!decimal.TryParse(worksheet.Cells[rowIndex, 2].Text, out recordValues[0])) return null;
            if (!decimal.TryParse(worksheet.Cells[rowIndex, 3].Text, out recordValues[1])) return null;
            if (!decimal.TryParse(worksheet.Cells[rowIndex, 4].Text, out recordValues[2])) return null;
            if (!decimal.TryParse(worksheet.Cells[rowIndex, 5].Text, out recordValues[3])) return null;
            if (!decimal.TryParse(worksheet.Cells[rowIndex, 6].Text, out recordValues[4])) return null;
            if (!decimal.TryParse(worksheet.Cells[rowIndex, 7].Text, out recordValues[5])) return null;

            return recordValues;
        }

        private BalanceSheetRecord? ParseBalanceSheetRecord(ExcelWorksheet worksheet, int rowIndex)
        {
            var recordValues = ParseRecord(worksheet, rowIndex);
            if (recordValues == null || recordValues.Length != 6) return null;

            return new BalanceSheetRecord
            {
                OpeningBalancesActive = recordValues[0],
                OpeningBalancesPassive = recordValues[1],
                TurnoversDebit = recordValues[2],
                TurnoversCredit = recordValues[3],
                ClosingBalancesActive = recordValues[4],
                ClosingBalancesPassive = recordValues[5]
            };
        }

        private GlobalTotal? ParseGlobalTotal(ExcelWorksheet worksheet, int rowIndex)
        {
            var totalValues = ParseRecord(worksheet, rowIndex);
            if (totalValues == null || totalValues.Length != 6) return null;

            return new GlobalTotal
            {
                OpeningBalancesActive = totalValues[0],
                OpeningBalancesPassive = totalValues[1],
                TurnoversDebit = totalValues[2],
                TurnoversCredit = totalValues[3],
                ClosingBalancesActive = totalValues[4],
                ClosingBalancesPassive = totalValues[5]
            };
        }

        private ClassTotal? ParseClassTotal(ExcelWorksheet worksheet, int rowIndex)
        {
            var totalValues = ParseRecord(worksheet, rowIndex);
            if (totalValues == null || totalValues.Length != 6) return null;

            return new ClassTotal
            {
                OpeningBalancesActive = totalValues[0],
                OpeningBalancesPassive = totalValues[1],
                TurnoversDebit = totalValues[2],
                TurnoversCredit = totalValues[3],
                ClosingBalancesActive = totalValues[4],
                ClosingBalancesPassive = totalValues[5]
            };
        }

        private GroupTotal? ParseGroupTotal(ExcelWorksheet worksheet, int rowIndex)
        {
            var totalValues = ParseRecord(worksheet, rowIndex);
            if (totalValues == null || totalValues.Length != 6) return null;

            return new GroupTotal
            {
                OpeningBalancesActive = totalValues[0],
                OpeningBalancesPassive = totalValues[1],
                TurnoversDebit = totalValues[2],
                TurnoversCredit = totalValues[3],
                ClosingBalancesActive = totalValues[4],
                ClosingBalancesPassive = totalValues[5]
            };
        }
    }
}

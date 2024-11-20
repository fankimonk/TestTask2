using Application.Interfaces;
using Application.Responses;
using DataAccess.Interfaces;

namespace Application.Services
{
    //Сервис для работы с таблицами
    public class TableService : ITableService
    {
        private readonly IClassesRepository _classesRepository;
        private readonly IGlobalTotalsRepository _globalTotalsRepository;

        public TableService(IClassesRepository classesRepository,
            IGlobalTotalsRepository globalTotalsRepository)
        {
            _classesRepository = classesRepository;
            _globalTotalsRepository = globalTotalsRepository;
        }

        //Получить таблицу по файлу для клиента
        public async Task<TableResponse?> GetTable(int fileId)
        {
            var classes = await _classesRepository.GetWithIncludedDataAsync(fileId);
            if (classes.Count == 0) return null;
            var globalTotal = await _globalTotalsRepository.GetByFileId(fileId);
            if (globalTotal == null) return null;

            //Генерация таблицы, где данные будут разбиты на классы, в которых будут данные о группах в этом классе,
            //в которых будут данные о записях в этих группах
            var classResponses = classes.Select(c =>
                new ClassResponse(
                    c.ClassNumber,
                    c.ClassName,
                    new ClassTotalResponse(
                        c.ClassTotals.First().OpeningBalancesActive,
                        c.ClassTotals.First().OpeningBalancesPassive,
                        c.ClassTotals.First().TurnoversDebit,
                        c.ClassTotals.First().TurnoversCredit,
                        c.ClassTotals.First().ClosingBalancesActive,
                        c.ClassTotals.First().ClosingBalancesPassive
                    ),
                    c.Groups.Select(g =>
                        new GroupResponse(
                            g.GroupNumber,
                            new GroupTotalResponse(
                                g.GroupTotals.First().OpeningBalancesActive,
                                g.GroupTotals.First().OpeningBalancesPassive,
                                g.GroupTotals.First().TurnoversDebit,
                                g.GroupTotals.First().TurnoversCredit,
                                g.GroupTotals.First().ClosingBalancesActive,
                                g.GroupTotals.First().ClosingBalancesPassive),
                            g.Accounts.Select(a =>
                            {
                                var record = a.BalanceSheetRecords.First();
                                return new RecordResponse(a.AccountNumber,
                                record.OpeningBalancesActive,
                                record.OpeningBalancesPassive,
                                record.TurnoversDebit,
                                record.TurnoversCredit,
                                record.ClosingBalancesActive,
                                record.ClosingBalancesPassive);
                            }).ToList()
                        )
                    ).ToList()
                )
            ).ToList();

            var tableResponse = new TableResponse(classResponses, new GlobalTotalResponse(
                globalTotal.OpeningBalancesActive,
                globalTotal.OpeningBalancesPassive,
                globalTotal.TurnoversDebit,
                globalTotal.TurnoversCredit,
                globalTotal.ClosingBalancesActive,
                globalTotal.ClosingBalancesPassive
            ));

            return tableResponse;
        }
    }
}

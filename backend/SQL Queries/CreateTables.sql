use TestTask2;

create table Periods (
	PeriodId int identity(1,1) primary key,
	StartDate date not null,
	EndDate date not null,
	constraint UQ_Period unique (StartDate, EndDate)
);

create table Banks (
	BankId int identity(1,1) primary key,
	BankName nvarchar(100) unique not null
);

create table SheetFiles (
    FileId int identity(1,1) primary key,
    FileName nvarchar(100) unique not null,
	PublicationDate datetime not null,
	BankId int not null,
	PeriodId int not null,
	constraint FK_Files_Banks foreign key (BankId) references Banks(BankId)
		on delete cascade
		on update cascade,
	constraint FK_Files_Periods foreign key (PeriodId) references Periods(PeriodId)
		on delete cascade
		on update cascade
);

create table Classes (
    ClassId int identity(1,1) primary key,
	ClassNumber char(1) not null,
    ClassName nvarchar(max) not null,
	FileId int not null,
	constraint FK_Classes_SheetFiles foreign key (FileId) references SheetFiles(FileId)
		on delete cascade
		on update cascade,
	constraint CK_Classes_ClassNumberOnlyDigit check (ClassNumber like '[1-9]')
);

create table Groups (
	GroupId int identity(1,1) primary key,
	GroupNumber char(2) not null,
	ClassId int not null,
	constraint FK_Groups_Classes foreign key (ClassId) references Classes(ClassId)
		on delete cascade
		on update cascade,
	constraint CK_Groups_GroupNumberFrom10to99 check (GroupNumber between '10' and '99')
);

create table Accounts (
	AccountId int identity(1,1) primary key,
	AccountNumber char(4) not null,
	GroupId int not null,
	constraint FK_Accounts_Groups foreign key (GroupId) references Groups(GroupId)
		on delete cascade
		on update cascade,
	constraint CK_Accounts_AccountNumberFrom1000to9999 check (AccountNumber between '1000' and '9999')
);

create table BalanceSheetRecords (
	RecordId int identity(1,1) primary key,
	AccountId int not null,
	OpeningBalancesActive decimal(18,2) not null,
	OpeningBalancesPassive decimal(18,2) not null,
	TurnoversDebit decimal(18,2) not null,
	TurnoversCredit decimal(18,2) not null,
	ClosingBalancesActive decimal(18,2) not null,
	ClosingBalancesPassive decimal(18,2) not null,
	constraint FK_BalanceSheet_Accounts foreign key (AccountId) references Accounts(AccountId)
		on delete cascade
		on update cascade
);

create table GroupTotals (
	RecordId int identity(1,1) primary key,
	GroupId int not null,
	OpeningBalancesActive decimal(18,2) not null,
	OpeningBalancesPassive decimal(18,2) not null,
	TurnoversDebit decimal(18,2) not null,
	TurnoversCredit decimal(18,2) not null,
	ClosingBalancesActive decimal(18,2) not null,
	ClosingBalancesPassive decimal(18,2) not null,
	constraint FK_GroupsTotals_Groups foreign key (GroupId) references Groups(GroupId)
		on delete cascade
		on update cascade
);

create table ClassTotals (
	RecordId int identity(1,1) primary key,
	ClassId int not null,
	OpeningBalancesActive decimal(18,2) not null,
	OpeningBalancesPassive decimal(18,2) not null,
	TurnoversDebit decimal(18,2) not null,
	TurnoversCredit decimal(18,2) not null,
	ClosingBalancesActive decimal(18,2) not null,
	ClosingBalancesPassive decimal(18,2) not null,
	constraint FK_ClassesTotals_Classes foreign key (ClassId) references Classes(ClassId)
		on delete cascade
		on update cascade
);

create table GlobalTotals (
	RecordId int identity(1,1) primary key,
	FileId int not null,
	OpeningBalancesActive decimal(18,2) not null,
	OpeningBalancesPassive decimal(18,2) not null,
	TurnoversDebit decimal(18,2) not null,
	TurnoversCredit decimal(18,2) not null,
	ClosingBalancesActive decimal(18,2) not null,
	ClosingBalancesPassive decimal(18,2) not null,
	constraint FK_GlobalTotals_Files foreign key (FileId) references SheetFiles(FileId)
		on delete cascade
		on update cascade
);
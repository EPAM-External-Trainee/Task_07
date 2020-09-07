﻿namespace ResultOfTheSessionUnitTestProject.ReportsUnitTest
{
    public abstract class ReportsUnitTestData
    {
        protected const string ConnectionString = @"Data Source=KONSTANTINPC\SQLEXPRESS; Initial Catalog=ResultSession; Integrated Security=true;";

        protected const string PathToSessionResultReportExcelFile = @"..\..\..\ResultOfTheSessionUnitTestProject\ReportsUnitTest\Resources\SessionResultReport.xlsx";

        protected const string PathToGroupSessionResultReportExcelFile = @"..\..\..\ResultOfTheSessionUnitTestProject\ReportsUnitTest\Resources\GroupSessionResultReport.xlsx";
    }
}
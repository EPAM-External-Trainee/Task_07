﻿namespace BLL.Reports.Excel.Views.Interfaces.SessionResultReport.TableRawViews
{
    public interface IGroupTableRawView
    {
        string Surname { get; set; }

        string Name { get; set; }

        string Patronymic { get; set; }

        string Subject { get; set; }

        string Form { get; set; }

        string Date { get; set; }

        string Assessment { get; set; }
    }
}
﻿using BLL.Reports.Abstract;
using BLL.Reports.Structs.ExcelTableRawViews.SessionResultReport;
using BLL.Reports.Structs.ReportData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Reports.Models
{
    public class SessionResultReport : Report
    {
        public SessionResultReport(string connectionString) : base(connectionString)
        {
        }

        private IEnumerable<GroupTableRawView> GetGroupTableRowsData(int sessionId, int groupId)
        {
            List<GroupTableRawView> result = new List<GroupTableRawView>();
            result.AddRange(from st in Students
                            join sr in SessionResults on st.Id equals sr.StudentId
                            join s in Subjects on sr.SubjectId equals s.Id
                            join ss in SessionSchedules on st.GroupId equals ss.GroupId
                            join kaf in KnowledgeAssessmentForms on ss.KnowledgeAssessmentFormId equals kaf.Id
                            join g in Groups on st.GroupId equals g.Id
                            where ss.SubjectId == sr.SubjectId && ss.SessionId == sessionId && st.GroupId == groupId
                            select new GroupTableRawView(st.Name, st.Surname, st.Patronymic, s.Name, kaf.Form, ss.Date.ToShortDateString(), sr.Assessment));
            return result;
        }

        private IEnumerable<GroupSpecialtyTableRawView> GetGroupSpecialtyTableRawsData(int sessionId)
        {
            List<GroupSpecialtyTableRawView> result = new List<GroupSpecialtyTableRawView>();

            var sessionSpecialities = from g in Groups
                                      join st in Students on g.Id equals st.Id
                                      join sr in SessionResults on st.Id equals sr.StudentId
                                      join gs in GroupSpecialties on g.GroupSpecialtyId equals gs.Id
                                      select gs.Name;

            List<double> assessments = new List<double>();
            foreach (string specialty in sessionSpecialities.Distinct())
            {
                assessments.AddRange(from g in Groups
                                     join st in Students on g.Id equals st.GroupId
                                     join sr in SessionResults on st.Id equals sr.StudentId
                                     join ss in SessionSchedules on st.GroupId equals ss.GroupId
                                     join gs in GroupSpecialties on g.GroupSpecialtyId equals gs.Id
                                     where ss.SubjectId == sr.SubjectId && ss.KnowledgeAssessmentFormId == 1 && sr.SessionId == sessionId && gs.Name == specialty
                                     select double.Parse(sr.Assessment));
                result.Add(new GroupSpecialtyTableRawView(specialty, Math.Round(assessments.Average(), 2)));
                assessments.Clear();
            }

            return result;
        }

        private IEnumerable<ExaminersTableRawView> GetExaminersTableRawsData(int sessionId)
        {
            List<ExaminersTableRawView> result = new List<ExaminersTableRawView>();




            return result;
        }

        private string GetSessionInfo(int sessionId) => Sessions.FirstOrDefault(s => s.Id == sessionId)?.ToString();

        public SessionResultReportData GetSessionResultReportData(int sessionId)
        {
            Dictionary<string, List<GroupTableRawView>> groupTableDictionary = new Dictionary<string, List<GroupTableRawView>>();
            foreach (int groupId in SessionSchedules.Where(ss => ss.SessionId == sessionId).Select(ss => ss.GroupId).Distinct().ToList())
            {
                groupTableDictionary.Add(Groups.FirstOrDefault(g => g.Id == groupId).Name, GetGroupTableRowsData(sessionId, groupId).ToList());
            }
            return new SessionResultReportData(groupTableDictionary, GetSessionInfo(sessionId), GetGroupSpecialtyTableRawsData(sessionId), GetExaminersTableRawsData(sessionId));
        }
    }
}
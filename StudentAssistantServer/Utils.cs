using System;
using System.Collections.Generic;
using System.Globalization;
using Google.Protobuf.WellKnownTypes;

namespace StudentAssistantServer
{
    public class Utils
    {
        public static int GetCurrentWeek(string startDateStr)
        {
            
            DateTime startDate = DateTime.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            while (!startDate.Date.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                startDate = startDate.AddDays(1);
            }

            DateTime firstSunday = startDate;
            var result = Convert.ToInt32(Math.Ceiling((DateTime.Now.Date - firstSunday.Date)
                                                      .TotalDays / 7 + 1));
            return result;
        }

        public static int GetRemainedDays(UserInfoItem userInfo, HomeworkItem homework, ScheduleItem schedule)
        {
            int currentWeek = Utils.GetCurrentWeek(userInfo.StartDate);
            
            SubjectCoords coords = new SubjectCoords();
            for (int i = 0; i < schedule.Schedule.Count; i++)
            {
                for (int j = 0; j < schedule.Schedule[i].Count; j++)
                {
                    if (homework.Week % 2 == 0)
                    {
                        if (schedule.Schedule[i][j][0] == homework.Subject)
                        {
                            coords.DayOfWeek = i;
                            coords.SubjectNumber = j;
                            coords.WeekType = 0;
                        }
                    }
                    else
                    {
                        if (schedule.Schedule[i][j][1] == homework.Subject)
                        {
                            coords.DayOfWeek = i;
                            coords.SubjectNumber = j;
                            coords.WeekType = 1;
                        }
                    }
                }
            }
            
            int days = 0;
            if (coords.DayOfWeek >= Convert.ToInt32(DateTime.Now.DayOfWeek))
            {
                days = (homework.Week - currentWeek) * 7 + (coords.DayOfWeek - Convert.ToInt32(DateTime.Now.DayOfWeek));
            }
            else
            {
                days = (homework.Week - currentWeek) * 7 - (Convert.ToInt32(DateTime.Now.DayOfWeek) - coords.DayOfWeek);
            }

            days++;
            return days;
        }

        public static bool IsSubjectExistInWeek(HomeworkItem homework, ScheduleItem schedule)
        {
            foreach (var day in schedule.Schedule)
            {
                foreach (var subject in day)
                {
                    if (homework.Week % 2 != 0)
                    {
                        if (subject[0] == homework.Subject)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (subject[1] == homework.Subject)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static List<string> GetSubjects(ScheduleItem schedule)
        {
            var subjects = new Dictionary<int, string>();
            var list = new List<string>();
            var key = 0;
            foreach (var daySchedule in schedule.Schedule)
            {
                foreach (var classShedule in daySchedule)
                {
                    if (!classShedule[0].Equals("-"))
                    {
                        if (!subjects.ContainsValue(classShedule[0]))
                        {
                            subjects.TryAdd(key, classShedule[0]);
                            key++;
                        }
                    }

                    if (!classShedule[1].Equals("-"))
                    {
                        if (!subjects.ContainsValue(classShedule[1]))
                        {
                            subjects.TryAdd(key, classShedule[1]);
                            key++;
                        }
                    }
                }
            }

            foreach (var item in subjects)
            {
                list.Add(item.Value);
            }

            return list;
        }
    }
}
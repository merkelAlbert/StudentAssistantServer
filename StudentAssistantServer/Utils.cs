using System;
using System.Collections.Generic;

namespace StudentAssistantServer
{
    public class Utils
    {
        public static int GetWeekNumber(DateTime startDate)
        {
            while (!startDate.Date.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                startDate = startDate.AddDays(1);
            }

            DateTime firstSunday = startDate;
            var result = Convert.ToInt32(Math.Ceiling((DateTime.Now.Date - firstSunday.Date)
                                                      .TotalDays / 7 + 1));
            return result;
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
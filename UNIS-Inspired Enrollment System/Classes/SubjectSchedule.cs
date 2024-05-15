using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    public class SubjectSchedule
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int AcademicYearId { get; set; }
        public string Day { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }

        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string AcademicYearName { get; set; }

        public SubjectSchedule() { }

        public SubjectSchedule(int id, int subjectId, int academicYearId, string day, TimeSpan timeStart, TimeSpan timeEnd, string subjectCode, string subjectName, string academicYearName)
        {
            Id = id;
            SubjectId = subjectId;
            AcademicYearId = academicYearId;
            Day = day;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            SubjectCode = subjectCode;
            SubjectName = subjectName;
            AcademicYearName = academicYearName;
        }

        public static bool AddSubjectSchedule(int subjectId, int academicYearId, string day, TimeSpan timeStart, TimeSpan timeEnd)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check for conflicts
                string conflictQuery = @"
                    SELECT COUNT(*) FROM SubjectSchedules
                    WHERE SubjectId IN (
                        SELECT SubjectId FROM CourseSubjects WHERE CourseId IN (
                            SELECT CourseId FROM CourseSubjects WHERE SubjectId = @SubjectId
                        )
                    ) AND Day = @Day AND AcademicYearId = @AcademicYearId AND
                    ((@TimeStart >= TimeStart AND @TimeStart < TimeEnd) OR
                    (@TimeEnd > TimeStart AND @TimeEnd <= TimeEnd) OR
                    (@TimeStart < TimeStart AND @TimeEnd > TimeEnd))";

                using (SqlCommand checkCommand = new SqlCommand(conflictQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    checkCommand.Parameters.AddWithValue("@AcademicYearId", academicYearId);
                    checkCommand.Parameters.AddWithValue("@Day", day);
                    checkCommand.Parameters.AddWithValue("@TimeStart", timeStart);
                    checkCommand.Parameters.AddWithValue("@TimeEnd", timeEnd);

                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                // Insert new schedule if no conflicts
                string insertQuery = @"
                    INSERT INTO SubjectSchedules (SubjectId, AcademicYearId, Day, TimeStart, TimeEnd)
                    VALUES (@SubjectId, @AcademicYearId, @Day, @TimeStart, @TimeEnd)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    insertCommand.Parameters.AddWithValue("@AcademicYearId", academicYearId);
                    insertCommand.Parameters.AddWithValue("@Day", day);
                    insertCommand.Parameters.AddWithValue("@TimeStart", timeStart);
                    insertCommand.Parameters.AddWithValue("@TimeEnd", timeEnd);

                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool UpdateSubjectSchedule(int id, int subjectId, int academicYearId, string day, TimeSpan timeStart, TimeSpan timeEnd)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check for conflicts
                string conflictQuery = @"
                    SELECT COUNT(*) FROM SubjectSchedules
                    WHERE Id != @Id AND SubjectId IN (
                        SELECT SubjectId FROM CourseSubjects WHERE CourseId IN (
                            SELECT CourseId FROM CourseSubjects WHERE SubjectId = @SubjectId
                        )
                    ) AND Day = @Day AND AcademicYearId = @AcademicYearId AND
                    ((@TimeStart >= TimeStart AND @TimeStart < TimeEnd) OR
                    (@TimeEnd > TimeStart AND @TimeEnd <= TimeEnd) OR
                    (@TimeStart < TimeStart AND @TimeEnd > TimeEnd))";

                using (SqlCommand checkCommand = new SqlCommand(conflictQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    checkCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    checkCommand.Parameters.AddWithValue("@AcademicYearId", academicYearId);
                    checkCommand.Parameters.AddWithValue("@Day", day);
                    checkCommand.Parameters.AddWithValue("@TimeStart", timeStart);
                    checkCommand.Parameters.AddWithValue("@TimeEnd", timeEnd);

                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                // Update the schedule if no conflicts
                string updateQuery = @"
                    UPDATE SubjectSchedules
                    SET SubjectId = @SubjectId, AcademicYearId = @AcademicYearId, Day = @Day, TimeStart = @TimeStart, TimeEnd = @TimeEnd
                    WHERE Id = @Id";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    updateCommand.Parameters.AddWithValue("@AcademicYearId", academicYearId);
                    updateCommand.Parameters.AddWithValue("@Day", day);
                    updateCommand.Parameters.AddWithValue("@TimeStart", timeStart);
                    updateCommand.Parameters.AddWithValue("@TimeEnd", timeEnd);

                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool DeleteSubjectSchedule(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM SubjectSchedules WHERE Id = @Id", connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", id);
                    if (deleteCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static List<SubjectSchedule> GetSubjectSchedules()
        {
            List<SubjectSchedule> subjectSchedules = new List<SubjectSchedule>();
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT ss.Id, ss.SubjectId, ss.AcademicYearId, ss.Day, ss.TimeStart, ss.TimeEnd,
                           s.Code AS SubjectCode, s.Name AS SubjectName, ay.Name AS AcademicYearName
                    FROM SubjectSchedules ss
                    JOIN Subjects s ON ss.SubjectId = s.Id
                    JOIN AcademicYears ay ON ss.AcademicYearId = ay.Id";

                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjectSchedules.Add(new SubjectSchedule
                            {
                                Id = reader.GetInt32(0),
                                SubjectId = reader.GetInt32(1),
                                AcademicYearId = reader.GetInt32(2),
                                Day = reader.GetString(3),
                                TimeStart = reader.GetTimeSpan(4),
                                TimeEnd = reader.GetTimeSpan(5),
                                SubjectCode = reader.GetString(6),
                                SubjectName = reader.GetString(7),
                                AcademicYearName = reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return subjectSchedules;
        }

        // Create the method to get all subject schedules connected to a course for the current academic year and semester where a student is admitted based on their admission
        public static List<SubjectSchedule> GetSubjectSchedulesBySemesterAndAcademicYear(string semesterName, string academicYearName)
        {
            List<SubjectSchedule> subjectSchedules = new List<SubjectSchedule>();
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT ss.Id, ss.SubjectId, ss.AcademicYearId, ss.Day, ss.TimeStart, ss.TimeEnd,
       s.Code AS SubjectCode, s.Name AS SubjectName, ay.Name AS AcademicYearName
FROM SubjectSchedules ss
JOIN Subjects s ON ss.SubjectId = s.Id
JOIN AcademicYears ay ON ss.AcademicYearId = ay.Id
JOIN Courses c ON s.Id IN (
    SELECT SubjectId FROM CourseSubjects WHERE CourseId = c.Id
)
JOIN Admissions a ON c.Id = a.CourseId
JOIN Semesters sem ON a.SemesterId = sem.Id
WHERE ay.Name = @AcademicYearName AND sem.Name = @SemesterName";

                using (SqlCommand selectCommand = new SqlCommand(query, connection))
                {
                    selectCommand.Parameters.AddWithValue("@AcademicYearName", academicYearName);
                    selectCommand.Parameters.AddWithValue("@SemesterName", semesterName);

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjectSchedules.Add(new SubjectSchedule
                            {
                                Id = reader.GetInt32(0),
                                SubjectId = reader.GetInt32(1),
                                AcademicYearId = reader.GetInt32(2),
                                Day = reader.GetString(3),
                                TimeStart = reader.GetTimeSpan(4),
                                TimeEnd = reader.GetTimeSpan(5),
                                SubjectCode = reader.GetString(6),
                                SubjectName = reader.GetString(7),
                                AcademicYearName = reader.GetString(8)
                            });
                        }
                    }
                }
            }

            return subjectSchedules;
        }
    }
}

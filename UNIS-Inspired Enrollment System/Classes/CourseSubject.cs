using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    class CourseSubject
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int CourseId { get; set; }
        public int YearLevelId { get; set; }
        public int SemesterId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
        public string YearLevelName { get; set; }
        public string SemesterName { get; set; }

        public CourseSubject()
        {

        }

        public CourseSubject(int id, int subjectId, int courseId, int yearLevelId, int semesterId, string subjectCode, string subjectName, string courseName, string yearLevelName, string semesterName)
        {
            Id = id;
            SubjectId = subjectId;
            CourseId = courseId;
            YearLevelId = yearLevelId;
            SemesterId = semesterId;
            SubjectCode = subjectCode;
            SubjectName = subjectName;
            CourseName = courseName;
            YearLevelName = yearLevelName;
            SemesterName = semesterName;
        }

        public bool AddCourseSubject(int subjectId, int courseId, int yearLevelId, int semesterId)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM CourseSubjects WHERE SubjectId = @SubjectId AND CourseId = @CourseId AND YearLevelId = @YearLevelId AND SemesterId = @SemesterId", connection))
                {
                    checkCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    checkCommand.Parameters.AddWithValue("@CourseId", courseId);
                    checkCommand.Parameters.AddWithValue("@YearLevelId", yearLevelId);
                    checkCommand.Parameters.AddWithValue("@SemesterId", semesterId);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO CourseSubjects (SubjectId, CourseId, YearLevelId, SemesterId) VALUES (@SubjectId, @CourseId, @YearLevelId, @SemesterId)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    insertCommand.Parameters.AddWithValue("@CourseId", courseId);
                    insertCommand.Parameters.AddWithValue("@YearLevelId", yearLevelId);
                    insertCommand.Parameters.AddWithValue("@SemesterId", semesterId);
                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateCourseSubject(int id, int subjectId, int courseId, int yearLevelId, int semesterId)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM CourseSubjects WHERE SubjectId = @SubjectId AND CourseId = @CourseId AND YearLevelId = @YearLevelId AND SemesterId = @SemesterId AND Id != @Id", connection))
                {
                    checkCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    checkCommand.Parameters.AddWithValue("@CourseId", courseId);
                    checkCommand.Parameters.AddWithValue("@YearLevelId", yearLevelId);
                    checkCommand.Parameters.AddWithValue("@SemesterId", semesterId);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand updateCommand = new SqlCommand("UPDATE CourseSubjects SET SubjectId = @SubjectId, CourseId = @CourseId, YearLevelId = @YearLevelId, SemesterId = @SemesterId WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@SubjectId", subjectId);
                    updateCommand.Parameters.AddWithValue("@CourseId", courseId);
                    updateCommand.Parameters.AddWithValue("@YearLevelId", yearLevelId);
                    updateCommand.Parameters.AddWithValue("@SemesterId", semesterId);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteCourseSubject(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM CourseSubjects WHERE Id = @Id", connection))
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

        public List<CourseSubject> GetCourseSubjects()
        {
            List<CourseSubject> courseSubjects = new List<CourseSubject>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT CourseSubjects.Id, Subjects.Id AS SubjectId, Courses.Id AS CourseId, YearLevels.Id AS YearLevelId, Semesters.Id AS SemesterId, Subjects.Code AS SubjectCode, Subjects.Name AS SubjectName, Courses.Name AS CourseName, YearLevels.Name AS YearLevelName, Semesters.Name AS SemesterName FROM CourseSubjects INNER JOIN Subjects ON CourseSubjects.SubjectId = Subjects.Id INNER JOIN Courses ON CourseSubjects.CourseId = Courses.Id INNER JOIN YearLevels ON CourseSubjects.YearLevelId = YearLevels.Id INNER JOIN Semesters ON CourseSubjects.SemesterId = Semesters.Id", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courseSubjects.Add(new CourseSubject(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetString(5), reader.GetString(6), reader.GetString(7), reader.GetString(8), reader.GetString(9)));
                        }
                    }
                }
            }

            return courseSubjects;
        }
    }
}

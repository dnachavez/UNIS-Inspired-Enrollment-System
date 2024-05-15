using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DeparmentId { get; set; }
        public string DepartmentName { get; set; }

        public Course()
        {

        }

        public Course(int id, string name, int departmentId, string departmentName)
        {
            Id = id;
            Name = name;
            DeparmentId = departmentId;
            DepartmentName = departmentName;
        }

        public bool AddCourse(string name, int departmentId)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Courses WHERE Name = @Name AND DepartmentId = @DepartmentId", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    checkCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Courses (Name, DepartmentId) VALUES (@Name, @DepartmentId)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateCourse(int id, string name, int departmentId)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Courses WHERE Name = @Name AND DepartmentId = @DepartmentId AND Id != @Id", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    checkCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand updateCommand = new SqlCommand("UPDATE Courses SET Name = @Name, DepartmentId = @DepartmentId WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", name);
                    updateCommand.Parameters.AddWithValue("@DepartmentId", departmentId);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteCourse(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM Courses WHERE Id = @Id", connection))
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

        public List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT Courses.Id, Courses.Name, Courses.DepartmentId, Departments.Name FROM Courses INNER JOIN Departments ON Courses.DepartmentId = Departments.Id", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3)));
                        }
                    }
                }
            }

            return courses;
        }
    }
}

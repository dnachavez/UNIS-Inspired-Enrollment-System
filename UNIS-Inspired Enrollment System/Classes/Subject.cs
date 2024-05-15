using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public Subject()
        {

        }

        public Subject(int id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public bool AddSubject(string name, string code)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE Name = @Name AND Code = @Code", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    checkCommand.Parameters.AddWithValue("@Code", code);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Subjects (Name, Code) VALUES (@Name, @Code)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Code", code);
                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateSubject(int id, string name, string code)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Subjects WHERE Name = @Name AND Code = @Code AND Id != @Id", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    checkCommand.Parameters.AddWithValue("@Code", code);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                using (SqlCommand updateCommand = new SqlCommand("UPDATE Subjects SET Name = @Name, Code = @Code WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", name);
                    updateCommand.Parameters.AddWithValue("@Code", code);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteSubject(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM Subjects WHERE Id = @Id", connection))
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

        public List<Subject> GetSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM Subjects", connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }

            return subjects;
        }

        public List<Subject> GetSubjectsConnectedToCourses()
        {
            List<Subject> subjects = new List<Subject>();
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT DISTINCT s.Id, s.Name, s.Code
                    FROM Subjects s
                    JOIN CourseSubjects cs ON s.Id = cs.SubjectId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Code = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return subjects;
        }
    }
}

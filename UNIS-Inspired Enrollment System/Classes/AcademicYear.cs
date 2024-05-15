using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class AcademicYear
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public AcademicYear()
        {

        }

        public AcademicYear(int id, string name, int status)
        {
            Id = id;
            Name = name;
            Status = status;
        }

        public bool AddAcademicYear(string name, int status)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM AcademicYears WHERE Name = @Name", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                if (status == 1)
                {
                    using (SqlCommand disableOthersCommand = new SqlCommand("UPDATE AcademicYears SET Status = 0 WHERE Status = 1", connection))
                    {
                        disableOthersCommand.ExecuteNonQuery();
                    }
                }

                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO AcademicYears (Name, Status) VALUES (@Name, @Status)", connection))
                {
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Status", status);
                    if (insertCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateAcademicYear(int id, string name, int status)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM AcademicYears WHERE Name = @Name AND Id != @Id", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", name);
                    checkCommand.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCommand.ExecuteScalar() > 0)
                    {
                        return false;
                    }
                }

                if (status == 1)
                {
                    using (SqlCommand disableOthersCommand = new SqlCommand("UPDATE AcademicYears SET Status = 0 WHERE Status = 1 AND Id != @Id", connection))
                    {
                        disableOthersCommand.Parameters.AddWithValue("@Id", id);
                        disableOthersCommand.ExecuteNonQuery();
                    }
                }

                using (SqlCommand updateCommand = new SqlCommand("UPDATE AcademicYears SET Name = @Name, Status = @Status WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", name);
                    updateCommand.Parameters.AddWithValue("@Status", status);
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    if (updateCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteAcademicYear(int id)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM AcademicYears WHERE Id = @Id", connection))
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

        public List<AcademicYear> GetAcademicYears()
        {
            List<AcademicYear> academicYears = new List<AcademicYear>();

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM AcademicYears", connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            academicYears.Add(new AcademicYear(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                        }
                    }
                }
            }

            return academicYears;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    public class Admission
    {
        public int AdmissionID { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int CourseId { get; set; }
        public int YearLevelId { get; set; }
        public int AdmissionTypeId { get; set; }
        public int SemesterId { get; set; }
        public int StudentDetailsId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }

        // Additional properties for displaying in the DataGrid
        public string AcademicYearName { get; set; }
        public string CourseName { get; set; }
        public string YearLevelName { get; set; }
        public string AdmissionTypeName { get; set; }
        public string SemesterName { get; set; }
        public string GenderName => Gender == 0 ? "Male" : "Female";

        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\DAN\\source\\repos\\UNIS-Inspired Enrollment System\\UNIS-Inspired Enrollment System\\Database.mdf;Integrated Security=True";

        public bool AddAdmission()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertAdmissionQuery = @"
            INSERT INTO Admissions (AcademicYearId, AdmissionDate, CourseId, YearLevelId, AdmissionTypeId, SemesterId)
            OUTPUT INSERTED.Id
            VALUES (@AcademicYearId, @AdmissionDate, @CourseId, @YearLevelId, @AdmissionTypeId, @SemesterId)";

                using (SqlCommand insertAdmissionCommand = new SqlCommand(insertAdmissionQuery, connection))
                {
                    insertAdmissionCommand.Parameters.AddWithValue("@AcademicYearId", AcademicYearId);
                    insertAdmissionCommand.Parameters.AddWithValue("@AdmissionDate", AdmissionDate);
                    insertAdmissionCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    insertAdmissionCommand.Parameters.AddWithValue("@YearLevelId", YearLevelId);
                    insertAdmissionCommand.Parameters.AddWithValue("@AdmissionTypeId", AdmissionTypeId);
                    insertAdmissionCommand.Parameters.AddWithValue("@SemesterId", SemesterId);

                    AdmissionID = (int)insertAdmissionCommand.ExecuteScalar();
                }

                string insertStudentDetailsQuery = @"
            INSERT INTO StudentDetails (AdmissionId, FirstName, MiddleName, LastName, Gender, DateOfBirth, MobileNo, Email, City, State, PermanentAddress, CurrentAddress)
            VALUES (@AdmissionId, @FirstName, @MiddleName, @LastName, @Gender, @DateOfBirth, @MobileNo, @Email, @City, @State, @PermanentAddress, @CurrentAddress)";

                using (SqlCommand insertStudentDetailsCommand = new SqlCommand(insertStudentDetailsQuery, connection))
                {
                    insertStudentDetailsCommand.Parameters.AddWithValue("@AdmissionId", AdmissionID);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@FirstName", FirstName);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@MiddleName", MiddleName);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@LastName", LastName);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@Gender", Gender);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@Email", Email);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@City", City);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@State", State);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@PermanentAddress", PermanentAddress);
                    insertStudentDetailsCommand.Parameters.AddWithValue("@CurrentAddress", CurrentAddress);

                    if (insertStudentDetailsCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateAdmission()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateAdmissionQuery = @"
            UPDATE Admissions
            SET AcademicYearId = @AcademicYearId, AdmissionDate = @AdmissionDate, CourseId = @CourseId, YearLevelId = @YearLevelId, AdmissionTypeId = @AdmissionTypeId, SemesterId = @SemesterId
            WHERE Id = @AdmissionID";

                using (SqlCommand updateAdmissionCommand = new SqlCommand(updateAdmissionQuery, connection))
                {
                    updateAdmissionCommand.Parameters.AddWithValue("@AcademicYearId", AcademicYearId);
                    updateAdmissionCommand.Parameters.AddWithValue("@AdmissionDate", AdmissionDate);
                    updateAdmissionCommand.Parameters.AddWithValue("@CourseId", CourseId);
                    updateAdmissionCommand.Parameters.AddWithValue("@YearLevelId", YearLevelId);
                    updateAdmissionCommand.Parameters.AddWithValue("@AdmissionTypeId", AdmissionTypeId);
                    updateAdmissionCommand.Parameters.AddWithValue("@SemesterId", SemesterId);
                    updateAdmissionCommand.Parameters.AddWithValue("@AdmissionID", AdmissionID);

                    if (updateAdmissionCommand.ExecuteNonQuery() <= 0)
                    {
                        return false;
                    }
                }

                string updateStudentDetailsQuery = @"
            UPDATE StudentDetails
            SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Gender = @Gender, DateOfBirth = @DateOfBirth, MobileNo = @MobileNo, Email = @Email, City = @City, State = @State, PermanentAddress = @PermanentAddress, CurrentAddress = @CurrentAddress
            WHERE AdmissionId = @AdmissionID";

                using (SqlCommand updateStudentDetailsCommand = new SqlCommand(updateStudentDetailsQuery, connection))
                {
                    updateStudentDetailsCommand.Parameters.AddWithValue("@FirstName", FirstName);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@MiddleName", MiddleName);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@LastName", LastName);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@Gender", Gender);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@Email", Email);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@City", City);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@State", State);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@PermanentAddress", PermanentAddress);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@CurrentAddress", CurrentAddress);
                    updateStudentDetailsCommand.Parameters.AddWithValue("@AdmissionID", AdmissionID);

                    if (updateStudentDetailsCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Create the method to delete a student's admission including his/her details by Id
        public bool DeleteAdmission(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteStudentDetailsQuery = @"
            DELETE FROM StudentDetails
            WHERE AdmissionId = @AdmissionID";

                using (SqlCommand deleteStudentDetailsCommand = new SqlCommand(deleteStudentDetailsQuery, connection))
                {
                    deleteStudentDetailsCommand.Parameters.AddWithValue("@AdmissionID", id);

                    if (deleteStudentDetailsCommand.ExecuteNonQuery() <= 0)
                    {
                        return false;
                    }
                }

                string deleteAdmissionQuery = @"
            DELETE FROM Admissions
            WHERE Id = @AdmissionID";

                using (SqlCommand deleteAdmissionCommand = new SqlCommand(deleteAdmissionQuery, connection))
                {
                    deleteAdmissionCommand.Parameters.AddWithValue("@AdmissionID", id);

                    if (deleteAdmissionCommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<Admission> GetAdmissions()
        {
            List<Admission> admissions = new List<Admission>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
            SELECT a.Id, a.AcademicYearId, ay.Name AS AcademicYearName, 
                   CONVERT(date, a.AdmissionDate) AS AdmissionDate, -- Convert the AdmissionDate to date only
                   a.CourseId, c.Name AS CourseName,
                   a.YearLevelId, yl.Name AS YearLevelName, a.AdmissionTypeId, at.Name AS AdmissionTypeName, a.SemesterId, s.Name AS SemesterName,
                   sd.Id AS StudentDetailsId, sd.FirstName, sd.MiddleName, sd.LastName, sd.Gender, sd.DateOfBirth, sd.MobileNo, sd.Email, sd.City, sd.State, sd.PermanentAddress, sd.CurrentAddress
            FROM Admissions a
            JOIN AcademicYears ay ON a.AcademicYearId = ay.Id
            JOIN Courses c ON a.CourseId = c.Id
            JOIN YearLevels yl ON a.YearLevelId = yl.Id
            JOIN AdmissionTypes at ON a.AdmissionTypeId = at.Id
            JOIN Semesters s ON a.SemesterId = s.Id
            JOIN StudentDetails sd ON a.Id = sd.AdmissionId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            admissions.Add(new Admission
                            {
                                AdmissionID = reader.GetInt32(0),
                                AcademicYearId = reader.GetInt32(1),
                                AcademicYearName = reader.GetString(2),
                                AdmissionDate = reader.GetDateTime(3), // This will now only have the date part
                                CourseId = reader.GetInt32(4),
                                CourseName = reader.GetString(5),
                                YearLevelId = reader.GetInt32(6),
                                YearLevelName = reader.GetString(7),
                                AdmissionTypeId = reader.GetInt32(8),
                                AdmissionTypeName = reader.GetString(9),
                                SemesterId = reader.GetInt32(10),
                                SemesterName = reader.GetString(11),
                                StudentDetailsId = reader.GetInt32(12),
                                FirstName = reader.GetString(13),
                                MiddleName = reader.GetString(14),
                                LastName = reader.GetString(15),
                                Gender = reader.GetInt32(16),
                                DateOfBirth = reader.GetDateTime(17),
                                MobileNo = reader.GetString(18),
                                Email = reader.GetString(19),
                                City = reader.GetString(20),
                                State = reader.GetString(21),
                                PermanentAddress = reader.GetString(22),
                                CurrentAddress = reader.GetString(23)
                            });
                        }
                    }
                }
            }

            return admissions;
        }
    }
}

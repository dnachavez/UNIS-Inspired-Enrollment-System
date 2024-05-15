using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIS_Inspired_Enrollment_System.Classes
{
    internal class Teacher
    {
        public int TeacherId { get; set; }
        public int DepartmentId { get; set; }
        public int Gender { get; set; }
        public int TeacherLoadId { get; set; }
        public int SubjectId { get; set; }
        public int AcademicYearId { get; set; }
        public int SemesterId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PermanentAddress { get; set; }
        public string CurrentAddress { get; set; }
    }
}

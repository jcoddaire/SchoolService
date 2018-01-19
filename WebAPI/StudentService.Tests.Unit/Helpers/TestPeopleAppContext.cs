using StudentService.Data;
using StudentService.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentService.Tests.Unit.Helpers
{
    [ExcludeFromCodeCoverage]
    public class TestPeopleAppContext : IStudentService
    {
        public List<PersonDTO> People { get; set; }

        public TestPeopleAppContext()
        {
            People = new List<PersonDTO>();
        }

        public IEnumerable<PersonDTO> GetAllPersons()
        {
            return People;
        }

        public PersonDTO GetPerson(int personID)
        {
            throw new NotImplementedException();
        }

        public PersonDTO CreatePerson(PersonDTO person)
        {
            throw new NotImplementedException();
        }

        public PersonDTO UpdatePerson(PersonDTO person)
        {
            throw new NotImplementedException();
        }

        public int DeletePerson(int personID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            throw new NotImplementedException();
        }

        public DepartmentDTO GetDepartment(int departmentID)
        {
            throw new NotImplementedException();
        }

        public DepartmentDTO CreateDepartment(DepartmentDTO department)
        {
            throw new NotImplementedException();
        }

        public DepartmentDTO UpdateDepartment(DepartmentDTO department)
        {
            throw new NotImplementedException();
        }

        public int DeleteDepartment(int departmentID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CourseDTO> GetAllCourses()
        {
            throw new NotImplementedException();
        }

        public CourseDTO GetCourse(int courseID)
        {
            throw new NotImplementedException();
        }

        public CourseDTO CreateCourse(CourseDTO course)
        {
            throw new NotImplementedException();
        }

        public CourseDTO UpdateCourse(CourseDTO course)
        {
            throw new NotImplementedException();
        }

        public int DeleteCourse(int courseID)
        {
            throw new NotImplementedException();
        }
    }
}

using StudentService.Data;
using StudentService.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
            return People.Where(x => x.PersonID == personID).FirstOrDefault();
        }

        public PersonDTO CreatePerson(PersonDTO person)
        {
            person.PersonID = People.Max(x => x.PersonID) + 1;

            People.Add(person);

            return person;
        }

        public PersonDTO UpdatePerson(PersonDTO person)
        {
            People.Remove(People.Where(x => x.PersonID == person.PersonID).First());
            People.Add(person);

            return person;
        }

        public int DeletePerson(int personID)
        {
            if(People.Remove(People.Where(x => x.PersonID == personID).First()))
            {
                return 1;
            }
            return -1;
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

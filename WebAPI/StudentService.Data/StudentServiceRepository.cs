﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentService.DTOs;

namespace StudentService.Data
{
    public class StudentServiceRepository : IStudentService
    {
        private StudentDB _Database = null;

        private StudentDB Database
        {
            get
            {
                if (_Database == null)
                {
                    _Database = new StudentDB();
                }

                return _Database;
            }
        }
        public StudentServiceRepository()
        {

        }

        #region People Methods

        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonDTO> GetAllPersons()
        {
            var sourcePeople = Database.People.Select(
                x => new PersonDTO()
                {
                    PersonID = x.PersonID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate,
                    EnrollmentDate = x.EnrollmentDate

                }).ToList();

            return sourcePeople;
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// A person DTO object. If no person was found, returns null.
        /// </returns>
        public PersonDTO GetPerson(int personID)
        {
            if(personID <= 0)
            {
                return null;
            }

            var personOfInterest = Database.People.Where(p => p.PersonID == personID).Select(
                x => new PersonDTO()
                {
                    PersonID = x.PersonID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate,
                    EnrollmentDate = x.EnrollmentDate

                }).FirstOrDefault();

            if(personOfInterest != null && personOfInterest.PersonID > 0)
            {
                return personOfInterest;
            }

            return null;
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public PersonDTO CreatePerson(PersonDTO person)
        {
            var newPeep = new Person
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                EnrollmentDate = person.EnrollmentDate,
                HireDate = person.HireDate
            };

            Database.People.Add(newPeep);
            Database.SaveChanges();

            //leap of faith, apparently the newPeep object has the ID.
            person.PersonID = newPeep.PersonID;

            return person;
        }

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public PersonDTO UpdatePerson(PersonDTO person)
        {
            var changedPeep = Database.People.Where(p => p.PersonID == person.PersonID).FirstOrDefault();
            if(changedPeep == null || changedPeep.PersonID != person.PersonID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            changedPeep.FirstName = person.FirstName;
            changedPeep.LastName = person.LastName;
            changedPeep.EnrollmentDate = person.EnrollmentDate;
            changedPeep.HireDate = person.HireDate;
            
            Database.People.Attach(changedPeep);

            var entry = Database.Entry(changedPeep);
            entry.Property(e => e.FirstName).IsModified = true;
            entry.Property(e => e.LastName).IsModified = true;
            entry.Property(e => e.EnrollmentDate).IsModified = true;
            entry.Property(e => e.HireDate).IsModified = true;

            Database.SaveChanges();

            return person;
        }

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// The number of rows affected.
        /// If something failed, returns -1.
        /// </returns>
        public int DeletePerson(int personID)
        {
            var target = Database.People.Where(x => x.PersonID == personID).FirstOrDefault();
            
            if(target != null && target.PersonID == personID)
            {
                Database.People.Remove(target);
                return Database.SaveChanges();                
            }
            return -1;
        }

        #endregion

        #region Department Methods

        public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            var source = Database.Departments.Select(
                x => new DepartmentDTO()
                {
                    DepartmentID = x.DepartmentID,
                    Name = x.Name,
                    Budget = x.Budget,
                    StartDate = x.StartDate,
                    Administrator = x.Administrator

                }).ToList();

            return source;
        }

        public DepartmentDTO GetDepartment(int departmentID)
        {
            if (departmentID <= 0)
            {
                return null;
            }

            var target = Database.Departments.Where(d => d.DepartmentID == departmentID).Select(
                x => new DepartmentDTO()
                {
                    DepartmentID = x.DepartmentID,
                    Name = x.Name,
                    Budget = x.Budget,
                    StartDate = x.StartDate,
                    Administrator = x.Administrator

                }).FirstOrDefault();

            if (target != null && target.DepartmentID > 0)
            {
                return target;
            }

            return null;
        }

        public DepartmentDTO CreateDepartment(DepartmentDTO department)
        {
            var topID = GetAllDepartments().Max(x => x.DepartmentID) + 1;

            var obj = new Department
            {
                DepartmentID = topID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = department.Administrator
            };

            Database.Departments.Add(obj);
            Database.SaveChanges();

            department.DepartmentID = topID;

            return department;
        }

        public DepartmentDTO UpdateDepartment(DepartmentDTO department)
        {
            var changed = Database.Departments.Where(d => d.DepartmentID == department.DepartmentID).FirstOrDefault();
            if (changed == null || changed.DepartmentID != department.DepartmentID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            changed.Name = department.Name;
            changed.Budget = department.Budget;
            changed.StartDate = department.StartDate;
            changed.Administrator = department.Administrator;

            Database.Departments.Attach(changed);

            var entry = Database.Entry(changed);
            entry.Property(e => e.Name).IsModified = true;
            entry.Property(e => e.Budget).IsModified = true;
            entry.Property(e => e.StartDate).IsModified = true;
            entry.Property(e => e.Administrator).IsModified = true;

            Database.SaveChanges();

            return department;
        }

        public int DeleteDepartment(int departmentID)
        {

            var target = Database.Departments.Where(x => x.DepartmentID == departmentID).FirstOrDefault();

            if (target != null && target.DepartmentID == departmentID)
            {
                Database.Departments.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }
        #endregion

        #region Course Methods
        
        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of courses.</returns>
        public IEnumerable<CourseDTO> GetAllCourses()
        {
            var courses = Database.Courses.Select(
                x => new CourseDTO()
                {
                    CourseID = x.CourseID,
                    Title = x.Title,
                    Credits = x.Credits,
                    DepartmentID = x.DepartmentID
                    
                }).ToList();

            return courses;
        }

        /// <summary>
        /// Gets the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>The Course DTO.</returns>
        public CourseDTO GetCourse(int courseID)
        {
            if (courseID <= 0)
            {
                return null;
            }

            var target = Database.Courses.Where(c => c.CourseID == courseID).Select(
                x => new CourseDTO()
                {
                    CourseID = x.CourseID,
                    Title = x.Title,
                    Credits = x.Credits,
                    DepartmentID = x.DepartmentID

                }).FirstOrDefault();

            if (target != null && target.CourseID > 0)
            {
                return target;
            }

            return null;
        }

        /// <summary>
        /// Creates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>The course.</returns>
        public CourseDTO CreateCourse(CourseDTO course)
        {
            var newItem = new Course
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentID = course.DepartmentID
            };

            Database.Courses.Add(newItem);
            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Updates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Could not find a matching item in the dataset.</exception>
        public CourseDTO UpdateCourse(CourseDTO course)
        {
            var changedTarget = Database.Courses.Where(p => p.CourseID == course.CourseID).FirstOrDefault();
            if (changedTarget == null || changedTarget.CourseID != course.CourseID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            //changedTarget.CourseID = course.CourseID; //this should never happen. Otherwise that ^^^ check will fail.
            changedTarget.Title = course.Title;
            changedTarget.Credits = course.Credits;
            changedTarget.DepartmentID = course.DepartmentID;

            Database.Courses.Attach(changedTarget);

            var entry = Database.Entry(changedTarget);
            //entry.Property(e => e.CourseID).IsModified = true;
            entry.Property(e => e.Title).IsModified = true;
            entry.Property(e => e.Credits).IsModified = true;
            entry.Property(e => e.DepartmentID).IsModified = true;

            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Deletes the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>The number of rows affected. Returns -1 if an error occured.</returns>
        public int DeleteCourse(int courseID)
        {
            var target = Database.Courses.Where(x => x.CourseID == courseID).FirstOrDefault();

            if (target != null && target.CourseID == courseID)
            {
                Database.Courses.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }
        #endregion
    }
}

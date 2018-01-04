using System;
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


        #region People Methods
        public StudentServiceRepository()
        {

        }

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
                throw new KeyNotFoundException("Could not find a matching ID in the dataset.");
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
        #endregion

        #region Course Methods
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
        #endregion
    }
}

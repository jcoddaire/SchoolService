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
        private StudentDB db = new StudentDB();

        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns></returns>
        public List<PersonDTO> GetAllPersons()
        {
            var sourcePeople = db.People.Select(
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

            var personOfInterest = db.People.Where(p => p.PersonID == personID).Select(
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentService.DTOs;

namespace StudentService.Data
{
    public interface IStudentService
    {
        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns></returns>
        List<PersonDTO> GetAllPersons();

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// A person DTO object. If no person was found, returns null.
        /// </returns>
        PersonDTO GetPerson(int personID);

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        PersonDTO CreatePerson(PersonDTO person);

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        PersonDTO UpdatePerson(PersonDTO person);

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// The number of rows affected.
        /// If something failed, returns -1.
        /// </returns>
        int DeletePerson(int personID);
    }
}

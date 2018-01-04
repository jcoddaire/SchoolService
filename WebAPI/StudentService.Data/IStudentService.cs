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
    }
}

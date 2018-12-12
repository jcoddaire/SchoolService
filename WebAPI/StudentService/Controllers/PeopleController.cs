using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentService.Data;
using StudentService.DTOs;

namespace StudentService.WebAPI.Controllers
{
    public class PeopleController : ControllerBase
    {
        public PeopleController()
        {
            
        }

        public PeopleController(IStudentService repository)
        {
            Repository = repository;
        }
                
        /// <summary>
        /// Gets all people in the system.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonDTO> Get()
        {
            return Repository.GetAllPersons();            
        }

        /// <summary>
        /// Gets a given person.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public PersonDTO Get(int id)
        {
            if (id <= 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var target = Repository.GetPerson(id);
            if (target != null && target.PersonID > 0)
            {
                return target;
            }

            //cannot find it, throw a 404.
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
                
        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpPost]
        public PersonDTO Post(PersonDTO person)
        {
            if(person == null)
            {
                //return 400 bad reqeust.
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //check data.
            //Not everyone has a name. Null / empty string is all valid. SO a totally empty PersonDTO object is valid.
            //See this, point 40. http://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            person = Repository.CreatePerson(person);

            return person;
        }

        /// <summary>
        /// Updates the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException">
        /// </exception>
        [HttpPut]
        public PersonDTO Put(PersonDTO person)
        {
            if (person == null)
            {
                //return 400 bad reqeust.
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if(person.PersonID <= 0)
            {
                //return 404 not found.
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var foundPerson = Repository.GetPerson(person.PersonID);
            if(foundPerson == null)
            {
                //return 404 not found.
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if(foundPerson.FirstName.Equals(person.FirstName)
                && foundPerson.LastName.Equals(person.LastName)                
                && foundPerson.HireDate.Equals(person.HireDate)
                && foundPerson.EnrollmentDate.Equals(person.EnrollmentDate))
            {
                //There are no changes to the object.
                //return 204 no change.
                return person;
            }
                        
            person = Repository.UpdatePerson(person);

            return person;
        }
                
        /// <summary>
        /// Deletes the specified person.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException">
        /// </exception>
        public HttpResponseMessage Delete(int id)
        {
            if (id <= 0)
            {
                //Return 404 not found. Could also return 400.
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var foundPerson = Repository.GetPerson(id);
            if (foundPerson == null)
            {
                //return 404 not found.
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var result = Repository.DeletePerson(id);
            if (result > 0)
            {
                //return 204 no content.
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            //something went wrong. TODO: find a better way to handle this.
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
}

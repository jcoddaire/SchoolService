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

        // GET api/People
        public IEnumerable<PersonDTO> Get()
        {
            return Repository.GetAllPersons();            
        }

        // GET api/People/5
        public PersonDTO Get(int id)
        {
            if(id <= 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var target = Repository.GetPerson(id);
            if(target != null && target.PersonID > 0)
            {
                return target;
            }

            //cannot find it, throw a 404.
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST api/values
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

            person = Repository.CreatePerson(person);

            return person;
        }

        // PUT api/values/5
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

        // DELETE api/values/5
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

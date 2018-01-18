using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StudentService.Data;
using StudentService.DTOs;

namespace StudentService.Controllers
{
    public class PeopleController : ControllerBase
    {
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
                return null;
            }

            var target = Repository.GetPerson(id);
            if(target != null && target.PersonID > 0)
            {
                return target;
            }

            //cannot find it, throw a 404.
            return null;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            if (id <= 0)
            {
                //Return 404 not found.
            }

            var result = Repository.DeletePerson(id);
            if (result > 0)
            {
                //return 204 no content.
            }

            //cannot find the id, throw a 404.            
        }
    }
}

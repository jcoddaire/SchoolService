using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using StudentService.WebAPI.Controllers;
using StudentService.Tests.Unit.Helpers;
using System.Web.Http;
using StudentService.DTOs;

namespace StudentService.Tests.Unit.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PeopleControllerTest
    {
        [TestMethod]
        public void People_Get_All_Valid()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var controller = new PeopleController(context);
            var result = controller.Get().ToList();

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual("Amber", result[0].FirstName);
            Assert.AreEqual("Ben", result[1].FirstName);
            Assert.AreEqual("Ronald", result[2].FirstName);

            Assert.AreEqual("Baar", result[0].LastName);
            Assert.AreEqual("Xavier", result[1].LastName);
            Assert.AreEqual("Weasley", result[2].LastName);

            Assert.AreEqual(3, result[0].PersonID);
            Assert.AreEqual(1, result[1].PersonID);
            Assert.AreEqual(5, result[2].PersonID);
        }

        #region GetPeopleByID

        [TestMethod]
        public void People_Get_ID_LessThan0()
        {
            var context = new TestPeopleAppContext();            
            var controller = new PeopleController(context);            
                        
            //iterate 10 times. Int.MinValue takes too long.
            for(int id = 0; id >= -10; id--)
            {
                try
                {
                    var result = controller.Get(id);
                }
                catch (HttpResponseException ex)
                {
                    Assert.IsNotNull(ex);
                    Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
                }
            }
        }

        [TestMethod]
        public void People_Get_ID_PersonDoesNotExist()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();
            var controller = new PeopleController(context);

            var MISSING_ID = 8;

            try
            {
                var result = controller.Get(MISSING_ID);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }            
        }

        [TestMethod]
        public void People_Get_ID_PersonDoesExist()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();
            var controller = new PeopleController(context);

            var FOUND_PERSON = context.People[0];            
            var result = controller.Get(FOUND_PERSON.PersonID);

            Assert.AreEqual(FOUND_PERSON.PersonID, result.PersonID);
            Assert.AreEqual(FOUND_PERSON.FirstName, result.FirstName);
            Assert.AreEqual(FOUND_PERSON.LastName, result.LastName);
            Assert.AreEqual(FOUND_PERSON.HireDate, result.HireDate);
            Assert.AreEqual(FOUND_PERSON.EnrollmentDate, result.EnrollmentDate);
        }

        #endregion

        #region PostPerson
        [TestMethod]
        public void People_Post_null()
        {
            var context = new TestPeopleAppContext();
            var controller = new PeopleController(context);

            try
            {
                var result = controller.Post(null);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ex.Response.StatusCode);
            }
        }
        
        [TestMethod]
        public void People_Post_ValidPerson_random_name()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();

            var EXPECTED_ID = context.People.Max(x => x.PersonID) + 1;
            var FIRST_NAME = "TEST";
            var LAST_NAME = "SUBJECT";
            var ENROLLMENT_DATE = DateTime.Today;


            var controller = new PeopleController(context);
            var newPerson = new PersonDTO { FirstName = FIRST_NAME, LastName = LAST_NAME, EnrollmentDate = ENROLLMENT_DATE};

            var result = controller.Post(newPerson);

            Assert.AreEqual(EXPECTED_ID, result.PersonID);
            Assert.AreEqual(FIRST_NAME, result.FirstName);
            Assert.AreEqual(LAST_NAME, result.LastName);
            Assert.AreEqual(ENROLLMENT_DATE, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT + 1, context.People.Count());
        }

        [TestMethod]
        public void People_Post_ValidPerson_no_name_empty_string()
        {
            //See this, point 40. http://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();

            var EXPECTED_ID = context.People.Max(x => x.PersonID) + 1;
            var FIRST_NAME = string.Empty;
            var LAST_NAME = string.Empty;
            var ENROLLMENT_DATE = DateTime.Today;


            var controller = new PeopleController(context);
            var newPerson = new PersonDTO { FirstName = FIRST_NAME, LastName = LAST_NAME, EnrollmentDate = ENROLLMENT_DATE };

            var result = controller.Post(newPerson);

            Assert.AreEqual(EXPECTED_ID, result.PersonID);
            Assert.AreEqual(FIRST_NAME, result.FirstName);
            Assert.AreEqual(LAST_NAME, result.LastName);
            Assert.AreEqual(ENROLLMENT_DATE, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT + 1, context.People.Count());
        }

        [TestMethod]
        public void People_Post_ValidPerson_no_name_null()
        {
            //See this, point 40. http://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();

            var EXPECTED_ID = context.People.Max(x => x.PersonID) + 1;
            string FIRST_NAME = null;
            string LAST_NAME = null;
            var ENROLLMENT_DATE = DateTime.Today;

            var controller = new PeopleController(context);
            var newPerson = new PersonDTO { FirstName = FIRST_NAME, LastName = LAST_NAME, EnrollmentDate = ENROLLMENT_DATE };

            var result = controller.Post(newPerson);

            Assert.AreEqual(EXPECTED_ID, result.PersonID);
            Assert.AreEqual(FIRST_NAME, result.FirstName);
            Assert.AreEqual(LAST_NAME, result.LastName);
            Assert.AreEqual(ENROLLMENT_DATE, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT + 1, context.People.Count());
        }

        #endregion

        #region PutPerson

        [TestMethod]
        public void People_Put_null()
        {
            var context = new TestPeopleAppContext();
            var controller = new PeopleController(context);

            try
            {
                var result = controller.Put(null);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, ex.Response.StatusCode);
            }
        }

        [TestMethod]
        public void People_Put_InvalidPerson_IDLessThanZero()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();
            
            var UPDATED_FIRST_NAME = "Adam";
            var UPDATED_LAST_NAME = "Smith";
            var UPDATED_ENROLLMENT_DATE = DateTime.Today;

            var controller = new PeopleController(context);

            var updatedPerson = context.People[0];
            updatedPerson.FirstName = UPDATED_FIRST_NAME;
            updatedPerson.LastName = UPDATED_LAST_NAME;
            updatedPerson.EnrollmentDate = UPDATED_ENROLLMENT_DATE;
            updatedPerson.PersonID = 0;

            try
            {
                var result = controller.Put(updatedPerson);
            }
            catch(HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }
        }

        [TestMethod]
        public void People_Put_InvalidPerson_IDDoesNotExist()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var UPDATED_FIRST_NAME = "Adam";
            var UPDATED_LAST_NAME = "Smith";
            var UPDATED_ENROLLMENT_DATE = DateTime.Today;
            var ID_SHOULD_NOT_EXIST = context.People.Max(x => x.PersonID) + 1; //this id should never exist.

            var controller = new PeopleController(context);
                        
            var updatedPerson = new PersonDTO();
            updatedPerson.PersonID = ID_SHOULD_NOT_EXIST;
            updatedPerson.FirstName = UPDATED_FIRST_NAME;
            updatedPerson.LastName = UPDATED_LAST_NAME;
            updatedPerson.EnrollmentDate = UPDATED_ENROLLMENT_DATE;            

            try
            {
                var result = controller.Put(updatedPerson);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }
        }

        [TestMethod]
        public void People_Put_ValidPerson_no_changes()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();
            
            var controller = new PeopleController(context);

            var updatedPerson = context.People[0];
            
            var result = controller.Put(updatedPerson);

            Assert.AreEqual(updatedPerson.PersonID, result.PersonID);
            Assert.AreEqual(updatedPerson.FirstName, result.FirstName);
            Assert.AreEqual(updatedPerson.LastName, result.LastName);
            Assert.AreEqual(updatedPerson.EnrollmentDate, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT, context.People.Count());
        }

        [TestMethod]
        public void People_Put_ValidPerson_random_name()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();
                        
            var UPDATED_FIRST_NAME = "Adam";
            var UPDATED_LAST_NAME = "Smith";
            var UPDATED_ENROLLMENT_DATE = DateTime.Today;
            
            var controller = new PeopleController(context);

            var updatedPerson = new PersonDTO();
            updatedPerson.PersonID = context.People[0].PersonID;
            updatedPerson.FirstName = UPDATED_FIRST_NAME;
            updatedPerson.LastName = UPDATED_LAST_NAME;
            updatedPerson.EnrollmentDate = UPDATED_ENROLLMENT_DATE;

            var result = controller.Put(updatedPerson);

            Assert.AreEqual(updatedPerson.PersonID, result.PersonID);
            Assert.AreEqual(updatedPerson.FirstName, result.FirstName);
            Assert.AreEqual(updatedPerson.LastName, result.LastName);
            Assert.AreEqual(updatedPerson.EnrollmentDate, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT, context.People.Count());
        }

        [TestMethod]
        public void People_Put_ValidPerson_no_name_empty_string()
        {
            //See this, point 40. http://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();

            var UPDATED_FIRST_NAME = "";
            var UPDATED_LAST_NAME = "";
            var UPDATED_ENROLLMENT_DATE = DateTime.Today;

            var controller = new PeopleController(context);

            var updatedPerson = new PersonDTO();
            updatedPerson.PersonID = context.People[0].PersonID;
            updatedPerson.FirstName = UPDATED_FIRST_NAME;
            updatedPerson.LastName = UPDATED_LAST_NAME;
            updatedPerson.EnrollmentDate = UPDATED_ENROLLMENT_DATE;

            var result = controller.Put(updatedPerson);

            Assert.AreEqual(updatedPerson.PersonID, result.PersonID);
            Assert.AreEqual(updatedPerson.FirstName, result.FirstName);
            Assert.AreEqual(updatedPerson.LastName, result.LastName);
            Assert.AreEqual(updatedPerson.EnrollmentDate, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT, context.People.Count());
        }

        [TestMethod]
        public void People_Put_ValidPerson_no_name_null()
        {
            //See this, point 40. http://www.kalzumeus.com/2010/06/17/falsehoods-programmers-believe-about-names/

            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();

            var CURRENT_PEOPLE_COUNT = context.People.Count();

            string UPDATED_FIRST_NAME = null;
            string UPDATED_LAST_NAME = null;
            var UPDATED_ENROLLMENT_DATE = DateTime.Today;

            var controller = new PeopleController(context);

            var updatedPerson = new PersonDTO();
            updatedPerson.PersonID = context.People[0].PersonID;
            updatedPerson.FirstName = UPDATED_FIRST_NAME;
            updatedPerson.LastName = UPDATED_LAST_NAME;
            updatedPerson.EnrollmentDate = UPDATED_ENROLLMENT_DATE;

            var result = controller.Put(updatedPerson);

            Assert.AreEqual(updatedPerson.PersonID, result.PersonID);
            Assert.AreEqual(updatedPerson.FirstName, result.FirstName);
            Assert.AreEqual(updatedPerson.LastName, result.LastName);
            Assert.AreEqual(updatedPerson.EnrollmentDate, result.EnrollmentDate);

            Assert.AreEqual(CURRENT_PEOPLE_COUNT, context.People.Count());
        }

        #endregion

        #region Delete
        [TestMethod]
        public void People_Delete_ID_LessThan0()
        {
            var context = new TestPeopleAppContext();
            var controller = new PeopleController(context);

            //iterate 10 times. Int.MinValue takes too long.
            for (int id = 0; id >= -10; id--)
            {
                try
                {
                    var result = controller.Delete(id);
                }
                catch (HttpResponseException ex)
                {
                    Assert.IsNotNull(ex);
                    Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
                }
            }
        }

        [TestMethod]
        public void People_Delete_ID_PersonDoesNotExist()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();
            var controller = new PeopleController(context);

            var MISSING_ID = 8;

            try
            {
                var result = controller.Delete(MISSING_ID);
            }
            catch (HttpResponseException ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }
        }

        [TestMethod]
        public void People_Delete_ID_PersonDoesExist()
        {
            var context = new TestPeopleAppContext();
            context.People = PeopleHelper.GetPeopleListDTO();
            var controller = new PeopleController(context);

            var FOUND_PERSON = context.People[0];
            var result = controller.Delete(FOUND_PERSON.PersonID);

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, result.StatusCode);
        }

        #endregion
    }
}

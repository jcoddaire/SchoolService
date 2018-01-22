using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using StudentService.WebAPI.Controllers;
using StudentService.Tests.Unit.Helpers;
using System.Web.Http;

namespace StudentService.Tests.Unit.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PeopleControllerTest
    {
        [TestMethod]
        public void GetAllPeople_Valid()
        {
            var context = new TestPeopleAppContext();
            context.People = Helpers.PeopleHelper.GetPeopleListDTO();

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

        [TestMethod]
        public void GetPeople_LessThan0()
        {
            var context = new TestPeopleAppContext();            
            var controller = new PeopleController(context);
            var id = 0;

            try
            {
                var result = controller.Get(id);
            }
            catch(HttpResponseException ex)
            {
                Assert.IsNotNull(ex);

                var statusCode = ex.Response.StatusCode;

                //Assert.AreEqual(404, (int)ex.Response.StatusCode);
            }
        }
    }
}

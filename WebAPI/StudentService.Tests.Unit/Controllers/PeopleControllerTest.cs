using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using StudentService.DTOs;
using StudentService.Controllers;
using StudentService.Data;
using Moq;
using StudentService.Tests.Unit.Helpers;

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
    }
}

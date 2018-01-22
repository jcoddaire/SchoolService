using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.DTOs;
using System.Diagnostics.CodeAnalysis;
using StudentService.WebAPI.Controllers;

namespace StudentService.Tests.Integration.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PersonTest
    {   
        [TestMethod]
        public void AddPerson_Test()
        {
            var controller = new PeopleController();

            var currentPeopleCount = controller.Get().Count();

            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            obj = controller.Post(obj);

            Assert.IsTrue(controller.Get().Count() > currentPeopleCount);

            PersonTest.DeleteTestObject(obj.PersonID, controller);
        }

        [TestMethod]
        public void ReadPerson_Test()
        {
            var controller = new PeopleController();

            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            Assert.AreEqual(0, obj.PersonID);

            obj = controller.Post(obj);

            Assert.IsTrue(obj.PersonID > 0);
            Assert.AreEqual("Test", obj.FirstName);
            Assert.AreEqual("Subject", obj.LastName);
            Assert.IsNull(obj.HireDate);
            Assert.IsNull(obj.EnrollmentDate);

            PersonTest.DeleteTestObject(obj.PersonID, controller);
        }

        [TestMethod]
        public void DeletePerson_Test()
        {
            var controller = new PeopleController();

            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            obj = controller.Post(obj);
          
            var currentCount = controller.Get().Count();

            PersonTest.DeleteTestObject(obj.PersonID, controller);

            Assert.IsTrue(controller.Get().Count() < currentCount);
        }

        [TestMethod]
        public void UpdatePerson_Test_LastName()
        {
            var controller = new PeopleController();

            var obj = CreateTestPerson(controller);
            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomName = Guid.NewGuid().ToString();

                obj.LastName = randomName;

                obj = controller.Put(obj);

                //confirm the object was updated.
                var updatedPerson = controller.Get(obj.PersonID);

                Assert.IsNotNull(updatedPerson);
                Assert.AreEqual(randomName, updatedPerson.LastName);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Remove the test data.
                PersonTest.DeleteTestObject(obj.PersonID, controller);
            }            
        }

        [TestMethod]
        public void UpdatePerson_Test_FirstName()
        {
            var controller = new PeopleController();

            var obj = CreateTestPerson(controller);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);
            try
            {

                var randomName = Guid.NewGuid().ToString();

                obj.FirstName = randomName;


                obj = controller.Put(obj);

                //confirm the object was updated.
                var updatedPerson = controller.Get(obj.PersonID);

                Assert.IsNotNull(updatedPerson);
                Assert.AreEqual(randomName, updatedPerson.FirstName);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //Remove the test data.
                PersonTest.DeleteTestObject(obj.PersonID, controller);
            }
        }

        [TestMethod]
        public void UpdatePerson_Test_HireDate()
        {
            var controller = new PeopleController();
            var obj = CreateTestPerson(controller);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomDate = DateTime.Today;

                obj.HireDate = randomDate;

                obj = controller.Put(obj);

                //confirm the object was updated.
                var updatedPerson = controller.Get(obj.PersonID);

                Assert.IsNotNull(updatedPerson);
                Assert.AreEqual(randomDate, updatedPerson.HireDate);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //Remove the test data.
                PersonTest.DeleteTestObject(obj.PersonID, controller);
            }
        }

        [TestMethod]
        public void UpdatePerson_Test_EnrollmentDate()
        {
            var controller = new PeopleController();
            var obj = CreateTestPerson(controller);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomDate = DateTime.Today;

                obj.EnrollmentDate = randomDate;

                obj = controller.Put(obj);

                //confirm the object was updated.
                var updatedPerson = controller.Get(obj.PersonID);

                Assert.IsNotNull(updatedPerson);
                Assert.AreEqual(randomDate, updatedPerson.EnrollmentDate);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Remove the test data.
                PersonTest.DeleteTestObject(obj.PersonID, controller);
            }
        }

        /// <summary>
        /// Creates the test person.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static PersonDTO CreateTestPerson(PeopleController controller)
        {
            var firstName = "Test";
            var lastName = "Subject";
            DateTime? hireDate = null;
            DateTime? enrollmentDate = null;

            var person = new PersonDTO();
            person.FirstName = firstName;
            person.LastName = lastName;
            person.HireDate = hireDate;
            person.EnrollmentDate = enrollmentDate;

            person = controller.Post(person);

            return person;
        }

        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <param name="controller">The controller.</param>
        public static void DeleteTestObject(int personID, PeopleController controller)
        {
            controller.Delete(personID);
        }
    }
}

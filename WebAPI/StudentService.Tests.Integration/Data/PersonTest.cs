using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.DTOs;
using StudentService.Data;
using System.Diagnostics.CodeAnalysis;

namespace StudentService.Tests.Integration.Data
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PersonTest : TestBase
    {   
        [TestMethod]
        public void AddPerson_Test()
        {
            var currentPeopleCount = Repository.GetAllPersons().Count();

            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";
            
            obj = Repository.CreatePerson(obj);

            Assert.IsTrue(Repository.GetAllPersons().Count() > currentPeopleCount);

            PersonTest.DeleteTestObject(obj, Repository);
        }

        [TestMethod]
        public void ReadPerson_Test()
        {            
            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            Assert.AreEqual(0, obj.PersonID);

            obj = Repository.CreatePerson(obj);

            Assert.IsTrue(obj.PersonID > 0);
            Assert.AreEqual("Test", obj.FirstName);
            Assert.AreEqual("Subject", obj.LastName);
            Assert.IsNull(obj.HireDate);
            Assert.IsNull(obj.EnrollmentDate);

            PersonTest.DeleteTestObject(obj, Repository);
        }

        [TestMethod]
        public void DeletePerson_Test()
        {
            var obj = new PersonDTO();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            obj = Repository.CreatePerson(obj);
            
            var currentCount = Repository.GetAllPersons().Count();

            PersonTest.DeleteTestObject(obj, Repository);

            Assert.IsTrue(Repository.GetAllPersons().Count() < currentCount);
        }

        [TestMethod]
        public void UpdatePerson_Test_LastName()
        {
            var obj = CreateTestPerson(Repository);
            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomName = Guid.NewGuid().ToString();

                obj.LastName = randomName;

                obj = Repository.UpdatePerson(obj);

                //confirm the object was updated.
                var updatedPerson = Repository.GetPerson(obj.PersonID);

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
                PersonTest.DeleteTestObject(obj, Repository);
            }            
        }

        [TestMethod]
        public void UpdatePerson_Test_FirstName()
        {
            var obj = CreateTestPerson(Repository);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);
            try
            {

                var randomName = Guid.NewGuid().ToString();

                obj.FirstName = randomName;


                obj = Repository.UpdatePerson(obj);

                //confirm the object was updated.
                var updatedPerson = Repository.GetPerson(obj.PersonID);

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
                PersonTest.DeleteTestObject(obj, Repository);
            }
        }

        [TestMethod]
        public void UpdatePerson_Test_HireDate()
        {
            var obj = CreateTestPerson(Repository);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomDate = DateTime.Today;

                obj.HireDate = randomDate;

                obj = Repository.UpdatePerson(obj);

                //confirm the object was updated.
                var updatedPerson = Repository.GetPerson(obj.PersonID);

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
                PersonTest.DeleteTestObject(obj, Repository);
            }
        }

        [TestMethod]
        public void UpdatePerson_Test_EnrollmentDate()
        {
            var obj = CreateTestPerson(Repository);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            try
            {
                var randomDate = DateTime.Today;

                obj.EnrollmentDate = randomDate;

                obj = Repository.UpdatePerson(obj);

                //confirm the object was updated.
                var updatedPerson = Repository.GetPerson(obj.PersonID);

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
                PersonTest.DeleteTestObject(obj, Repository);
            }
        }

        /// <summary>
        /// Creates the test person.
        /// </summary>
        /// <param name="Repository">The repository.</param>
        /// <returns></returns>
        public static PersonDTO CreateTestPerson(IStudentService Repository)
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

            person = Repository.CreatePerson(person);

            return person;
        }
        
        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="toDelete">The object to delete.</param>
        public static void DeleteTestObject(PersonDTO toDelete, IStudentService Repository)
        {
            Repository.DeletePerson(toDelete.PersonID);            
        }
    }
}

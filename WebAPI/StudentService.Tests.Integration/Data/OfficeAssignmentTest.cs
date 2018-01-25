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
    public class OfficeAssignmentTest : TestBase
    {
        [TestMethod]
        public void AddOfficeAssignment_Test()
        {
            var obj = CreateTestOfficeAssignment(Repository);

            try
            {
                Assert.IsTrue(Repository.GetAllOfficeAssignments().Any(x => x.InstructorID == obj.InstructorID));
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                OfficeAssignmentTest.DeleteTestObject(obj, Repository);
            }
        }

        [TestMethod]
        public void ReadOfficeAssignment_Test()
        {
            var obj = CreateTestOfficeAssignment(Repository);

            Assert.IsNotNull(obj.InstructorID);
            Assert.IsNotNull(obj.Location);

            Assert.IsTrue(obj.InstructorID > 0);
            Assert.IsFalse(string.IsNullOrEmpty(obj.Location));

            OfficeAssignmentTest.DeleteTestObject(obj, Repository);
        }
        
        [TestMethod]
        public void DeleteOfficeAssignment_Test()
        {
            var obj = CreateTestOfficeAssignment(Repository);

            var currentCount = Repository.GetAllOfficeAssignments().Count();

            OfficeAssignmentTest.DeleteTestObject(obj, Repository);

            Assert.IsTrue(Repository.GetAllOfficeAssignments().Count() < currentCount);
        }

        [TestMethod]
        public void UpdateOfficeAssignment_Test_Location()
        {
            var obj = CreateTestOfficeAssignment(Repository);

            try
            {
                var randomLocation = Guid.NewGuid().ToString();

                obj.Location = randomLocation;

                obj = Repository.UpdateOfficeAssignment(obj);

                //confirm the object was updated.
                var updated = Repository.GetOfficeAssignment(obj.InstructorID);

                Assert.IsNotNull(updated);
                Assert.AreEqual(randomLocation, updated.Location);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Remove the test data.            
                OfficeAssignmentTest.DeleteTestObject(obj, Repository);
            }
        }
        
        /// <summary>
        /// Creates the test office assignment.
        /// This will automatically create an instructor and assign it to the result object.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        /// <returns></returns>
        public static OfficeAssignmentDTO CreateTestOfficeAssignment(IStudentService _repository)
        {
            var obj = new OfficeAssignmentDTO();
            obj.Person = PersonTest.CreateTestPerson(_repository);
            obj.InstructorID = obj.Person.PersonID;
            obj.Location = Guid.NewGuid().ToString();
            obj.Timestamp = null;

            obj = _repository.CreateOfficeAssignment(obj);

            return obj;
        }

        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="toDelete">The object to delete.</param>
        /// <param name="_repository">The repository.</param>
        public static void DeleteTestObject(OfficeAssignmentDTO toDelete, IStudentService _repository)
        {
            _repository.DeleteOfficeAssignment(toDelete.InstructorID);
            _repository.DeletePerson(toDelete.InstructorID);
        }
    }
}

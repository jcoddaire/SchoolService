using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.Data;

namespace StudentService.Tests.Integration.Data
{
    [TestClass]
    public class PersonTest
    {
        StudentDB db = new StudentDB();

        [TestMethod]
        public void AddPerson_Test()
        {
            var currentPeopleCount = db.People.Count();

            var obj = new Person();
            obj.FirstName = "Test";
            obj.LastName = "Subject";


            db.People.Add(obj);
            db.SaveChanges();

            Assert.IsTrue(db.People.Count() > currentPeopleCount);

            PersonTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void ReadPerson_Test()
        {            
            var obj = new Person();
            obj.FirstName = "Test";
            obj.LastName = "Subject";

            Assert.AreEqual(0, obj.PersonID);

            db.People.Add(obj);
            db.SaveChanges();

            Assert.IsTrue(obj.PersonID > 0);
            Assert.AreEqual("Test", obj.FirstName);
            Assert.AreEqual("Subject", obj.LastName);
            Assert.IsNull(obj.HireDate);
            Assert.IsNull(obj.EnrollmentDate);
            
            PersonTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void DeletePerson_Test()
        {
            var obj = new Person();
            obj.FirstName = "Test";
            obj.LastName = "Subject";
            
            db.People.Add(obj);
            db.SaveChanges();

            var currentCount = db.People.Count();

            PersonTest.DeleteTestObject(obj, db);

            Assert.IsTrue(db.People.Count() < currentCount);

        }

        [TestMethod]
        public void UpdatePerson_Test_LastName()
        {
            var obj = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            var randomName = Guid.NewGuid().ToString();

            obj.LastName = randomName;



            db.People.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.LastName).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == obj.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomName, updatedPerson.LastName);

            //Remove the test data.
            PersonTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void UpdatePerson_Test_FirstName()
        {
            var obj = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            var randomName = Guid.NewGuid().ToString();

            obj.FirstName = randomName;



            db.People.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.FirstName).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == obj.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomName, updatedPerson.FirstName);

            //Remove the test data.
            PersonTest.DeleteTestObject(obj, db);
        }
        
        [TestMethod]
        public void UpdatePerson_Test_HireDate()
        {
            var obj = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            var randomDate = DateTime.Today;

            obj.HireDate = randomDate;
            
            db.People.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.HireDate).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == obj.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomDate, updatedPerson.HireDate);

            //Remove the test data.
            PersonTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void UpdatePerson_Test_EnrollmentDate()
        {
            var obj = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.PersonID > 0);

            var randomDate = DateTime.Today;

            obj.EnrollmentDate = randomDate;
            
            db.People.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.EnrollmentDate).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == obj.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomDate, updatedPerson.EnrollmentDate);

            //Remove the test data.
            PersonTest.DeleteTestObject(obj, db);
        }

        /// <summary>
        /// Creates the test person.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        public static Person CreateTestPerson(StudentDB db)
        {
            var firstName = "Test";
            var lastName = "Subject";
            DateTime? hireDate = null;
            DateTime? enrollmentDate = null;

            var person = new Person();
            person.FirstName = firstName;
            person.LastName = lastName;
            person.HireDate = hireDate;
            person.EnrollmentDate = enrollmentDate;
            
            db.People.Add(person);
            db.SaveChanges();

            return person;
        }


        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="toDelete">The object to delete.</param>
        public static void DeleteTestObject(Person toDelete, StudentDB db)
        {
            db.People.Remove(toDelete);
            db.SaveChanges();
        }
    }
}

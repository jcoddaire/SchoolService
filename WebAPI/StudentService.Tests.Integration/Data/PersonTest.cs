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

            var person = new Person();
            person.FirstName = "Test";
            person.LastName = "Subject";


            db.People.Add(person);
            db.SaveChanges();

            Assert.IsTrue(db.People.Count() > currentPeopleCount);

            db.People.Remove(person);
            db.SaveChanges();
        }

        [TestMethod]
        public void ReadPerson_Test()
        {            
            var person = new Person();
            person.FirstName = "Test";
            person.LastName = "Subject";

            Assert.AreEqual(0, person.PersonID);

            db.People.Add(person);
            db.SaveChanges();

            Assert.IsTrue(person.PersonID > 0);
            Assert.AreEqual("Test", person.FirstName);
            Assert.AreEqual("Subject", person.LastName);
            Assert.IsNull(person.HireDate);
            Assert.IsNull(person.EnrollmentDate);

            db.People.Remove(person);
            db.SaveChanges();
        }

        [TestMethod]
        public void DeletePerson_Test()
        {
            var person = new Person();
            person.FirstName = "Test";
            person.LastName = "Subject";


            db.People.Add(person);
            db.SaveChanges();

            var currentPeopleCount = db.People.Count();


            db.People.Remove(person);
            db.SaveChanges();

            Assert.IsTrue(db.People.Count() < currentPeopleCount);

        }

        [TestMethod]
        public void UpdatePerson_Test_LastName()
        {
            var person = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(person.PersonID > 0);

            var randomName = Guid.NewGuid().ToString();

            person.LastName = randomName;



            db.People.Attach(person);
            var entry = db.Entry(person);
            entry.Property(e => e.LastName).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == person.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomName, updatedPerson.LastName);

            //Remove the test data.
            db.People.Remove(updatedPerson);
            db.SaveChanges();
        }

        [TestMethod]
        public void UpdatePerson_Test_FirstName()
        {
            var person = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(person.PersonID > 0);

            var randomName = Guid.NewGuid().ToString();

            person.FirstName = randomName;



            db.People.Attach(person);
            var entry = db.Entry(person);
            entry.Property(e => e.FirstName).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == person.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomName, updatedPerson.FirstName);

            //Remove the test data.
            db.People.Remove(updatedPerson);
            db.SaveChanges();
        }
        
        [TestMethod]
        public void UpdatePerson_Test_HireDate()
        {
            var person = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(person.PersonID > 0);

            var randomDate = DateTime.Today;

            person.HireDate = randomDate;
            
            db.People.Attach(person);
            var entry = db.Entry(person);
            entry.Property(e => e.HireDate).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == person.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomDate, updatedPerson.HireDate);

            //Remove the test data.
            db.People.Remove(updatedPerson);
            db.SaveChanges();
        }
        
        [TestMethod]
        public void UpdatePerson_Test_EnrollmentDate()
        {
            var person = CreateTestPerson(db);

            //confirm they are saved in the database.
            Assert.IsTrue(person.PersonID > 0);

            var randomDate = DateTime.Today;

            person.EnrollmentDate = randomDate;
            
            db.People.Attach(person);
            var entry = db.Entry(person);
            entry.Property(e => e.EnrollmentDate).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updatedPerson = db.People.Where(x => x.PersonID == person.PersonID).FirstOrDefault();

            Assert.IsNotNull(updatedPerson);
            Assert.AreEqual(randomDate, updatedPerson.EnrollmentDate);

            //Remove the test data.
            db.People.Remove(updatedPerson);
            db.SaveChanges();
        }
        
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
    }
}

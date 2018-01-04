using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.Data;

namespace StudentService.Tests.Integration.Data
{
    [TestClass]
    public class DepartmentTest : TestBase
    {
        StudentDB db = new StudentDB();        

        [TestMethod]
        public void AddDepartment_Test()
        {
            var obj = CreateTestDepartment(db);

            Assert.IsTrue(db.Departments.Any(x => x.DepartmentID == obj.DepartmentID));

            DepartmentTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void ReadDepartment_Test()
        {
            var obj = CreateTestDepartment(db);

            Assert.IsTrue(obj.DepartmentID > 0);
            Assert.IsNotNull(obj.Name);

            Assert.IsNotNull(obj.StartDate);
            Assert.AreEqual(DateTime.Today, obj.StartDate);

            Assert.IsNotNull(obj.Budget);
            Assert.AreEqual(1000000, obj.Budget);

            Assert.IsNull(obj.Administrator);
            
            DepartmentTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void DeleteDepartment_Test()
        {
            var obj = CreateTestDepartment(db);            
            
            var currentCount = db.Departments.Count();

            DepartmentTest.DeleteTestObject(obj, db);

            Assert.IsTrue(db.Departments.Count() < currentCount);            
        }

        [TestMethod]
        public void UpdateDepartment_Test_Name()
        {
            var obj = CreateTestDepartment(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.DepartmentID > 0);

            var randomName = Guid.NewGuid().ToString();

            obj.Name = randomName;
            
            db.Departments.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.Name).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Departments.Where(x => x.DepartmentID == obj.DepartmentID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(randomName, updated.Name);

            //Remove the test data.
            DepartmentTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void UpdateDepartment_Test_Budget()
        {
            var obj = CreateTestDepartment(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.DepartmentID > 0);

            var differentBudget = 1;

            obj.Budget = differentBudget;

            db.Departments.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.Budget).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Departments.Where(x => x.DepartmentID == obj.DepartmentID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(differentBudget, updated.Budget);

            //Remove the test data.
            DepartmentTest.DeleteTestObject(obj, db);
        }
        
        [TestMethod]
        public void UpdateDepartment_Test_StartDate()
        {
            var obj = CreateTestDepartment(db);

            //confirm they are saved in the database.
            Assert.IsTrue(obj.DepartmentID > 0);

            var differentDate = new DateTime(1999, 12, 31);

            obj.StartDate = differentDate;

            db.Departments.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.StartDate).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Departments.Where(x => x.DepartmentID == obj.DepartmentID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(differentDate, updated.StartDate);

            //Remove the test data.
            DepartmentTest.DeleteTestObject(obj, db);
        }
        
        [TestMethod]
        public void UpdateDepartment_Test_Administrator()
        {
            var obj = CreateTestDepartment(db);
                                    
            var person = PersonTest.CreateTestPerson(Repository);

            obj.Administrator = person.PersonID;
            
            db.Departments.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.Administrator).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Departments.Where(x => x.DepartmentID == obj.DepartmentID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(person.PersonID, updated.Administrator);

            //Remove the test data.
            DepartmentTest.DeleteTestObject(obj, db);
            PersonTest.DeleteTestObject(person, Repository);
        }

        /// <summary>
        /// Creates the test department.
        /// </summary>
        /// <param name="db">The database.</param>
        /// <returns></returns>
        public static Department CreateTestDepartment(StudentDB db)
        {            
            var departmentName = Guid.NewGuid().ToString();
            decimal budget = 1000000;
            var startDate = DateTime.Today;            
            
            var obj = new Department();
            obj.Name = departmentName;
            obj.Budget = budget;
            obj.StartDate = startDate;            
            
            obj.DepartmentID = db.Departments.Max(x => x.DepartmentID) + 1;

            db.Departments.Add(obj);
            db.SaveChanges();

            return obj;
        }
        
        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="toDelete">The object to delete.</param>
        public static void DeleteTestObject(Department toDelete, StudentDB db)
        {
            db.Departments.Remove(toDelete);            
            db.SaveChanges();
        }
    }
}

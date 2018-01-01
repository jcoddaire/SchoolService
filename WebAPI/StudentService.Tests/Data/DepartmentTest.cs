using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.Data;

namespace StudentService.Tests.Data
{
    [TestClass]
    public class DepartmentTest
    {
        StudentDB db = new StudentDB();

        [TestMethod]
        public void AddDepartment_Test()
        {
            var department = CreateTestDepartment(db);
            Assert.IsTrue(department.DepartmentID > 0);

            db.Departments.Remove(department);
            db.SaveChanges();
        }

        [TestMethod]
        public void ReadDepartment_Test()
        {
            var department = CreateTestDepartment(db);
            Assert.IsTrue(department.DepartmentID > 0);


            Assert.IsTrue(department.DepartmentID > 0);
            Assert.IsNotNull(department.Name);

            Assert.IsNotNull(department.StartDate);
            Assert.AreEqual(DateTime.Today, department.StartDate);

            Assert.IsNotNull(department.Budget);
            Assert.AreEqual(1000000, department.Budget);

            Assert.IsNull(department.Administrator);


            db.Departments.Remove(department);
            db.SaveChanges();
        }

        [TestMethod]
        public void DeleteDepartment_Test()
        {

            var department = CreateTestDepartment(db);
            Assert.IsTrue(department.DepartmentID > 0);


            var currentCount = db.People.Count();

            db.Departments.Remove(department);
            db.SaveChanges();
            
            Assert.IsTrue(db.Departments.Count() < currentCount);

            var addedDepartment = db.Departments.Where(x => x.DepartmentID == department.DepartmentID).FirstOrDefault();
            Assert.IsNull(addedDepartment);
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
            db.Departments.Remove(updated);
            db.SaveChanges();
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
            db.Departments.Remove(updated);
            db.SaveChanges();
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
            db.Departments.Remove(updated);
            db.SaveChanges();
        }
        
        [TestMethod]
        public void UpdateDepartment_Test_Administrator()
        {
            var obj = CreateTestDepartment(db);
                                    
            var person = PersonTest.CreateTestPerson(db);

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
            db.Departments.Remove(updated);
            db.People.Remove(person);
            db.SaveChanges();
        }

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
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StudentService.Data;

namespace StudentService.Tests.Integration.Data
{
    [TestClass]
    public class CourseTest
    {
        StudentDB db = new StudentDB();

        [TestMethod]
        public void AddCourse_Test()
        {
            var obj = CreateTestCourse(db);

            Assert.IsTrue(db.Courses.Any(x => x.CourseID == obj.CourseID));

            CourseTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void ReadCourse_Test()
        {
            var obj = CreateTestCourse(db);

            Assert.IsNotNull(obj.CourseID);
            Assert.IsNotNull(obj.Title);
            Assert.IsNotNull(obj.Credits);
            Assert.IsNotNull(obj.DepartmentID);

            Assert.IsTrue(obj.CourseID > 0);
            Assert.IsTrue(obj.DepartmentID > 0);
            Assert.IsTrue(obj.Title.Length > 5);

            CourseTest.DeleteTestObject(obj, db);
        }
        
        [TestMethod]
        public void DeleteCourse_Test()
        {
            var obj = CreateTestCourse(db);
            
            var currentCount = db.Courses.Count();

            CourseTest.DeleteTestObject(obj, db);

            Assert.IsTrue(db.Courses.Count() < currentCount);
        }

        [TestMethod]
        public void UpdateCourse_Test_Title()
        {
            var obj = CreateTestCourse(db);
            
            var randomTitle = Guid.NewGuid().ToString();

            obj.Title = randomTitle;
            
            db.Courses.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.Title).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Courses.Where(x => x.CourseID == obj.CourseID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(randomTitle, updated.Title);

            //Remove the test data.            
            CourseTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void UpdateCourse_Test_Credits()
        {
            var obj = CreateTestCourse(db);
            
            var differentCredit = obj.Credits + 1;

            obj.Credits = differentCredit;

            db.Courses.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.Credits).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Courses.Where(x => x.CourseID == obj.CourseID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(differentCredit, updated.Credits);

            //Remove the test data.
            CourseTest.DeleteTestObject(obj, db);
        }

        [TestMethod]
        public void UpdateCourse_Test_Department()
        {
            var obj = CreateTestCourse(db);

            var originalDepartmentID = obj.DepartmentID;

            var newDepartment = DepartmentTest.CreateTestDepartment(db);

            obj.DepartmentID = newDepartment.DepartmentID;

            db.Courses.Attach(obj);
            var entry = db.Entry(obj);
            entry.Property(e => e.DepartmentID).IsModified = true;
            db.SaveChanges();

            //confirm the object was updated.
            var updated = db.Courses.Where(x => x.CourseID == obj.CourseID).FirstOrDefault();

            Assert.IsNotNull(updated);
            Assert.AreEqual(newDepartment.DepartmentID, updated.DepartmentID);

            //Remove the test data.
            CourseTest.DeleteTestObject(obj, db);

            var originalDepartment = db.Departments.Where(x => x.DepartmentID == originalDepartmentID).FirstOrDefault();
            DepartmentTest.DeleteTestObject(originalDepartment, db);
        }        

        /// <summary>
        /// Creates the test course.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <returns></returns>
        public static Course CreateTestCourse(StudentDB db)
        {            
            var title = Guid.NewGuid().ToString();
            var random = new Random();
            var credits = random.Next(1, 4);
            var departmentID = DepartmentTest.CreateTestDepartment(db).DepartmentID;                 
            
            var obj = new Course();
            obj.CourseID = db.Courses.Max(x => x.CourseID) + 1;
            obj.Title = title;
            obj.Credits = credits;
            obj.DepartmentID = departmentID;            

            db.Courses.Add(obj);
            db.SaveChanges();

            return obj;
        }
        
        /// <summary>
        /// Deletes the test object.
        /// </summary>
        /// <param name="toDelete">The object to delete.</param>
        public static void DeleteTestObject(Course toDelete, StudentDB db)
        {
            db.Courses.Remove(toDelete);
            db.SaveChanges();

            var originalDepartment = db.Departments.Where(x => x.DepartmentID == toDelete.DepartmentID).FirstOrDefault();
            if(originalDepartment != null)
            {
                DepartmentTest.DeleteTestObject(originalDepartment, db);
            }
        }
    }
}

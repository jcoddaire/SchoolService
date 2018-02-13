using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentService.DTOs;

namespace StudentService.Data
{
    public class StudentServiceRepository : IStudentService
    {
        private StudentDB _Database = null;

        private StudentDB Database
        {
            get
            {
                if (_Database == null)
                {
                    _Database = new StudentDB();
                }

                return _Database;
            }
        }
        public StudentServiceRepository(StudentDB db)
        {
            _Database = db;
        }

        #region People Methods

        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PersonDTO> GetAllPersons()
        {
            var sourcePeople = Database.People.Select(
                x => new PersonDTO()
                {
                    PersonID = x.PersonID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate,
                    EnrollmentDate = x.EnrollmentDate

                }).ToList();

            return sourcePeople;
        }

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// A person DTO object. If no person was found, returns null.
        /// </returns>
        public PersonDTO GetPerson(int personID)
        {
            if(personID <= 0)
            {
                return null;
            }

            var personOfInterest = Database.People.Where(p => p.PersonID == personID).Select(
                x => new PersonDTO()
                {
                    PersonID = x.PersonID,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate,
                    EnrollmentDate = x.EnrollmentDate

                }).FirstOrDefault();

            if(personOfInterest != null && personOfInterest.PersonID > 0)
            {
                return personOfInterest;
            }

            return null;
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public PersonDTO CreatePerson(PersonDTO person)
        {
            var newPeep = new Person
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                EnrollmentDate = person.EnrollmentDate,
                HireDate = person.HireDate
            };

            Database.People.Add(newPeep);
            Database.SaveChanges();

            //leap of faith, apparently the newPeep object has the ID.
            person.PersonID = newPeep.PersonID;

            return person;
        }

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public PersonDTO UpdatePerson(PersonDTO person)
        {
            var changedPeep = Database.People.Where(p => p.PersonID == person.PersonID).FirstOrDefault();
            if(changedPeep == null || changedPeep.PersonID != person.PersonID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            changedPeep.FirstName = person.FirstName;
            changedPeep.LastName = person.LastName;
            changedPeep.EnrollmentDate = person.EnrollmentDate;
            changedPeep.HireDate = person.HireDate;
            
            Database.People.Attach(changedPeep);

            var entry = Database.Entry(changedPeep);
            entry.Property(e => e.FirstName).IsModified = true;
            entry.Property(e => e.LastName).IsModified = true;
            entry.Property(e => e.EnrollmentDate).IsModified = true;
            entry.Property(e => e.HireDate).IsModified = true;

            Database.SaveChanges();

            return person;
        }

        /// <summary>
        /// Deletes the person. If the person has Student Grades, this also removes the grades.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// The number of rows affected.
        /// If something failed, returns -1.
        /// </returns>
        public int DeletePerson(int personID)
        {
            var target = Database.People.Where(x => x.PersonID == personID).FirstOrDefault();
            
            if(target != null && target.PersonID == personID)
            {
                Database.StudentGrades.RemoveRange(Database.StudentGrades.Where(x => x.StudentID == personID).ToList());
                Database.People.Remove(target);                
                return Database.SaveChanges();
            }
            return -1;
        }

        #endregion

        #region Department Methods

        public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            var source = Database.Departments.Select(
                x => new DepartmentDTO()
                {
                    DepartmentID = x.DepartmentID,
                    Name = x.Name,
                    Budget = x.Budget,
                    StartDate = x.StartDate,
                    Administrator = x.Administrator

                }).ToList();

            return source;
        }

        public DepartmentDTO GetDepartment(int departmentID)
        {
            if (departmentID <= 0)
            {
                return null;
            }

            var target = Database.Departments.Where(d => d.DepartmentID == departmentID).Select(
                x => new DepartmentDTO()
                {
                    DepartmentID = x.DepartmentID,
                    Name = x.Name,
                    Budget = x.Budget,
                    StartDate = x.StartDate,
                    Administrator = x.Administrator

                }).FirstOrDefault();

            if (target != null && target.DepartmentID > 0)
            {
                return target;
            }

            return null;
        }

        public DepartmentDTO CreateDepartment(DepartmentDTO department)
        {
            var topID = GetAllDepartments().Max(x => x.DepartmentID) + 1;

            var obj = new Department
            {
                DepartmentID = topID,
                Name = department.Name,
                Budget = department.Budget,
                StartDate = department.StartDate,
                Administrator = department.Administrator
            };

            Database.Departments.Add(obj);
            Database.SaveChanges();

            department.DepartmentID = topID;

            return department;
        }

        public DepartmentDTO UpdateDepartment(DepartmentDTO department)
        {
            var changed = Database.Departments.Where(d => d.DepartmentID == department.DepartmentID).FirstOrDefault();
            if (changed == null || changed.DepartmentID != department.DepartmentID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            changed.Name = department.Name;
            changed.Budget = department.Budget;
            changed.StartDate = department.StartDate;
            changed.Administrator = department.Administrator;

            Database.Departments.Attach(changed);

            var entry = Database.Entry(changed);
            entry.Property(e => e.Name).IsModified = true;
            entry.Property(e => e.Budget).IsModified = true;
            entry.Property(e => e.StartDate).IsModified = true;
            entry.Property(e => e.Administrator).IsModified = true;

            Database.SaveChanges();

            return department;
        }

        public int DeleteDepartment(int departmentID)
        {

            var target = Database.Departments.Where(x => x.DepartmentID == departmentID).FirstOrDefault();

            if (target != null && target.DepartmentID == departmentID)
            {
                Database.Departments.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }
        #endregion

        #region Course Methods
        
        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of courses.</returns>
        public IEnumerable<CourseDTO> GetAllCourses()
        {
            var courses = Database.Courses.Select(
                x => new CourseDTO()
                {
                    CourseID = x.CourseID,
                    Title = x.Title,
                    Credits = x.Credits,
                    DepartmentID = x.DepartmentID
                    
                }).ToList();

            return courses;
        }

        /// <summary>
        /// Gets the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>The Course DTO.</returns>
        public CourseDTO GetCourse(int courseID)
        {
            if (courseID <= 0)
            {
                return null;
            }

            var target = Database.Courses.Where(c => c.CourseID == courseID).Select(
                x => new CourseDTO()
                {
                    CourseID = x.CourseID,
                    Title = x.Title,
                    Credits = x.Credits,
                    DepartmentID = x.DepartmentID

                }).FirstOrDefault();

            if (target != null && target.CourseID > 0)
            {
                return target;
            }

            return null;
        }

        /// <summary>
        /// Creates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns>The course.</returns>
        public CourseDTO CreateCourse(CourseDTO course)
        {
            var newItem = new Course
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentID = course.DepartmentID
            };

            Database.Courses.Add(newItem);
            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Updates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Could not find a matching item in the dataset.</exception>
        public CourseDTO UpdateCourse(CourseDTO course)
        {
            var changedTarget = Database.Courses.Where(p => p.CourseID == course.CourseID).FirstOrDefault();
            if (changedTarget == null || changedTarget.CourseID != course.CourseID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            //changedTarget.CourseID = course.CourseID; //this should never happen. Otherwise that ^^^ check will fail.
            changedTarget.Title = course.Title;
            changedTarget.Credits = course.Credits;
            changedTarget.DepartmentID = course.DepartmentID;

            Database.Courses.Attach(changedTarget);

            var entry = Database.Entry(changedTarget);
            //entry.Property(e => e.CourseID).IsModified = true;
            entry.Property(e => e.Title).IsModified = true;
            entry.Property(e => e.Credits).IsModified = true;
            entry.Property(e => e.DepartmentID).IsModified = true;

            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Deletes the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns>The number of rows affected. Returns -1 if an error occured.</returns>
        public int DeleteCourse(int courseID)
        {
            var target = Database.Courses.Where(x => x.CourseID == courseID).FirstOrDefault();

            if (target != null && target.CourseID == courseID)
            {
                Database.Courses.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }
        #endregion

        #region Office Assignments
        
        /// <summary>
        /// Gets the office assignment.
        /// </summary>
        /// <param name="instructorID">The person identifier.</param>
        /// <returns></returns>
        public OfficeAssignmentDTO GetOfficeAssignment(int instructorID)
        {
            if (instructorID <= 0)
            {
                return null;
            }

            var target = Database.OfficeAssignments.Where(c => c.InstructorID == instructorID).Select(
                x => new OfficeAssignmentDTO()
                {
                    InstructorID = x.InstructorID,
                    Location = x.Location,
                    Timestamp = x.Timestamp

                }).FirstOrDefault();

            if (target != null && target.InstructorID > 0)
            {
                return target;
            }

            return null;
        }

        /// <summary>
        /// Gets all office assignments.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OfficeAssignmentDTO> GetAllOfficeAssignments()
        {
            var results = Database.OfficeAssignments.Select(
               x => new OfficeAssignmentDTO()
               {
                   InstructorID = x.InstructorID,
                   Location = x.Location,
                   Timestamp = x.Timestamp

               }).ToList();

            return results;
        }

        /// <summary>
        /// Creates the office assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">assignment - Assignment cannot be null</exception>
        /// <exception cref="ArgumentOutOfRangeException">assignment.InstructorID - Must be greater than 0!</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public OfficeAssignmentDTO CreateOfficeAssignment(OfficeAssignmentDTO assignment)
        {
            if(assignment == null)
            {
                throw new ArgumentNullException("assignment", "Assignment cannot be null");
            }
            if(assignment.InstructorID <= 0)
            {
                throw new ArgumentOutOfRangeException("assignment.InstructorID", "Must be greater than 0!");
            }

            if(!Database.People.Any(x => x.PersonID == assignment.InstructorID))
            {
                throw new KeyNotFoundException($"The InstructorID '{assignment.InstructorID}' was not found in the system.");
            }

            DeleteOfficeAssignment(assignment.InstructorID);

            var newOfficeAssignment = new OfficeAssignment
            {
                InstructorID = assignment.InstructorID,
                Location = assignment.Location,
                Timestamp = assignment.Timestamp
            };

            Database.OfficeAssignments.Add(newOfficeAssignment);
            Database.SaveChanges();

            return assignment;
        }

        /// <summary>
        /// Updates the office assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public OfficeAssignmentDTO UpdateOfficeAssignment(OfficeAssignmentDTO assignment)
        {
            var changedTarget = Database.OfficeAssignments.Where(p => p.InstructorID == assignment.InstructorID).FirstOrDefault();
            if (changedTarget == null || changedTarget.InstructorID != assignment.InstructorID)
            {
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }
                        
            if(DeleteOfficeAssignment(assignment.InstructorID) > 0)
            {
                assignment = CreateOfficeAssignment(assignment);
            }            

            return assignment;
        }

        /// <summary>
        /// Deletes the office assignment.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns></returns>
        public int DeleteOfficeAssignment(int personID)
        {
            var target = Database.OfficeAssignments.Where(x => x.InstructorID == personID).FirstOrDefault();

            if (target != null && target.InstructorID == personID)
            {
                Database.OfficeAssignments.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }
        #endregion

        #region Student Grade
        /// <summary>
        /// Gets the student grade.
        /// </summary>
        /// <param name="enrollmentID">The enrollment identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">enrollmentID - Must be greater than 0!</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public StudentGradeDTO GetStudentGrade(int enrollmentID)
        {
            if (enrollmentID <= 0)
            {
                return null;
            }
            if (!Database.StudentGrades.Any(x => x.EnrollmentID == enrollmentID))
            {
                return null;
            }

            var result = Database.StudentGrades.Where(x => x.EnrollmentID == enrollmentID).FirstOrDefault();
            if(result != null)
            {
                var newResult = new StudentGradeDTO
                {
                    EnrollmentID = result.EnrollmentID,
                    CourseID = result.CourseID,
                    StudentID = result.StudentID,
                    Grade = result.Grade
                };

                return newResult;
            }
            return null; //this should never happen.
        }

        /// <summary>
        /// Gets all student grades.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudentGradeDTO> GetAllStudentGrades()
        {
            var results = Database.StudentGrades.Select(
                x => new StudentGradeDTO()
                {
                    EnrollmentID = x.EnrollmentID,
                    CourseID = x.CourseID,
                    StudentID = x.StudentID,
                    Grade = x.Grade

                }).ToList();

            return results;
        }

        /// <summary>
        /// Adds the student grade.
        /// </summary>
        /// <param name="grade">The grade.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">grade - grade cannot be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// grade.CourseID - Must be greater than 0!
        /// or
        /// grade.StudentID - Must be greater than 0!
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public StudentGradeDTO AddStudentGrade(StudentGradeDTO grade)
        {
            if(grade == null)
            {
                throw new ArgumentNullException("grade", "grade cannot be null.");
            }

            if (grade.CourseID <= 0)
            {
                throw new ArgumentOutOfRangeException("grade.CourseID", "Must be greater than 0!");
            }
            if (!Database.Courses.Any(x => x.CourseID == grade.CourseID))
            {
                throw new KeyNotFoundException($"The courseID '{grade.CourseID}' was not found in the system.");
            }

            if (grade.StudentID <= 0)
            {
                throw new ArgumentOutOfRangeException("grade.StudentID", "Must be greater than 0!");
            }
            if (!Database.People.Any(x => x.PersonID == grade.StudentID))
            {
                throw new KeyNotFoundException($"The studentID '{grade.StudentID}' was not found in the system.");
            }
            
            var newTarget = new StudentGrade
            {
                CourseID = grade.CourseID,
                StudentID = grade.StudentID,
                Grade = grade.Grade
            };

            Database.StudentGrades.Add(newTarget);
            Database.SaveChanges();
                        
            grade.EnrollmentID = newTarget.EnrollmentID;

            return grade;
        }

        /// <summary>
        /// Updates the student grade.
        /// </summary>
        /// <param name="grade">The grade.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">grade - grade cannot be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// grade.EnrollmentID - Must be greater than 0!
        /// or
        /// grade.CourseID - Must be greater than 0!
        /// or
        /// grade.StudentID - Must be greater than 0!
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Could not find a matching item in the dataset.
        /// </exception>
        public StudentGradeDTO UpdateStudentGrade(StudentGradeDTO grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException("grade", "grade cannot be null.");
            }

            if (grade.EnrollmentID <= 0)
            {
                throw new ArgumentOutOfRangeException("grade.EnrollmentID", "Must be greater than 0!");
            }
            if (!Database.StudentGrades.Any(x => x.EnrollmentID == grade.EnrollmentID))
            {
                throw new KeyNotFoundException($"The enrollmentID '{grade.EnrollmentID}' was not found in the system.");
            }

            if (grade.CourseID <= 0)
            {
                throw new ArgumentOutOfRangeException("grade.CourseID", "Must be greater than 0!");
            }
            if (!Database.Courses.Any(x => x.CourseID == grade.CourseID))
            {
                throw new KeyNotFoundException($"The courseID '{grade.CourseID}' was not found in the system.");
            }

            if (grade.StudentID <= 0)
            {
                throw new ArgumentOutOfRangeException("grade.StudentID", "Must be greater than 0!");
            }
            if (!Database.People.Any(x => x.PersonID == grade.StudentID))
            {
                throw new KeyNotFoundException($"The studentID '{grade.StudentID}' was not found in the system.");
            }

            var changed = Database.StudentGrades.Where(p => p.EnrollmentID == grade.EnrollmentID).FirstOrDefault();
            if (changed == null || changed.EnrollmentID != grade.EnrollmentID)
            {
                //this should never happen.
                throw new KeyNotFoundException("Could not find a matching item in the dataset.");
            }

            changed.StudentID = grade.StudentID;
            changed.CourseID = grade.CourseID;
            changed.Grade = grade.Grade;

            Database.StudentGrades.Attach(changed);

            var entry = Database.Entry(changed);
            entry.Property(e => e.StudentID).IsModified = true;
            entry.Property(e => e.CourseID).IsModified = true;
            entry.Property(e => e.Grade).IsModified = true;            

            Database.SaveChanges();

            return grade;
        }

        /// <summary>
        /// Deletes the student grade.
        /// </summary>
        /// <param name="enrollmentID">The enrollment identifier.</param>
        /// <returns></returns>
        public int DeleteStudentGrade(int enrollmentID)
        {
            var target = Database.StudentGrades.Where(x => x.EnrollmentID == enrollmentID).FirstOrDefault();

            if (target != null && target.EnrollmentID == enrollmentID)
            {
                Database.StudentGrades.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }

        #endregion

        #region Online Courses

        /// <summary>
        /// Gets all online courses.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OnlineCourseDTO> GetAllOnlineCourses()
        {
            var results = Database.OnlineCourses.Select(
               x => new OnlineCourseDTO()
               {                   
                   CourseID = x.CourseID,
                   URL = x.URL

               }).ToList();

            return results;
        }

        /// <summary>
        /// Gets the online course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        public OnlineCourseDTO GetOnlineCourse(int courseID)
        {
            if (courseID <= 0)
            {
                return null;
            }
            if (!Database.OnlineCourses.Any(x => x.CourseID == courseID))
            {
                return null;
            }

            var result = Database.OnlineCourses.Where(x => x.CourseID == courseID).FirstOrDefault();
            if (result != null)
            {
                var newResult = new OnlineCourseDTO
                {
                    CourseID = result.CourseID,
                    URL = result.URL
                };

                return newResult;
            }
            return null; //this should never happen.
        }

        /// <summary>
        /// Adds the online course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        public OnlineCourseDTO AddOnlineCourse(OnlineCourseDTO course)
        {
            if(course == null)
            {
                throw new ArgumentNullException("course");
            }
            if (Database.Courses.Any(x => x.CourseID == course.CourseID) == false)
            {
                throw new KeyNotFoundException($"The course ID {course.CourseID} was not found in the system.");
            }
            if (Database.OnlineCourses.Any(x => x.CourseID == course.CourseID))
            {
                DeleteOnlineCourse(course.CourseID);
            }

            var newItem = new OnlineCourse
            {
                CourseID = course.CourseID,
                URL = course.URL
            };

            Database.OnlineCourses.Add(newItem);
            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Updates the online course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public OnlineCourseDTO UpdateOnlineCourse(OnlineCourseDTO course)
        {
            if(course == null)
            {
                throw new ArgumentNullException("course");
            }
            if (Database.Courses.Any(x => x.CourseID == course.CourseID) == false)
            {
                throw new KeyNotFoundException($"The course ID {course.CourseID} was not found in the system.");
            }
            if (Database.OnlineCourses.Any(x => x.CourseID == course.CourseID))
            {
                DeleteOnlineCourse(course.CourseID);
            }
            
            AddOnlineCourse(course);

            return course;
        }

        /// <summary>
        /// Deletes the online course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        public int DeleteOnlineCourse(int courseID)
        {
            var target = Database.OnlineCourses.Where(x => x.CourseID == courseID).FirstOrDefault();

            if (target != null && target.CourseID == courseID)
            {
                Database.OnlineCourses.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }

        #endregion

        #region Onsite Courses

        /// <summary>
        /// Gets all onsite courses.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OnsiteCourseDTO> GetAllOnsiteCourses()
        {
            var results = Database.OnsiteCourses.Select(
              x => new OnsiteCourseDTO()
              {
                  CourseID = x.CourseID,
                  Location = x.Location,
                  Days = x.Days,
                  Time = x.Time

              }).ToList();

            return results;
        }

        /// <summary>
        /// Gets the onsite course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        public OnsiteCourseDTO GetOnsiteCourse(int courseID)
        {
            if (courseID <= 0)
            {
                return null;
            }
            if (!Database.OnsiteCourses.Any(x => x.CourseID == courseID))
            {
                return null;
            }

            var result = Database.OnsiteCourses.Where(x => x.CourseID == courseID).FirstOrDefault();
            if (result != null)
            {
                var newResult = new OnsiteCourseDTO
                {
                    CourseID = result.CourseID,
                    Location = result.Location,
                    Days = result.Days,
                    Time = result.Time
                };

                return newResult;
            }
            return null; //this should never happen.
        }

        /// <summary>
        /// Adds the onsite course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">course</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public OnsiteCourseDTO AddOnsiteCourse(OnsiteCourseDTO course)
        {
            if (course == null)
            {
                throw new ArgumentNullException("course");
            }
            if (Database.Courses.Any(x => x.CourseID == course.CourseID) == false)
            {
                throw new KeyNotFoundException($"The course ID {course.CourseID} was not found in the system.");
            }
            if (Database.OnsiteCourses.Any(x => x.CourseID == course.CourseID))
            {
                DeleteOnsiteCourse(course.CourseID);
            }

            var newItem = new OnsiteCourse
            {
                CourseID = course.CourseID,
                Location = course.Location,
                Days = course.Days,
                Time = course.Time
            };

            Database.OnsiteCourses.Add(newItem);
            Database.SaveChanges();

            return course;
        }

        /// <summary>
        /// Updates the onsite course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public OnsiteCourseDTO UpdateOnsiteCourse(OnsiteCourseDTO course)
        {
            if(course == null)
            {
                throw new ArgumentNullException("course");
            }
            if (Database.Courses.Any(x => x.CourseID == course.CourseID) == false)
            {
                throw new KeyNotFoundException($"The course ID {course.CourseID} was not found in the system.");
            }
            if (Database.OnsiteCourses.Any(x => x.CourseID == course.CourseID))
            {
                DeleteOnsiteCourse(course.CourseID);
            }

            AddOnsiteCourse(course);

            return course;
        }

        /// <summary>
        /// Deletes the onsite course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        public int DeleteOnsiteCourse(int courseID)
        {
            var target = Database.OnsiteCourses.Where(x => x.CourseID == courseID).FirstOrDefault();

            if (target != null && target.CourseID == courseID)
            {
                Database.OnsiteCourses.Remove(target);
                return Database.SaveChanges();
            }
            return -1;
        }

        #endregion
    }
}

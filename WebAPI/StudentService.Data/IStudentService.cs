using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentService.DTOs;

namespace StudentService.Data
{
    public interface IStudentService
    {
        #region People Methods
        /// <summary>
        /// Gets all people.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PersonDTO> GetAllPersons();

        /// <summary>
        /// Gets the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// A person DTO object. If no person was found, returns null.
        /// </returns>
        PersonDTO GetPerson(int personID);

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        PersonDTO CreatePerson(PersonDTO person);

        /// <summary>
        /// Updates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        PersonDTO UpdatePerson(PersonDTO person);

        /// <summary>
        /// Deletes the person.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns>
        /// The number of rows affected.
        /// If something failed, returns -1.
        /// </returns>
        int DeletePerson(int personID);
        #endregion

        #region Department Methods
        
        /// <summary>
        /// Gets all departments.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DepartmentDTO> GetAllDepartments();

        /// <summary>
        /// Gets the department.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns></returns>
        DepartmentDTO GetDepartment(int departmentID);

        /// <summary>
        /// Creates the department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns></returns>
        DepartmentDTO CreateDepartment(DepartmentDTO department);

        /// <summary>
        /// Updates the department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns></returns>
        DepartmentDTO UpdateDepartment(DepartmentDTO department);

        /// <summary>
        /// Deletes the department.
        /// </summary>
        /// <param name="departmentID">The department identifier.</param>
        /// <returns></returns>
        int DeleteDepartment(int departmentID);

        #endregion

        #region Course Methods

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseDTO> GetAllCourses();

        /// <summary>
        /// Gets the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        CourseDTO GetCourse(int courseID);

        /// <summary>
        /// Creates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        CourseDTO CreateCourse(CourseDTO course);

        /// <summary>
        /// Updates the course.
        /// </summary>
        /// <param name="course">The course.</param>
        /// <returns></returns>
        CourseDTO UpdateCourse(CourseDTO course);

        /// <summary>
        /// Deletes the course.
        /// </summary>
        /// <param name="courseID">The course identifier.</param>
        /// <returns></returns>
        int DeleteCourse(int courseID);

        #endregion

        #region Office Assignments

        /// <summary>
        /// Gets the office assignment.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns></returns>
        OfficeAssignmentDTO GetOfficeAssignment(int personID);

        /// <summary>
        /// Gets all office assignments.
        /// </summary>
        /// <returns></returns>
        OfficeAssignmentDTO GetAllOfficeAssignments();

        /// <summary>
        /// Creates the office assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        /// <returns></returns>
        OfficeAssignmentDTO CreateOfficeAssignment(OfficeAssignmentDTO assignment);

        /// <summary>
        /// Updates the office assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        /// <returns></returns>
        OfficeAssignmentDTO UpdateOfficeAssignment(OfficeAssignmentDTO assignment);

        /// <summary>
        /// Deletes the office assignment.
        /// </summary>
        /// <param name="personID">The person identifier.</param>
        /// <returns></returns>
        int DeleteOfficeAssignment(int personID);

        #endregion
    }
}

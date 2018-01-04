using System;
using System.Collections.Generic;

namespace StudentService.DTOs
{
   
    
    public partial class OnlineCourseDTO
    {
        public int CourseID { get; set; }

        public string URL { get; set; }

        public virtual CourseDTO Course { get; set; }
    }
}

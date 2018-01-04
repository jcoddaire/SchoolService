﻿using StudentService.Data;
using System.Web.Http;

namespace StudentService.Controllers
{
    public class ControllerBase : ApiController
    {
        private IStudentService _repository = null;

        internal IStudentService Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new StudentServiceRepository();
                }

                return _repository;
            }
        }
    }
}
using StudentService.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentService.Tests.Integration.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase
    {        
        private IStudentService _repository = null;

        internal IStudentService Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new StudentServiceRepository(new StudentDB());
                }

                return _repository;
            }
        }

    }
}

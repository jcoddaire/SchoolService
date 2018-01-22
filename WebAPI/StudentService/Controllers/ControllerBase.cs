using StudentService.Data;
using System.Web.Http;

namespace StudentService.WebAPI.Controllers
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
                    _repository = new StudentServiceRepository(new StudentDB());
                }

                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
    }
}
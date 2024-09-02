using System.Net;

namespace EmployeeManagementNextrek.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessful { get; set; } = true;

        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}

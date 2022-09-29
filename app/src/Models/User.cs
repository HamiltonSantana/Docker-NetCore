using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace ServerSide.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Pwd { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDWORKS_STUDENT.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models {
    public class CreateUserRequest {
        public Guid CongTyID { get; set; }
        public string HoTen { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.Staff;
    }
}

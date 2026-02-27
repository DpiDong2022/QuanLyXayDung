using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities {
    public enum UserStatus {
        Active,
        Inactive,
        Locked,
        Deleted
    }

    public enum UserRole {
        SuperAdmin,
        Boss,
        Staff
    }

    public class AppUser {
        public Guid ID { get; set; }
        public Guid CongTyID { get; set; }
        public CongTy? CongTy { get; set; }
        public string HoTen { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.Staff;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

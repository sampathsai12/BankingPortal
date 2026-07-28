using System;
using System.Collections.Generic;
using System.Text;

namespace BankingPortal.Domain.Entities
{
    public class User
    {
        public long UserId { get; set; }
        public long? CustomerId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public bool IsLocked { get; set; } = false;
        public DateTime? LastLoginDate { get; set; }

        public Customer? Customer { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole
    {
        public long UserRoleId { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }

        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}

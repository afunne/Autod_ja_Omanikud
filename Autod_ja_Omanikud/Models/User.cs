using System;

namespace Autod_ja_Omanikud.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Permissions
        public bool IsSuperAdmin { get; set; } // can't be changed/removed via UI
        public bool CanManageOwners { get; set; } // allows managing car owners
        public bool CanManageCars { get; set; } // allows managing cars with their owners
        public bool CanManageServices { get; set; } // allows managing services
        public bool CanManageMaintenance { get; set; } // allows managing maintenance
        public bool CanManageUsers { get; set; } // allows managing users

        public static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password ?? string.Empty);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}

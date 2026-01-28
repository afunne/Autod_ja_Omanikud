using System;

namespace Autod_ja_Omanikud.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsSuperAdmin { get; set; }

        public bool CanManageOwners { get; set; }
        public bool CanManageCars { get; set; }
        public bool CanManageServices { get; set; }
        public bool CanManageMaintenance { get; set; }
        public bool CanManageUsers { get; set; }

        public static string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password ?? string.Empty);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}

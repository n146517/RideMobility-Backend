using System.Text.RegularExpressions;

namespace RideMobility.Api.Services.Helpers
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password)
        {
            if (password.Length < 8) return false;
            if (!Regex.IsMatch(password, @"[A-Z]")) return false;
            if (!Regex.IsMatch(password, @"[a-z]")) return false;
            if (!Regex.IsMatch(password, @"[0-9]")) return false;
            if (!Regex.IsMatch(password, @"[\W_]")) return false;
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SideQuest.BLL.Models;

namespace SideQuest.BLL.Services
{
    public class RegisterService
    {
        private static readonly List<User> _users = new();

        public bool Register(RegisterRequest request)
        {
            if (request == null) return false;

            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.FullName))
                return false;

            if (request.Email != request.Email.Trim() ||
                request.Password != request.Password.Trim() ||
                request.FullName != request.FullName.Trim())
                return false;

            if (request.Email.Any(char.IsUpper) || !Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;

            if (!ValidatePassword(request.Password)) return false;
            if (!ValidateFullName(request.FullName)) return false;

            if (_users.Any(u => u.Email == request.Email)) return false;

            _users.Add(new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.FullName, 
                Username = request.Email.Split('@')[0] 
            });

            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 6) return false;
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) || !password.Any(c => "!@#$%^&*()_+-=.".Contains(c)))
                return false;
            return true;
        }

        private bool ValidateFullName(string fullName)
        {
            var words = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 2 || words.Length > 5 || fullName.Length > 50) return false;

            if (!fullName.All(c => char.IsLetter(c) || c == ' ' || c == '\'' || c == '_')) return false;
            if (fullName.Replace(" ", "").All(c => c == '\'' || c == '_')) return false;

            foreach (var word in words)
            {
                if (!char.IsUpper(word[0]) || word.Skip(1).Any(char.IsUpper)) return false;
            }
            return true;
        }

        public static void ClearUsers() => _users.Clear();
    }
}
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
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.County) ||
                string.IsNullOrWhiteSpace(request.City))
                return false;

            if (request.Email != request.Email.Trim() ||
                request.LastName != request.LastName.Trim() ||
                request.FirstName != request.FirstName.Trim())
                return false;

            if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return false;

            if (!ValidatePassword(request.Password)) return false;
            if (!ValidateName(request.LastName)) return false;
            if (!ValidateName(request.FirstName)) return false;
            if (!ValidatePhoneNumber(request.PhoneNumber)) return false;
            if (!ValidateBirthDate(request.BirthDate)) return false;

            if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
                return false;

            _users.Add(new User
            {
                Email = request.Email.ToLower(),
                Password = request.Password,
                LastName = request.LastName,
                FirstName = request.FirstName,
                County = request.County,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Username = request.Email.Split('@')[0]
            });

            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper) ||
                !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) ||
                !password.Any(c => "!@#$%^&*()_+-=.".Contains(c)))
                return false;
            return true;
        }

        private bool ValidateName(string name)
        {
            if (name.Length < 2 || name.Length > 30) return false;
            if (!Regex.IsMatch(name, @"^[A-Z][a-z]+$")) return false;
            return true;
        }

        private bool ValidatePhoneNumber(long phoneNumber)
        {
            string phoneStr = phoneNumber.ToString();
            return phoneStr.Length >= 9 && phoneStr.Length <= 10;
        }

        private bool ValidateBirthDate(DateTime birthDate)
        {
            if (birthDate >= DateTime.Now) return false;
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;
            return age >= 14 && age <= 100;
        }

        public static void ClearUsers() => _users.Clear();
    }
}
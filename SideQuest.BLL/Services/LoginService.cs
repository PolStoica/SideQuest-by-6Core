using System;
using System.Collections.Generic;
using System.Linq;
using SideQuest.BLL.Models;

namespace SideQuest.BLL.Services
{
    public class LoginService
    {
        private static readonly List<User> _users = new();
        private readonly Dictionary<string, LoginState> _loginStates = new();
        private readonly ITimeProvider _time;

        private const int MaxFailedAttempts = 3;
        private static readonly TimeSpan LockDuration = TimeSpan.FromDays(1);

        public LoginService(ITimeProvider time) => _time = time;

        public LoginService AddUserForTesting(RegisterRequest request)
        {
            if (!_users.Any(u => u.Email == request.Email))
            {
                _users.Add(new User
                {
                    Email = request.Email.ToLower().Trim(),
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    County = request.County,
                    City = request.City,
                    BirthDate = request.BirthDate,
                    PhoneNumber = request.PhoneNumber,
                    ProfilePicture = request.ProfilePicture,
                    SelectedCategories = request.SelectedCategories,
                    Username = GenerateProfessionalUsername(request.FirstName, request.LastName)
                });
            }
            return this;
        }

        public bool Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            if (email != email.Trim() || password != password.Trim())
                return false;

            var user = _users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            if (!_loginStates.ContainsKey(email))
                _loginStates[email] = new LoginState();

            var state = _loginStates[email];

            if (state.LockTime.HasValue)
            {
                if (_time.UtcNow - state.LockTime.Value < LockDuration)
                    return false;

                state.LockTime = null;
                state.FailedAttempts = 0;
            }

            if (user.Password == password)
            {
                state.FailedAttempts = 0;
                return true;
            }

            state.FailedAttempts++;
            if (state.FailedAttempts >= MaxFailedAttempts)
                state.LockTime = _time.UtcNow;

            return false;
        }
    }

    public class LoginState
    {
        public int FailedAttempts { get; set; }
        public DateTime? LockTime { get; set; }
    }
}
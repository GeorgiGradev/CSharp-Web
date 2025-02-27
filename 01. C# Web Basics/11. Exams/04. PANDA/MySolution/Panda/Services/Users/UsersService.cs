﻿using Panda.Data;
using Panda.Data.Models;
using Panda.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Panda.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void Create(RegisterInputModel register)
        {
            var user = new User
            {
                Username = register.Username,
                Email = register.Email,
                Password = ComputeHash(register.Password)
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public ICollection<string> GetAllUsernames()
        {
            var allUserNames = this.db.Users
                .Select(x => x.Username)
                .ToList();

            return allUserNames;
        }

        public string GetUserId(LoginInputModel login)
        {
            var hashPassword = ComputeHash(login.Password);
            var user = db.Users.FirstOrDefault(x => x.Username == login.Username && x.Password == hashPassword);

            return user?.Id;
        }

        public string GetUsername(string id)
        {
            var userName = this.db.Users
                .FirstOrDefault(x => x.Id == id).Username;

            return userName;
        }

        public bool IsEmailAvailable(RegisterInputModel register)
        {
            return !db.Users.Any(x => x.Email == register.Email);
        }

        public bool IsUsernameAvailable(RegisterInputModel register)
        {
            return !db.Users.Any(x => x.Username == register.Username);
        }

        private string ComputeHash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);

            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }
}


using System;
using System.Collections.Generic;
using WorldsBelly.DataAccess.Entities;

namespace WorldsBelly.API.Models
{
    public class UserView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
    }
    public class UserUpdateEmail
    {
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
        public string VerificationCode { get; set; }
    }
}

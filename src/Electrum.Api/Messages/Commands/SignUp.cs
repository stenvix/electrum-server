﻿using Newtonsoft.Json;
using System;

namespace Electrum.Api.Messages.Commands
{
    public class SignUp
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }

        [JsonConstructor]
        public SignUp(Guid id, string email, string password, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}

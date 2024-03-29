﻿using System.ComponentModel.DataAnnotations;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    public class UserAuthenticateRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
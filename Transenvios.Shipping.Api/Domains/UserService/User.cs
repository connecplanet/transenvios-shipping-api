﻿using System.Text.Json.Serialization;

namespace Transenvios.Shipping.Api.Domains.UserService
{
    public class User : BaseEntity<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        public int? DocumentId { get; set; }
        public string? DocumentType { get; set; }
    }
}
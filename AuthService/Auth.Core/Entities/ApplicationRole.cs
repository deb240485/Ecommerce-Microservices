﻿using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


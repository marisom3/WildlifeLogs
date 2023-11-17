﻿namespace WildlifeLogAPI.Models.DTO
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string>? Roles { get; set; }

    }
}

﻿namespace ClientApi.Core.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}

﻿namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

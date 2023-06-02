﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}

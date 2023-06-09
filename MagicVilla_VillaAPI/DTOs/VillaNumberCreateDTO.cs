﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.DTOs
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public string SpecialDetails { get; set; } = string.Empty;
        [Required]
        public int VillaId { get; set; }
    }
}

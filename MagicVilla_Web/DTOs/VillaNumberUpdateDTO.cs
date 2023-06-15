﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.DTOs
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public string SpecialDetails { get; set; } = string.Empty;
        [Required]
        public int VillaId { get; set; }
    }
}

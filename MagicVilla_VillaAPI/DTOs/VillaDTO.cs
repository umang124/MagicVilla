using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}

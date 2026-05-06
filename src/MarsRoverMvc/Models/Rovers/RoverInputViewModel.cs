using System.ComponentModel.DataAnnotations;

namespace MarsRoverMvc.Models.Rovers
{
    /// DTO for individual rover input
    public class RoverInputViewModel
    {
        [Range(0, 50)] public int StartX { get; set; }

        [Range(0, 50)] public int StartY { get; set; }

        [Required]
        [RegularExpression("N|E|S|W")]
        public string StartDirection { get; set; } = "N";

        [Required] public string Commands { get; set; } = string.Empty;
    }
}
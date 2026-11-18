using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Colleges
{
    public class RegisterCollegeRequestDto
    {
        [Required(ErrorMessage = "College name is required.")]
        [StringLength(200)]
        public string CollegeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "College code is required.")]
        [StringLength(50)]
        public string CollegeCode { get; set; } = string.Empty;

        [StringLength(200)]
        public string? UniversityName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string OfficialEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;

        [Url]
        [StringLength(255)]
        public string? Website { get; set; }
    }
}
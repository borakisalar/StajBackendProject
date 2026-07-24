using System.ComponentModel.DataAnnotations;

namespace StajBackendProject.Models.Dto
{
    public class AssignRoleDto
    {
        [Required]
        public int RoleId { get; set; }
    }
}

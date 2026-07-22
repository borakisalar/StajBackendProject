namespace StajBackendProject.Models.Dto
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? LockoutEnd { get; set; }
    }
}

namespace StajBackendProject.Models.Dto
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}

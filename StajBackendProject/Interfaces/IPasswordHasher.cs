namespace StajBackendProject.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}

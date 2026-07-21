namespace StajBackendProject.Interfaces
{
    public interface INotificationService
    {
        void Send(string receiver, string message);
    }
}

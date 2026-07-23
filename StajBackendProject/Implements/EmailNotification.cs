using StajBackendProject.Interfaces;

namespace StajBackendProject.Implements
{
    public class EmailNotification : INotificationService
    {
        public void Send(string receiver, string message)
        {
            Console.WriteLine($"[EMAIL SENT] Receiver: {receiver} | Message: {message}");
        }
    }
}

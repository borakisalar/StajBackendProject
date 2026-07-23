using StajBackendProject.Interfaces;

namespace StajBackendProject.Implements
{
    public class SmsNotification : INotificationService
    {
        public void Send(string receiver, string message)
        {
            Console.WriteLine($"[SMS SENT] Receiver: {receiver} | Message: {message}");
        }
    }
}

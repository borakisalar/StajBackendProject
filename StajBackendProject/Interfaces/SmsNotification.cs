namespace StajBackendProject.Interfaces
{
    public class SmsNotification : INotificationService
    {
        public void Send(string receiver, string message)
        {
            Console.WriteLine($"[SMS SENT] Receiver: {receiver} | Message: {message}");
        }
    }
}

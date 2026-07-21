using StajBackendProject.Interfaces;

namespace StajBackendProject.Factories
{
    public class NotificationFactory
    {
        public INotificationService CreateNotification(string notificationType)
        {
            switch (notificationType.ToLower())
            {
                case "email":
                    return new EmailNotification();
                case "sms":
                    return new SmsNotification();
                default:
                    throw new ArgumentException("Invalid notification type!");
            }
        }
    }
}

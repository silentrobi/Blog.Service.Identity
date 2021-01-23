using MassTransit;
using System;
using System.Threading.Tasks;
using SharedLibrary;

namespace Blog.Service.Identity.Application.IntegrationEvents
{
    public class CreateUserAccountNotificationEvent<T> : NotificationEvent<T> where T: class
    {
        public static async Task Publish(IPublishEndpoint endpoint, T notification)
        {
            try
            {
                await endpoint.Publish(notification);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}

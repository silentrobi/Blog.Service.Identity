﻿using MassTransit;
using SharedLibrary;
using System;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Application.IntegrationEvents
{
    public class ChangeUserAccountPasswordNotificationEvent<T> : NotificationEvent<T> where T : class
    {
        public async Task Publish(IPublishEndpoint endpoint, T notification)
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
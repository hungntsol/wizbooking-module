﻿using MediatR;
using SharedCommon.Domain;

namespace SharedCommon.Commons.Notification;

public abstract class NotificationEventHandler<TEvent> : INotificationHandler<DomainEvent<TEvent>>
    where TEvent : class
{
    public virtual async Task Handle(DomainEvent<TEvent> notification, CancellationToken cancellationToken)
    {
        await HandleAsync(notification, cancellationToken);
    }

    protected abstract Task HandleAsync(DomainEvent<TEvent> @event, CancellationToken cancellationToken = default);
}

﻿using CarRentalCo.Common.Application.Messages;
using System;
using System.Threading.Tasks;

namespace CarRentalCo.Common.Application.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
        Task HandleAsync(T command, Guid correlationId);
    }
}

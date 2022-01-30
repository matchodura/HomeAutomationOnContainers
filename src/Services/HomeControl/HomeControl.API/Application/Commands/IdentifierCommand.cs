using MediatR;
using System;

namespace HomeControl.API.Application.Commands
{
    public class IdentifierCommand<T, R> : IRequest<R>
        where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }
        public IdentifierCommand(T command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}

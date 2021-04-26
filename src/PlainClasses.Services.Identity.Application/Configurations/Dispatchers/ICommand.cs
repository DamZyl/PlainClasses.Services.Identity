using System;
using MediatR;

namespace PlainClasses.Services.Identity.Application.Configurations.Dispatchers
{
    public interface ICommand : IRequest
    {
        public Guid Id { get; }
    }
    
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid Id { get; }
    }
}
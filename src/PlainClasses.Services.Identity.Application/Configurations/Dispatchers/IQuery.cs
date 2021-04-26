using MediatR;

namespace PlainClasses.Services.Identity.Application.Configurations.Dispatchers
{
    public interface IQuery<out TResult> : IRequest<TResult> { }
}
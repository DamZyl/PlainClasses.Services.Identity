using System.Threading.Tasks;
using MassTransit;
using MicroserviceLibrary.Domain.Repositories;
using PlainClasses.Services.Identity.Domain.Models;
using SharedModels;

namespace PlainClasses.Services.Identity.Application.Utils.Consumers
{
    public class AuthAddedIntegrationEventConsumer : IConsumer<AuthAddedIntegrationEvent>
    {
    private readonly IUnitOfWork _unitOfWork;

    public AuthAddedIntegrationEventConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
        
    public async Task Consume(ConsumeContext<AuthAddedIntegrationEvent> context)
    {
        var message = context.Message;

        var person = await _unitOfWork.Repository<Person>().FindByIdAsync(message.PersonId);
        
        person.AddAuthToPerson(message.AuthName);

        await _unitOfWork.CommitAsync();
    }
    }
}
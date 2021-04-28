using System.Threading.Tasks;
using MassTransit;
using MicroserviceLibrary.Domain.Repositories;
using PlainClasses.Services.Identity.Domain.Models;
using SharedModels;

namespace PlainClasses.Services.Identity.Application.Utils.Consumers
{
    public class PersonCreatedIntegrationEventConsumer : IConsumer<PersonCreatedIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonCreatedIntegrationEventConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task Consume(ConsumeContext<PersonCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            var person = Person.CreatePerson(message.PersonId, message.PersonalNumber, message.MilitaryRankAcr,
                message.Password, message.FirstName, message.LastName);

            await _unitOfWork.Repository<Person>().AddAsync(person);
            await _unitOfWork.CommitAsync();
        }
    }
}
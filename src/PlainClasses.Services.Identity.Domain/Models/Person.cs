using System;
using System.Collections.Generic;
using MicroserviceLibrary.Domain.SharedKernels;

namespace PlainClasses.Services.Identity.Domain.Models
{
    public class Person : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string PersonalNumber { get; private set; }
        public string MilitaryRankAcr { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private ISet<PersonAuth> _personAuths = new HashSet<PersonAuth>();
        public IEnumerable<PersonAuth> PersonAuths => _personAuths;

        private Person(Guid id, string personalNumber, string militaryRankAcr, string password, string firstName, string lastName)
        {
            Id = id;
            PersonalNumber = personalNumber;
            MilitaryRankAcr = militaryRankAcr;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public static Person CreatePerson(Guid id, string personalNumber, string militaryRankAcr, string password,
            string firstName, string lastName)
            => new Person(id, personalNumber, militaryRankAcr, password, firstName, lastName);
        
        public void AddAuthToPerson(string authName)
        {
            var auth = PersonAuth.CreateAuthForPerson(Id, authName);
            _personAuths.Add(auth);
        }
    }
}
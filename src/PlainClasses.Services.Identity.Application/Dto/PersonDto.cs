using System;
using System.Collections.Generic;

namespace PlainClasses.Services.Identity.Application.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public string Password { get; set; }
        public string MilitaryRankAcr { get; set; }
        public List<AuthDto> PersonAuths { get; set; }
    }
}
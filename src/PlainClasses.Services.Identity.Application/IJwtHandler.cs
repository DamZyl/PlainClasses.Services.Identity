using System;
using System.Collections.Generic;
using PlainClasses.Services.Identity.Application.Commands;

namespace PlainClasses.Services.Identity.Application
{
    public interface IJwtHandler
    {
        string CreateToken(Guid userId, string fullName, IEnumerable<AuthDto> auths);
    }
}
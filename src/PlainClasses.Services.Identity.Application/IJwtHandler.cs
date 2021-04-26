using System;
using System.Collections.Generic;
using PlainClasses.Services.Identity.Application.Commands;
using PlainClasses.Services.Identity.Application.Dto;

namespace PlainClasses.Services.Identity.Application
{
    public interface IJwtHandler
    {
        string CreateToken(Guid userId, string fullName, List<AuthDto> auths);
    }
}
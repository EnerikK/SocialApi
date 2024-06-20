﻿using MediatR;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.Identity.Commands;

public class LoginCommand : IRequest<OperationResult<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}
﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.UserProfiles.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Application_UseCases_.Models;

namespace Social.Application_UseCases_.UserProfiles.CommandHandlers
{

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
    {
        private readonly DataContext  _Datactx;
        public CreateUserCommandHandler(DataContext Datactx)
        {
            _Datactx = Datactx;
        }
        public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            //Creates a new instance 
            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName,request.LastName
                ,request.EmailAddress,request.Phone,request.DateOfBirth,request.CurrentCity); 
            //The request is source and basicinfo is destination
            //Factory Method
            //Creates a userprofile TODO:when add Validation have to add a TRY CATCH for error handle
            var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(),basicInfo);
            _Datactx.UserProfiles.Add(userProfile);
            await _Datactx.SaveChangesAsync(); //Asynchronously saves all changes made in this context to the underlying database
            result.PayLoad = userProfile;
            return result;
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application_UseCases_.Enums;
using Social.Application_UseCases_.Models;
using Social.Application_UseCases_.Posts.Commands;
using Social.DataAccess;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;

namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostText,OperationResult<Post>>
{
    private readonly DataContext _dataContext;

    public UpdatePostHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<OperationResult<Post>> Handle(UpdatePostText request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(post => post.PostId == request.PostId);
            if (post is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No Post Found With ID {request.PostId}"
                };
                result.Errors.Add(error);
                return result;
            }

            post.UpdatePostText(request.NewText);
            await _dataContext.SaveChangesAsync();
            result.PayLoad = post;
        }
        catch (PostNotValidException e)
        {
            result.IsError = true;
            e.ValidationErrors.ForEach(resultError =>
            {
                var error = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{e.Message}"
                };
                result.Errors.Add(error);
            });
        }
        catch (Exception ex)
        {
            var error = new Error
            {
                Code = ErrorCode.UnknownError,
                Message = $"{ex.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }
}
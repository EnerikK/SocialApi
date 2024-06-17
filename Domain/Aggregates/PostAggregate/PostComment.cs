using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;

namespace Social.Domain.Aggregates.PostAggregate
{
    
    public class PostComment
    {
        private PostComment()
        {

        }
        public Guid CommentId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public string Text { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }

        //Factory Method
        /// <summary>
        /// Create post comment  
        /// </summary>
        /// <param name="postId">The ID of the post to which the comment belongs to</param>
        /// <param name="text">User Input</param>
        /// <param name="userProfileId">The ID of the user who created the comment</param>
        /// <returns><see cref="PostComment"/></returns>
        /// <exception cref="PostCommentNotValidException">Throw if data provided is not valid</exception>
        public static PostComment CreatePostComment(Guid postId,string text,Guid userProfileId)
        {
            var validator = new PostCommentValidator();
            var objectToValidate =  new PostComment
            {
                PostId = postId,
                Text = text,
                UserProfileId = userProfileId,
                DateCreated = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            var validationResult = validator.Validate(objectToValidate);

            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostCommentNotValidException("PostComment is not valid");
            validationResult.Errors.ForEach(result => exception.ValidationErrors.Add(result.ErrorMessage));
            throw exception;
        }
        //Public Method
        public void UpdateCommentText(string newText)
        {
            Text = newText;
            LastModified = DateTime.UtcNow;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Aggregates.UserProfileAggregate;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;


namespace Social.Domain.Aggregates.PostAggregate
{
    public class Post
    {
        private readonly List<PostComment> _comments = new List<PostComment>();
        private readonly List<PostInteraction> _interactions = new List<PostInteraction>();
        private Post() // Constructor
        {
            
        }
        public Guid PostId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public string TextContent { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime LastModified {  get; private set; }
        //Using IEnumerable instead of IConnection  because once its instantiated
        //users could still add things to them which we want to avoid 
        public IEnumerable<PostComment> Comments { get { return _comments; } }
        public IEnumerable<PostInteraction> Interactions { get  { return _interactions; } }

        //Factories
        /// <summary>
        /// Create a new instance of a post 
        /// </summary>
        /// <param name="userProfileId">The userprofileId</param>
        /// <param name="textContent">The Given Post Content</param>
        /// <returns><see cref="Post"/></returns>
        /// <exception cref="PostNotValidException"></exception>
        public static Post CreatePost(Guid userProfileId , string textContent)
        {
            var validator = new PostValidator();
            var objectToValidate =  new Post
            {
                UserProfileId = userProfileId ,
                TextContent = textContent,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
            
            var validationResult = validator.Validate(objectToValidate);
            if (validationResult.IsValid) return objectToValidate;

            var exception = new PostNotValidException("Post is not valid ");
            validationResult.Errors.ForEach(result => exception.ValidationErrors.Add(result.ErrorMessage));
            throw exception;
        }
        //Public Methods
        /// <summary>
        /// Update Post Text
        /// </summary>
        /// <param name="newText">The updated post text</param>
        /// <exception cref="PostNotValidException"></exception>
        public void UpdatePostText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
            {
                var exception = new PostNotValidException("Cannot Update Post. The Post Text Is Probably Not Valid");
                exception.ValidationErrors.Add("The Provided Post Text Is Either Null or WhiteSpace");
                throw exception;
            }
            TextContent = newText;
            LastModified = DateTime.UtcNow;
        }
        public void AddPostComment(PostComment newComment)
        {
            _comments.Add(newComment);
        }
        public void RemoveComment(PostComment toRemove)
        {
            _comments.Remove(toRemove);
        }

        public void UpdatePostComment(Guid postCommentId, string updatedComment)
        {
            var comment = _comments.FirstOrDefault(com => com.CommentId == postCommentId);
            if (comment != null && !string.IsNullOrWhiteSpace(updatedComment)) comment.UpdateCommentText(updatedComment);
        }
        public void AddInteraction(PostInteraction newInteraction)
        {
            _interactions.Add(newInteraction);
        }
        public void RemoveInteraction(PostInteraction toRemove)
        {
            _interactions.Remove(toRemove);
        }
    }
}

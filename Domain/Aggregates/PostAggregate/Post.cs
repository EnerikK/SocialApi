using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Aggregates.UserProfileAggregate;
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
            
        }
        //Public Methods
        public void UpdatePostText(string newText)
        {
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

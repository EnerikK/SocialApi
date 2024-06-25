using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Aggregates.UserProfileAggregate;

namespace Social.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {

        }
        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public Guid? UserProfileId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public InteractionTypes InteractionType { get; private set; }

        //Factory Method
        public static PostInteraction CreatePostInteraction(Guid postId, InteractionTypes type,Guid userProfileId)
        {
            return new PostInteraction
            {
                PostId = postId,
                InteractionType = type,
                UserProfileId = userProfileId
            };
        }

    }
}

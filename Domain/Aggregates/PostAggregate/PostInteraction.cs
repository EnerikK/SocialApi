using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.PostAggregate
{
    public class PostInteraction
    {
        private PostInteraction()
        {

        }
        public Guid InteractionId { get; private set; }
        public Guid PostId { get; private set; }
        public InteractionTypes InteractionType { get; private set; }

        //Factory Method
        public static PostInteraction CreatePostInteraction(Guid postId, InteractionTypes type)
        {
            return new PostInteraction
            {
                PostId = postId,
                InteractionType = type 
            };
        }

    }
}

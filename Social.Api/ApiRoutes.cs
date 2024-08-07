﻿namespace Social.Api
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiversion}/[controller]";
        public static class UserProfiles
        {
            public const string IdRoute = "{id}";
        }
        public static class Posts
        {
            public const string IdRoute = "{id}";
            public const string PostComment = "{postId}/comments";
            public const string CommentById = "{postId}/comments{commentId}";
            public const string Interaction = "{postId}/interactions";
            public const string PostInteractions = "{postId}/interactions";
            public const string DeleteInteraction = "{postId}/interactions/{interactionId}";
        }
        public static class Identity
        {
            public const string Login = "login";
            public const string Registration = "registration";
            public const string IdentityById = "{identityUserId}";
            public const string CurrentUser = "currentuser";
        }

        public static class FriendRequest
        {
            public const string FriendRequestCreate = "friendRequest";
            public const string FriendRequestAccept = "friendRequest/{friendRequestId}/accept";
            public const string FriendRequestReject = "friendRequest/{friendRequestId}/reject";
        }
    }

}

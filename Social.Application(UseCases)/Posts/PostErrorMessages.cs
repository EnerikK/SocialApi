﻿namespace Social.Application_UseCases_.Posts.CommandHandlers;

public class PostErrorMessages
{
    public const string PostNotFound = "No post found with ID {0}";
    public const string PostDeleteNotPossible = "Only the owner of a post can delete the post";
    public const string PostUpdateNotPossible = "Post update not possible because it's not the post owner that called the update";
    public const string PostInteractionNotFound = "Interaction not found";
    public const string InteractionRemovalNotAuthorized = "Cannot remove interaction as you are not its author";
    public const string PostCommentNotFound = "Comment not found";
    public const string CommentRemovalNotAuthorized = "Cannot remove comment from post as you are not its author";
}
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
        }
    }

}

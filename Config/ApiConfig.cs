namespace tplayer.Config
{
    public static class ApiConfig
    {
        /// <summary>
        /// The base URL for the TafaPlayer API server.
        /// Format: http://{host}:{port}/api/v1
        /// This should point to your local development server or production API endpoint.
        /// </summary>
        public static string BaseUrl { get; } = "http://45.79.97.25:5600/api/v1";

        public static class Endpoints
        {
            private static string Auth => $"{BaseUrl}/users/auth";
            
            public static string Login => $"{Auth}/login";
            public static string Register => $"{Auth}/register";
            public static string RefreshToken => $"{Auth}/refresh";
            public static string ForgotPassword => $"{Auth}/forgot-password";
            public static string ResetPassword => $"{Auth}/reset-password";
        }
    }
} 
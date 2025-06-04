using System;

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
            private static string Auth => $"{BaseUrl.TrimEnd('/')}/users/auth";
            
            public static string Login => $"{Auth.TrimEnd('/')}/login";
            public static string Register => $"{Auth.TrimEnd('/')}/register";
            public static string RefreshToken => $"{Auth.TrimEnd('/')}/refresh";
            public static string ForgotPassword => $"{Auth.TrimEnd('/')}/forgot-password";
            public static string ResetPassword => $"{Auth.TrimEnd('/')}/reset-password";

            // Helper method to ensure URL is properly formatted
            private static string EnsureValidUrl(string url)
            {
                return new Uri(url).AbsoluteUri;
            }
        }
    }
} 
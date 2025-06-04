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
        private static readonly Uri _baseUri = new Uri("http://45.79.97.25:5600/api/v1");

        public static string BaseUrl => _baseUri.AbsoluteUri.TrimEnd('/');

        public static class Endpoints
        {
            public static string Login => BuildUrl("users/auth/login");
            public static string Register => BuildUrl("users/auth/register");
            public static string RefreshToken => BuildUrl("users/auth/refresh");
            public static string ForgotPassword => BuildUrl("users/auth/forgot-password");
            public static string ResetPassword => BuildUrl("users/auth/reset-password");

            private static string BuildUrl(string path)
            {
                try
                {
                    return new Uri(_baseUri, path.TrimStart('/')).AbsoluteUri;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error building URL for path '{path}': {ex.Message}");
                    throw new ArgumentException($"Invalid API endpoint path: {path}", nameof(path), ex);
                }
            }
        }
    }
} 
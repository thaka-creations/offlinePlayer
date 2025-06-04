namespace tplayer.Config
{
    public static class ApiConfig
    {
        public static string BaseUrl { get; } = "http://localhost:5600/api/v1";

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
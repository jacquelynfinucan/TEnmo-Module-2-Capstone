using TenmoClient.Models;

namespace TenmoClient
{
    public static class UserService
    {
        private static ApiUser user = new ApiUser();

        public static void SetLogin(ApiUser u)
        {
            user = u;
        }

        public static int GetUserId()
        {
            return user.UserId;
        }

        public static bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(user.Token);
        }

        public static string GetToken()
        {
            return user?.Token ?? string.Empty;
        }
    }
}

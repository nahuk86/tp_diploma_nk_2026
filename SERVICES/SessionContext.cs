using DOMAIN.Entities;

namespace SERVICES
{
    public static class SessionContext
    {
        private static User _currentUser;

        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public static int? CurrentUserId
        {
            get { return _currentUser?.UserId; }
        }

        public static string CurrentUsername
        {
            get { return _currentUser?.Username; }
        }

        public static void Clear()
        {
            _currentUser = null;
        }
    }
}

using Ovr.Domain.Models;

namespace Ovr.BlazorApp.Extensions
{
    public static class UserSession
    {
        private static UserInfo _session = new UserInfo(); // Instancia única en memoria

        public static UserInfo? SetSession
        {
            set => _session = value;
        }

        public static UserInfo GetSession
        {
            get => _session;
        }
    }
}

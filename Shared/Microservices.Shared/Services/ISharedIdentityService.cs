namespace Microservices.Shared.Services
{
    public interface ISharedIdentityService
    {
        public string GetCurrentUserId();
        public string GetCurrentUserIdFromClient();
    }
}
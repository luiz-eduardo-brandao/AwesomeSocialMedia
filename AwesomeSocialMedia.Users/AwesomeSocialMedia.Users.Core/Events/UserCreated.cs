namespace AwesomeSocialMedia.Users.Core.Events
{
    public class UserCreated : IEvent
    {
        public UserCreated(string email, string display)
        {
            Email = email;
            Display = display;
        }

        public string Email { get; private set; }
        public string Display { get; private set; }
    }
}

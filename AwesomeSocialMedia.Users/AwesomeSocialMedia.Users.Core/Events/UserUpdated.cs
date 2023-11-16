namespace AwesomeSocialMedia.Users.Core.Events
{
    public class UserUpdated : IEvent
    {
        public UserUpdated(Guid id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public Guid Id { get; set; }
        public string DisplayName { get; set; }
    }
}
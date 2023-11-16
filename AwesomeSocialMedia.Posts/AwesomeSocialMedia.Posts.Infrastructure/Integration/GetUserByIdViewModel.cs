namespace AwesomeSocialMedia.Posts.Infrastructure.Integration
{
    public class GetUserByIdViewModel
    {
        public string DisplayName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Header { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
    }
}

using System;
using AwesomeSocialMedia.Newsfeed.API.Core.Entities;

namespace AwesomeSocialMedia.Newsfeed.API.Core.Repositories
{
	public interface IUserNewsfeedRepository
	{
		Task AddPost(User user, Post post);
		Task<List<UserNewsfeed?>> GetAll();
		Task<UserNewsfeed?> GetByUserId(Guid userId);
        Task UpdateUserNewsFeed(User user);
    }
}


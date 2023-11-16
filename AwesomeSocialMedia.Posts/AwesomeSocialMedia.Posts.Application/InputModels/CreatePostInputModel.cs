using AwesomeSocialMedia.Posts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeSocialMedia.Posts.Application.InputModels
{
    public class CreatePostInputModel
    {
        public string Content { get; set; }
        public string UserDisplayName { get; set; }
        public Guid UserId { get; set; }

        public Post ToEntity() => new Post(Content, new User(UserId, UserDisplayName));
    }
}

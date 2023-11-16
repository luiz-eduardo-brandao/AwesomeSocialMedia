using AwesomeSocialMedia.Users.Application.Models;
using AwesomeSocialMedia.Users.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeSocialMedia.Users.Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<BaseResult<GetUserByIdViewModel>>
    {
        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}

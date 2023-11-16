using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeSocialMedia.Posts.Infrastructure.EventBus
{
    public interface IEventBus
    {
        void Publish<T>(T @event);
    }
}

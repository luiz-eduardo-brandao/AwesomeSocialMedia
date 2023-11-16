using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeSocialMedia.Users.Core.ValueObjects
{
    public record ContactInfo(string Email, string Website, string PhoneNumber)
    {
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Common.Exceptions
{
    public class IdentityException : ApplicationException
    {
        public IdentityException(string message, List<IdentityError> errors) : base(message)
        {
            Errors = errors;
        }

        public List<IdentityError> Errors { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NightClub.Core.Exceptions
{
    public class CustomBadRequestException : Exception
    {
        public CustomBadRequestException(string message) : base(message) { }
    }
}

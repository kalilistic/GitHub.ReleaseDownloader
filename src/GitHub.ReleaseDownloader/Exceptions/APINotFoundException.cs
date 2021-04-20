using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.ReleaseDownloader.Exceptions
{
    [Serializable]
    class APINotFoundException : APIException
    {
        public APINotFoundException()
        {

        }
        public APINotFoundException(string message) : base(message)
        {

        }
        public APINotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

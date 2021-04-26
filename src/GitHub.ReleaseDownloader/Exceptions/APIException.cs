using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.ReleaseDownloader.Exceptions
{
    [Serializable]
    class APIException : Exception
    {
        public APIException()
        {

        }
        public APIException(string message) : base(message)
        {

        }
        public APIException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

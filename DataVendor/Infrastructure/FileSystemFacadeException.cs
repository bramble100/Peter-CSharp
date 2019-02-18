using System;
using System.Runtime.Serialization;

namespace Infrastructure
{
    [Serializable]
    internal class FileSystemFacadeException : Exception
    {
        public FileSystemFacadeException()
        {
        }

        public FileSystemFacadeException(string message) : base(message)
        {
        }

        public FileSystemFacadeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FileSystemFacadeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
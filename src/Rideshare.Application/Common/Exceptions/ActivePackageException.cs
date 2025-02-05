namespace Rideshare.Application.Common.Exceptions
{
    public class ActivePackageException : Exception
    {
        public ActivePackageException(string message) : base(message) { }
    }
}
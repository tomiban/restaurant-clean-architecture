namespace Restaurant.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("User is not authorized")
    {
    }
}
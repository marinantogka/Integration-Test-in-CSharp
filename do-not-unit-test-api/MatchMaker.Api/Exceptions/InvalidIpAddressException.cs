namespace MatchMaker.Api.Exceptions;

public class InvalidIpAddressException : Exception
{
    public InvalidIpAddressException(string ipAddress)
    {
        this.IpAddress = ipAddress;
    }

    public string IpAddress { get; }
}
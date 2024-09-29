namespace RaptorUtils.Net;

using System.Net;
using System.Net.Sockets;

public static class PortFinder
{
    private static readonly IPEndPoint DefaultLoopbackEndpoint = new(IPAddress.Loopback, port: 0);

    public static int GetAvailablePort()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(DefaultLoopbackEndpoint);
        if (socket.LocalEndPoint == null)
        {
            throw new InvalidOperationException("Failed to get available port.");
        }

        return ((IPEndPoint)socket.LocalEndPoint).Port;
    }
}

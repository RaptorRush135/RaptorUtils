namespace RaptorUtils.Net;

using System.Net;
using System.Net.Sockets;

using RaptorUtils.CodeAnalysis;

/// <summary>
/// Utility class for finding an available port on the local machine.
/// </summary>
public static class PortFinder
{
    private static readonly IPEndPoint DefaultLoopbackEndpoint = new(IPAddress.Loopback, port: 0);

    /// <summary>
    /// Finds and returns an available TCP port.
    /// </summary>
    /// <returns>
    /// The port number of an available TCP port.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the socket fails to bind to a local endpoint and retrieve the port number.
    /// </exception>
    [MustUseReturnValue]
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

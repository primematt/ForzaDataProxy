using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ForzaDataProxy
{
    public class ListenServer
    {
        private readonly IPAddress BindAddress;
        private readonly int Port;
        private readonly ILogger Logger;

        private ArrayList Clients = new ArrayList();
        private TcpListener? Server = null;

        public ListenServer(string bindAddress, int port, ILogger logger)
        {
            BindAddress = IPAddress.Parse(bindAddress);
            Port = port;
            Logger = logger;
        }

        public void Start(CancellationToken cancellationToken)
        {
            Server = new TcpListener(BindAddress, Port);
            _ = StartListener(cancellationToken);
        }

        private async Task StartListener(CancellationToken cancellationToken)
        {
            if (Server == null)
            {
                Logger.LogError($"Attempting to start ListenServer: Server is null");
                return;
            }

            try
            {
                Server.Start();
                Logger.LogInformation($"ListenServer listening on {BindAddress}:{Port}");
                while (!cancellationToken.IsCancellationRequested)
                {
                    TcpClient client = await Server.AcceptTcpClientAsync(cancellationToken);
                    _ = ClientHandler(client, cancellationToken);
                }
            }
            catch (SocketException e)
            {
                Logger.LogError(e, "SocketException:");
            }

            Logger.LogInformation("ListenServer shutting down");
            Server.Stop();
        }

        private void AddClient(TcpClient client)
        {
            lock (Clients.SyncRoot)
            {
                Clients.Add(client);
            }
        }

        private void RemoveClient(TcpClient client)
        {
            lock (Clients.SyncRoot)
            {
                Clients.Remove(client);
            }
        }

        private async Task ClientHandler(TcpClient client, CancellationToken cancellationToken)
        {
            IPEndPoint? endPoint = (IPEndPoint?)client.Client.RemoteEndPoint;
            Logger.LogInformation($"Client Connected: {endPoint?.Address}");
            AddClient(client);

            TcpState clientState = client.GetState();
            while (clientState == TcpState.Established && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(1000, cancellationToken);
                    clientState = client.GetState();
                }
                catch
                {
                    // ignored
                }
            }

            if (client.GetState() == TcpState.Established)
            {
                try
                {
                    client.Close();
                }
                catch
                {
                    // ignored
                }
            }
            Logger.LogInformation($"Client Disconnected: {endPoint?.Address}");

            RemoveClient(client);
        }

        public async Task Broadcast(byte[] data)
        {
            foreach (TcpClient client in Clients)
            {
                await client.GetStream().WriteAsync(data);
            }
        }
    }
}
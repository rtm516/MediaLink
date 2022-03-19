using Fleck;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaLink
{
    internal class WebSocketManager
    {
        WebSocketServer server;
        List<IWebSocketConnection> connections;
        private MediaInfoWrapper lastMediaWrapper;

        public WebSocketManager()
        {
            connections = new List<IWebSocketConnection>();

            Restart();
        }

        private void OnOpen(IWebSocketConnection socket)
        {
            connections.Add(socket);
            Console.WriteLine("Open socket " + socket.ConnectionInfo.Id);

            if (lastMediaWrapper != null)
            {
                String jsonData = JsonSerializer.Serialize(lastMediaWrapper, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                socket.Send(jsonData);
            }
        }

        private void OnClose(IWebSocketConnection socket)
        {
            connections.Remove(socket);
            Console.WriteLine("Close socket " + socket.ConnectionInfo.Id);
        }

        private void OnMessage(IWebSocketConnection socket, String message)
        {
            switch (message)
            {
                case "play":
                    LinkManager.CurrentSession.TryTogglePlayPauseAsync();
                    break;
                case "prev":
                    LinkManager.CurrentSession.TrySkipPreviousAsync();
                    break;
                case "next":
                    LinkManager.CurrentSession.TrySkipNextAsync();
                    break;
                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        internal void Restart()
        {
            if (server != null) {
                server.ListenerSocket.Close();
                server.Dispose();
            }

            connections.Clear();

            server = new WebSocketServer("ws://" + Properties.Settings.Default.ListenAddress + ":" + Properties.Settings.Default.ListenPort);
            server.RestartAfterListenError = true;
            server.Start(socket =>
            {
                socket.OnOpen = () => OnOpen(socket);
                socket.OnClose = () => OnClose(socket);
                socket.OnMessage = message => OnMessage(socket, message);
            });
        }

        public void BroadcastMessage(MediaInfoWrapper mediaWrapper)
        {
            String jsonData = JsonSerializer.Serialize(mediaWrapper, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Console.WriteLine("Broadcasting: " + jsonData);

            lastMediaWrapper = mediaWrapper;

            foreach (IWebSocketConnection connection in connections)
            {
                connection.Send(jsonData);
            }
        }

        public bool IsRunning()
        {
            try
            {
                return server.ListenerSocket.LocalEndPoint != null;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }
}
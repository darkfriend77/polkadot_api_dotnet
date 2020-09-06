using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;

namespace SubstrateMetadata
{
    public class Client
    {
        private Uri uri;

        private readonly ClientWebSocket socket;

        private JsonRpc jsonRpc;

        private readonly CancellationTokenSource cts;

        public Client(Uri uri)
        {
            this.uri = uri;
            this.socket = new ClientWebSocket();
            this.cts = new CancellationTokenSource();
        }



        internal void ConnectAsync()
        {

            var task = socket.ConnectAsync(new Uri("wss://boot.worldofmogwais.com"), cts.Token);
            task.Wait();
            jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket));
            jsonRpc.StartListening();
        }

        internal string RequestAsync(string methode, object param = null)
        {

            var task = jsonRpc.InvokeWithCancellationAsync<string>(methode, null, cts.Token);
            task.Wait();
            return task.Result;

        }

        internal void Disconnect()
        {
            cts.Cancel();
        }
    }
}

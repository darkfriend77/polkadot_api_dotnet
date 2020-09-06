using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace SubstrateMetadata
{
    public class Program
    {
        static void Main(string[] args)
        {
            var webSocketURL = "wss://boot.worldofmogwais.com";
            var client = new Client(new Uri(webSocketURL));

            client.ConnectAsync();
            var result = client.RequestAsync("state_getMetadata");
            client.Disconnect();

            var metaData = new MetaDataParser(webSocketURL, result);
            var md11 = metaData.Parse();

            var md11serialized = JsonConvert.SerializeObject(md11, new StringEnumConverter());

            Console.WriteLine(md11serialized);
        }

    }
}
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


            var metaDataParser = new MetaDataParser(webSocketURL, result);
            var metaData = metaDataParser.MetaData;

            if (client.TryRequest(metaData, "Sudo", "Key", out object reqResult))
            {
                Console.WriteLine($"RESPONSE: {reqResult} [{reqResult.GetType().Name}]");
            }
            else
            {

            }


            client.Disconnect();





        }

    }
}
using Newtonsoft.Json.Linq;
using Polkadot;
using Polkadot.Api;
using Polkadot.Data;
using Polkadot.DataStructs;
using Polkadot.DataStructs.Metadata;
using Polkadot.Utils;
using System;

namespace TestDMogApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (IApplication app = PolkaApi.GetApplication())
            {

                int retCode = app.Connect("wss://boot.worldofmogwais.com/");
                //int retCode = app.Connect("ws://localhost:9944/"); 
                if (retCode > 0)
                {
                    Console.WriteLine("Failed to Connect!");
                    Console.ReadKey();
                    return;
                }

                GetMetaData(app);


                //var something = app.GetStorage("TemplateModule", "Something");
                //Console.WriteLine($"Something: {something}");

                //var allMogwaisCount = app.GetStorage("Dmog", "AllMogwaisCount");
                //Console.WriteLine($"all mogwais count: {allMogwaisCount}");

                //var allMogwaisCount = app.GetStorage("Balances", "TotalIssuance");
                //Console.WriteLine($"all mogwais count: {allMogwaisCount}");

                //var result = app.GetStorage("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", "Dmog", "OwnedMogwaisCount");

                //var result = app.GetStorage("Sudo", "Key");
                //Console.WriteLine($"result: {result}");
                //var resultObject = JObject.Parse(result);
                //Console.WriteLine($"result: {AddressUtils.GetAddrFromPublicKey(new PublicKey() { Bytes = Converters.HexToByteArray(resultObject.Value<string>("result")) })}");             


                var _protocolParams = app.GetProtocolParameters();
                var _serializer = app.Serializer;
                string module = "Dmog";
                string variable = "OwnedMogwaisCount";
                string key;
                key = _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, module, _serializer);
                key += _protocolParams.Metadata.GetPlainStorageKey(_protocolParams.FreeBalanceHasher, variable, _serializer);

                /***
                 * Rätsel für Rene
                 * ---------------
                 * string accountId = "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY";
                 * var addressHexPrefixed = SS58.Encode(accountId);
                 */
                var addressHexPrefixed = "0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d";
                var addressBytes = Converters.HexToByteArray(addressHexPrefixed);
                var addressBytesBlake2Concat = Hash.GetStorageKey(Hasher.BLAKE2_128_CONCAT, addressBytes, addressBytes.Length, _serializer);
                key += Converters.ToHexString(addressBytesBlake2Concat);
                //key += "de1e86a9a8c739864cf3cc5ec2bea59fd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d";

                Console.WriteLine($"#######################################################################################");
                Console.WriteLine($"- CURL - ##############################################################################");
                Console.WriteLine($"#######################################################################################");
                Console.WriteLine("curl -H \"Content-Type: application/json\" -d '{\"id\":1, \"jsonrpc\":\"2.0\", \"method\": \"state_getStorage\", \"params\": [\"0x" + key + "\"]}' http://localhost:9933");
                Console.WriteLine();
                Console.WriteLine($"#######################################################################################");
                Console.WriteLine($"- JSON - ##############################################################################");
                Console.WriteLine($"#######################################################################################");

                JObject query = new JObject { { "method", "state_getStorage" }, { "params", new JArray { $"0x{key}" } } };
                Console.WriteLine(query.ToString());
                JObject response = (app as Application).GetIJsonRpc.Request(query);
                Console.WriteLine(response["result"].ToString());
                Console.WriteLine();

                app.Disconnect();
            }

            Console.ReadKey();
        }

        private static void GetMetaData(IApplication app)
        {
            Console.WriteLine("================== Get Metadata ====================================");
            var result = app.GetMetadata(null);
            Console.WriteLine($"MetaData v{result.Version}");
            foreach (string extrinsicExt in result.GetExtrinsicExtension())
            {
                Console.WriteLine($"- ExtrinsicExtension[{extrinsicExt}]");
            }
            foreach (var module in result.GetModules())
            {
                if (module is null)
                {
                    continue;
                }

                Console.WriteLine($"*** Module {module.GetName()} ***");

                var mod11 = module as ModuleV11;

                if (!(mod11.Storage is null))
                {
                    var storage11 = mod11.Storage as StorageCollectionV11;

                    Console.WriteLine($"Storage Prefix: {storage11.Prefix}");

                    foreach (var item in storage11.Items)
                    {
                        Console.WriteLine($"- Item[{item.Name},type:[{item.Type.Type},{item.Type.Hasher},{item.Type.Key1},{item.Type.Key2},{item.Type.Value},{item.Type.Key2hasher},{item.Type.IsLinked}],mod:{item.Modifier}]");
                    }
                }

                if (!(mod11.GetCalls() is null))
                {
                    foreach (var call in mod11.GetCalls())
                    {
                        var call11 = call as CallV11;

                        string resultArgs = "";
                        if (!(call11.Args is null))
                        {
                            foreach (var call11args in call11.Args)
                            {
                                resultArgs += $"[{call11args.Name}[{call11args.Type}]]";
                            }
                        }
                        Console.WriteLine($"- Call[{call11.GetName()}({resultArgs})]");
                    }
                }

                if (!(mod11.GetConstants() is null))
                {
                    foreach (var constant in mod11.GetConstants())
                    {
                        Console.WriteLine($"- Constant[{constant.GetName()}({constant.GetType()})]");
                    }
                }


            }
        }
    }
}

using Polkadot.Api;
using Polkadot.DataStructs;
using Polkadot.DataStructs.Metadata;
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

                var result = app.GetStorage("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY", "Balances", "Account");
                Console.WriteLine($"result: {result}");

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

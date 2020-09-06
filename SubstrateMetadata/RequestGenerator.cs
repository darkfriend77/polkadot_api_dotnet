using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamJsonRpc.Protocol;

namespace SubstrateMetadata
{

    public class RequestGenerator
    {
        public static string GetStorage(Module module, Item item, string parameter)
        {
            var mBytes = Encoding.ASCII.GetBytes(module.Name);
            var iBytes = Encoding.ASCII.GetBytes(item.Name);

            var keybytes = HashExtension.XXHash128(mBytes).Concat(HashExtension.XXHash128(iBytes)).ToArray();
            var key = BitConverter.ToString(keybytes).Replace("-", "");
            
            switch (item.Type)
            {
                case Storage.Type.Plain:
                    break;
                case Storage.Type.Map:
                    break;
                case Storage.Type.DoubleMap:
                    break;
            }

            return key;

        }
    }
}

using System;
using System.Linq;
using HashDepot;

namespace SubstrateMetadata
{
    public class HashExtension
    {
        public static byte[] XXHash128(byte[] bytes)
        {
            return BitConverter.GetBytes(XXHash.Hash64(bytes, 0))
                .Concat(BitConverter.GetBytes(XXHash.Hash64(bytes, 1))).ToArray();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SubstrateMetadata
{
    public static class Utils
    {
        public static byte[] HexToByteArray(this string hex)
        {
            var span = hex.AsSpan();
            if ((hex[0] == '0') && (hex[1] == 'x'))
            {
                span = span[2..];
            }

            var value = new byte[span.Length / 2];
            for (int i = 0; i < value.Length; i++)
            {
                var substring = span[(i * 2)..(i * 2 + 2)];
                value[i] = byte.Parse(substring.ToString(), NumberStyles.HexNumber);
            }

            return value;
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static byte[] GetPublicKeyFrom(string address)
        {
            int PUBLIC_KEY_LENGTH = 32;

            var pubkByteList = new List<byte>();

            var bs58decoded = SimpleBase.Base58.Bitcoin.Decode(address).ToArray();
            int len = bs58decoded.Length;

            if (len == 35)
            {
                byte[] ssPrefixed = { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
                pubkByteList.AddRange(ssPrefixed);
                pubkByteList.AddRange(bs58decoded.Take(PUBLIC_KEY_LENGTH + 1));

                var blake2bHashed = HashExtension.Blake2(pubkByteList.ToArray(), 512);
                if (bs58decoded[PUBLIC_KEY_LENGTH + 1] != blake2bHashed[0] ||
                    bs58decoded[PUBLIC_KEY_LENGTH + 2] != blake2bHashed[1])
                {
                    throw new ApplicationException("Address checksum is wrong.");
                }

                return bs58decoded.Skip(1).Take(PUBLIC_KEY_LENGTH).ToArray();
            }

            throw new ApplicationException("Address checksum is wrong.");
        }

        public static string GetAddressFrom(byte[] bytes)
        {
            int SR25519_PUBLIC_SIZE = 32;
            int PUBLIC_KEY_LENGTH = 32;

            var plainAddr = Enumerable
                .Repeat((byte)0x2A, 35)
                .ToArray();

            bytes.CopyTo(plainAddr.AsMemory(1));

            var ssPrefixed = new byte[SR25519_PUBLIC_SIZE + 8];
            var ssPrefixed1 = new byte[] { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
            ssPrefixed1.CopyTo(ssPrefixed, 0);
            plainAddr.AsSpan(0, SR25519_PUBLIC_SIZE + 1).CopyTo(ssPrefixed.AsSpan(7));

            var blake2bHashed = HashExtension.Blake2(ssPrefixed, 0, SR25519_PUBLIC_SIZE + 8);
            plainAddr[1 + PUBLIC_KEY_LENGTH] = blake2bHashed[0];
            plainAddr[2 + PUBLIC_KEY_LENGTH] = blake2bHashed[1];

            var addrCh = SimpleBase.Base58.Bitcoin.Encode(plainAddr).ToArray();

            return new string(addrCh);
        }

    }
}

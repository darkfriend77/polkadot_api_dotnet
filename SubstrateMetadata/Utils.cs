using System;
using System.Collections.Generic;
using System.Globalization;
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
    }
}

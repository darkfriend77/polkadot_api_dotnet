using System.Linq;
using System.Text;
using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest
{
    public class HasherTest
    {
        [Fact]
        public void HasherTestBlake2Concat()
        {

            //string accountId = "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY";
            //var accountIdBytes = SimpleBase.Base58.Bitcoin.Decode(accountId).ToArray();
            //int accountIdBytesLen = accountIdBytes.Length;
            //Assert.Equal("", Converters.ToHexString(accountIdBytes));
            var addressHexPrefixed = "0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d";
            var addressBytes = Converters.HexToByteArray(addressHexPrefixed);
            Assert.Equal(addressHexPrefixed, Converters.ToPrefixedHexString(addressBytes), ignoreCase: true);

            var addressBytesBlake2 = Hash.GetStorageKey(Hasher.BLAKE2, addressBytes, addressBytes.Length, null);
            Assert.Equal("0xde1e86a9a8c739864cf3cc5ec2bea59f", Converters.ToPrefixedHexString(addressBytesBlake2), ignoreCase: true);

            var addressBytesBlake2Concat = Hash.GetStorageKey(Hasher.BLAKE2_128_CONCAT, addressBytes, addressBytes.Length, null);

            Assert.Equal("0xde1e86a9a8c739864cf3cc5ec2bea59fd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d", Converters.ToPrefixedHexString(addressBytesBlake2Concat), ignoreCase: true);

        }
    }
}
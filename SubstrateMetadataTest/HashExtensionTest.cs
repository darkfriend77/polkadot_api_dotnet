using System;
using System.Text;
using NUnit.Framework;
using SubstrateMetadata;

namespace SubstrateMetadataTest
{
    public class HashExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void XXHash128()
        {
            var bytes1 = Encoding.ASCII.GetBytes("Sudo");
            var hashBytes1 = HashExtension.XXHash128(bytes1);
            Assert.AreEqual("5C0D1176A568C1F92944340DBFED9E9C", BitConverter.ToString(hashBytes1).Replace("-", ""));

            var bytes2 = Encoding.ASCII.GetBytes("Key");
            var hashBytes2 = HashExtension.XXHash128(bytes2);
            Assert.AreEqual("530EBCA703C85910E7164CB7D1C9E47B", BitConverter.ToString(hashBytes2).Replace("-", ""));
        }
    }
}
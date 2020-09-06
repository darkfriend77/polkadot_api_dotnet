namespace SubstrateMetadata
{
    public class MetaDataV11
    {
        public MetaDataV11(string origin = "unknown")
        {
            this.Origin = origin;
        }
        public string Origin { get; internal set; }
        public string Magic { get; set; }
        public string Version { get; set; }
        public Module[] Modules { get; set; }
        public string[] ExtrinsicExtensions { get; set; }

    }
}
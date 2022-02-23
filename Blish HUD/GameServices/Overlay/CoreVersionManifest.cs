using System.Text.Json.Serialization;
using Version = SemVer.Version;

namespace Blish_HUD.Overlay {
    public struct CoreVersionManifest {
        [JsonPropertyName("url")]
        public string Url { get; set; }


        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }


        [JsonPropertyName("version")]
        [JsonConverter(typeof(Content.Serialization.SemVerConverter))]
        public Version Version { get; set; }

        [JsonPropertyName("is_prerelease")]
        public bool IsPrerelease { get; set; }

        [JsonPropertyName("changelog")]
        public string Changelog { get; set; }

    }
}
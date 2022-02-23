using System.Text.Json.Serialization;

namespace Blish_HUD.Modules.Pkgs {
    public class PkgManifestV1 : PkgManifest {

        public override SupportedModulePkgVersion ManifestVersion => SupportedModulePkgVersion.V1;

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}

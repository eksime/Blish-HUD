using System.Text.Json.Serialization;

namespace Blish_HUD.Modules {

    public class ManifestV1 : Manifest {

        [JsonPropertyName("manifest_version")]
        public override SupportedModuleManifestVersion ManifestVersion => SupportedModuleManifestVersion.V1;

    }
}

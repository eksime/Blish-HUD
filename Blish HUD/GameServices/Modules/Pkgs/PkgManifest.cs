using System.Collections.Generic;
using System.Text.Json.Serialization;
using Blish_HUD.Modules.Serialization;

namespace Blish_HUD.Modules.Pkgs {
    [JsonConverter(typeof(PkgManifestJsonConverter))]
    public abstract class PkgManifest {
        private List<ModuleDependency> _dependencies = new List<ModuleDependency>(0);

        public abstract SupportedModulePkgVersion ManifestVersion { get; }

        // Required attributes
        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonInclude]
        [JsonPropertyName("namespace")]
        public string Namespace { get; private set; }

        [JsonInclude]
        [JsonPropertyName("version")]
        [JsonConverter(typeof(Content.Serialization.SemVerConverter))]
        public SemVer.Version Version { get; private set; }

        [JsonInclude]
        [JsonPropertyName("contributors")]
        public List<ModuleContributor> Contributors { get; private set; }

        [JsonInclude]
        [JsonPropertyName("dependencies")]
        [JsonConverter(typeof(ModuleDependency.VersionDependenciesConverter))]
        public List<ModuleDependency> Dependencies {
            get => _dependencies;
            private set => _dependencies = value ?? new List<ModuleDependency>(0);
        }

        [JsonInclude]
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonInclude]
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

    }
}

﻿using System.Collections.Generic;
using Gw2Sharp.WebApi.V2.Models;
using System.Text.Json.Serialization;
using Blish_HUD.Modules.Serialization;

namespace Blish_HUD.Modules {

    [JsonConverter(typeof(ManifestJsonConverter))]
    public abstract class Manifest {

        [JsonInclude]
        [JsonPropertyName("manifest_version")]
        public abstract SupportedModuleManifestVersion ManifestVersion { get; }

        // Required attribtes
        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonInclude]
        [JsonPropertyName("version")]
        [JsonConverter(typeof(Content.Serialization.SemVerConverter))]
        public SemVer.Version Version { get; private set; }

        [JsonInclude]
        [JsonPropertyName("namespace")]
        public string Namespace { get; private set; }

        [JsonInclude]
        [JsonPropertyName("package")]
        public string Package { get; private set; }

        // Recommended attributes
        [JsonInclude]
        [JsonPropertyName("description")]
        public string Description { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("dependencies")]
        [JsonConverter(typeof(ModuleDependency.VersionDependenciesConverter))]
        public List<ModuleDependency> Dependencies { get; private set; } = new List<ModuleDependency>(0);

        [JsonInclude]
        [JsonPropertyName("url")]
        public string Url { get; private set; } = string.Empty;

        [JsonInclude]
        [JsonPropertyName("author")]
        public ModuleContributor Author { get; private set; }

        [JsonInclude]
        [JsonPropertyName("contributors")]
        public List<ModuleContributor> Contributors { get; private set; } = new List<ModuleContributor>(0);

        // Optional attributes
        [JsonInclude]
        [JsonPropertyName("directories")]
        public List<string> Directories { get; private set; } = new List<string>(0);

        [JsonInclude]
        [JsonPropertyName("enable_without_gw2")]
        public bool EnabledWithoutGW2 { get; private set; } = false;

        [JsonInclude]
        [JsonPropertyName("api_permissions")]
        public Dictionary<TokenPermission, ModuleApiPermissions> ApiPermissions { get; private set; } = new Dictionary<TokenPermission, ModuleApiPermissions>();

        /// <summary>
        /// Gets the detailed name of the module suitable for displaying in logs.
        /// [ModuleName] ([ModuleNamespace] v[ModuleVersion])
        /// </summary>
        public virtual string GetDetailedName() {
            return $"{this.Name} ({this.Namespace}) v{this.Version}";
        }

    }

}
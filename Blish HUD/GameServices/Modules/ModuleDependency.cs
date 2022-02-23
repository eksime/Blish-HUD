using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using Range = SemVer.Range;
using Version = SemVer.Version;
using System.Text.Json;
using Blish_HUD.Content.Serialization;
using System.Linq;

namespace Blish_HUD.Modules {

    public class ModuleDependency {

        private const string BLISHHUD_DEPENDENCY_NAME = "bh.blishhud";

        internal class VersionDependenciesConverter : JsonConverter<List<ModuleDependency>> {
            public override List<ModuleDependency> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
                try {
                    Dictionary<string, string> dict = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options);

                    return dict.Select(kvp => new ModuleDependency() {
                        Namespace = kvp.Key,
                        VersionRange = new Range(kvp.Value, true),
                    }).ToList();
                } catch (Exception ex) {

                    throw ex;
                }
            }

            public override void Write(Utf8JsonWriter writer, List<ModuleDependency> value, JsonSerializerOptions options) {
                Dictionary<string, string> dict = value.ToDictionary(val => val.Namespace, val => val.VersionRange.ToString());
                JsonSerializer.Serialize(writer, dict, options);
            }
        }

        public string Namespace { get; private set; }

        [JsonConverter(typeof(SemVerRangeConverter))]
        public Range VersionRange { get; private set; }

        public bool IsBlishHud => string.Equals(this.Namespace, BLISHHUD_DEPENDENCY_NAME, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Calculates the current details of the dependency.
        /// </summary>
        public ModuleDependencyCheckDetails GetDependencyDetails() {
            // Check against Blish HUD version
            if (this.IsBlishHud) {
                return new ModuleDependencyCheckDetails(this,
                                                        this.VersionRange.IsSatisfied(Program.OverlayVersion.BaseVersion())
                                                        || Program.OverlayVersion.BaseVersion() == new Version(0, 0, 0) // Ensure local builds ignore prerequisite
                                                            ? ModuleDependencyCheckResult.Available
                                                            : ModuleDependencyCheckResult.AvailableWrongVersion);
            }

            // Check for module dependency
            foreach (var module in GameService.Module.Modules) {
                if (string.Equals(this.Namespace, module.Manifest.Namespace, StringComparison.OrdinalIgnoreCase)) {
                    if (this.VersionRange.IsSatisfied(module.Manifest.Version.BaseVersion())) {
                        // Module exists and is a valid version
                        return new ModuleDependencyCheckDetails(this,
                                                                module.Enabled
                                                                    ? ModuleDependencyCheckResult.Available
                                                                    : ModuleDependencyCheckResult.AvailableNotEnabled,
                                                                module);
                    }

                    // Module exists but is the wrong version
                    return new ModuleDependencyCheckDetails(this, ModuleDependencyCheckResult.AvailableWrongVersion, module);
                }
            }

            // No module could be found that matches
            return new ModuleDependencyCheckDetails(this, ModuleDependencyCheckResult.NotFound);
        }

    }

}
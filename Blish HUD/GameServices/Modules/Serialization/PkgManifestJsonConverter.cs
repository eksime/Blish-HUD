using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blish_HUD.Modules.Pkgs;

namespace Blish_HUD.Modules.Serialization {
    internal class PkgManifestJsonConverter : JsonConverter<PkgManifest> {
        public override bool CanConvert(Type typeToConvert) {
            return typeof(PkgManifest).IsAssignableFrom(typeToConvert);
        }

        public override PkgManifest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            JsonElement manifestElement = JsonElement.ParseValue(ref reader);

            JsonElement manifestVersionElement = manifestElement.GetProperty("manifest_version");

            int version = manifestVersionElement.ValueKind switch {
                JsonValueKind.Number => manifestVersionElement.GetInt32(),
                JsonValueKind.String => int.Parse(manifestVersionElement.GetString()),
                _ => throw new JsonException()
            };

            SupportedModulePkgVersion manifestVersion = (SupportedModulePkgVersion)version;

            return manifestVersion switch {
                SupportedModulePkgVersion.V1 => manifestElement.Deserialize<PkgManifestV1>(options)!,
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, PkgManifest value, JsonSerializerOptions options) {
            JsonSerializer.Serialize(value, value.GetType(), options);
        }
    }
}

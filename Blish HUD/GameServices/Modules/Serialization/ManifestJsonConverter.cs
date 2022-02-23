using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blish_HUD.Modules.Serialization {
    internal class ManifestJsonConverter : JsonConverter<Manifest> {
        public override bool CanConvert(Type typeToConvert) {
            return typeof(Manifest).IsAssignableFrom(typeToConvert);
        }

        public override Manifest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            JsonElement manifestElement = JsonElement.ParseValue(ref reader);

            JsonElement manifestVersionElement = manifestElement.GetProperty("manifest_version");

            int version = manifestVersionElement.ValueKind switch {
                JsonValueKind.Number => manifestVersionElement.GetInt32(),
                JsonValueKind.String => int.Parse(manifestVersionElement.GetString()),
                _ => throw new JsonException()
            };

            SupportedModuleManifestVersion manifestVersion = (SupportedModuleManifestVersion)version;

            return manifestVersion switch {
                SupportedModuleManifestVersion.V1 => manifestElement.Deserialize<ManifestV1>(options)!,
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, Manifest value, JsonSerializerOptions options) {
            JsonSerializer.Serialize(value, value.GetType(), options);
        }
    }
}

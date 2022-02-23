using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Range = SemVer.Range;

namespace Blish_HUD.Content.Serialization {
    public class SemVerRangeConverter : JsonConverter<Range> {
        /// <inheritdoc />
        public override Range Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string versionStr = JsonSerializer.Deserialize<string>(ref reader, options);
            return new Range(versionStr, true);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Range value, JsonSerializerOptions options) {
            JsonSerializer.Serialize(writer, value.ToString(), options);
        }
    }
}

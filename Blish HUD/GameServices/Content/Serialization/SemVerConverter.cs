﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Version = SemVer.Version;

namespace Blish_HUD.Content.Serialization {
    public class SemVerConverter : JsonConverter<Version> {
        /// <inheritdoc />
        public override Version Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string versionStr = JsonSerializer.Deserialize<string>(ref reader, options);
            return new Version(versionStr, true);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options) {
            JsonSerializer.Serialize(writer, value.ToString(), options);
        }
    }
}

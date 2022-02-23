using System.Text.Json.Serialization;

namespace Blish_HUD.Modules {

    public class ModuleContributor {

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("username")]
        public string Username { get; private set; }

        [JsonPropertyName("url")]
        public string Url { get; private set; }

    }

}
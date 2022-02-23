using System.Text.Json.Serialization;

namespace Blish_HUD.Modules {

    public class ModuleApiPermissions {

        [JsonPropertyName("optional")]
        public bool Optional { get; private set; }

        [JsonPropertyName("details")]
        public string Details { get; private set; }

    }

}
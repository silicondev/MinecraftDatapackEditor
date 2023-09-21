using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data
{
    public class Pack
    {
        [JsonProperty("pack_format")]
        public int Format { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}

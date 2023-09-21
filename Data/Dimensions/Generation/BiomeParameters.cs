using MinecraftDatapackEditor.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class BiomeParameters
    {
        public double[] temperature { get; set; }
        public double[] humidity { get; set; }
        public double[] continentalness { get; set; }
        public double[] weirdness { get; set; }
        [JsonConverter(typeof(SingleArrayConverter<double>))]
        public double[] depth { get; set; }
        [JsonConverter(typeof(SingleArrayConverter<double>))]
        public double[] offset { get; set; }
    }
}

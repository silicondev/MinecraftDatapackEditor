using MinecraftDatapackEditor.Interfaces;
using MinecraftDatapackEditor.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackEditor.Data.Dimensions.Generation
{
    public class Biome : Tableable
    {
        [JsonProperty("biome")]
        public string Id { get; set; }
        [JsonProperty("parameters")]
        public BiomeParameters Parameters { get; set; }

        protected override DataRow GenerateRow(DataRow row)
        {
            row["Biome Id"] = Id;
            row["Temperature"] =     Parameters.temperature;
            row["Humidity"] =        Parameters.humidity;
            row["Continentalness"] = Parameters.continentalness;
            row["Weirdness"] =       Parameters.weirdness;
            row["Depth"] =           Parameters.depth;
            row["Offset"] =          Parameters.offset;
            return row;
        }

        public override DataColumn[] GetColumns() =>
            new DataColumn[]
            {
                new DataColumn()
                {
                    DataType = typeof(int),
                    ColumnName = "Index",
                    AutoIncrement = true,
                },
                new DataColumn()
                {
                    DataType = typeof(string),
                    ColumnName = "Biome Id"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Temperature"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Humidity"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Continentalness"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Weirdness"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Depth"
                },
                new DataColumn()
                {
                    DataType = typeof(Vector2D),
                    ColumnName = "Offset"
                }
            };
    }
}

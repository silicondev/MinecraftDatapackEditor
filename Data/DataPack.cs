using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MinecraftDatapackEditor.Data
{
    public class DataPack
    {
        public static DataPack CreateNew(string path, DatapackVersion version = DatapackVersion.v120)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var mcmeta = new Pack()
            {
                Format = (int)version,
                Description = ""
            };

            string json = JsonConvert.SerializeObject(new { pack = mcmeta });

            using (var packFile = File.CreateText(Path.Combine(path, "pack.mcmeta")))
                packFile.Write(json);

            return new DataPack(path);
        }

        public static DataPack CreateNew(string path, DataPack preset)
        {
            var datapack = CreateNew(path, (DatapackVersion)preset.Pack.Format);

            // Insert preset data here

            return datapack;
        }

        private string _path;

        public string Name { get; set; }
        public Pack Pack { get; set; }

        public DataPack(string path)
        {
            _path = path;
            Load(path);
        }

        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = _path;


        }

        public void Load(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = _path;

            Name = Path.GetDirectoryName(path) ?? "";

            string mcmetaPath = Path.Combine(path, "pack.mcmeta");
            if (!File.Exists(mcmetaPath))
                throw new Exception("Folder is missing the pack.mcmeta file");

            using (var stream = File.OpenRead(mcmetaPath))
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                var doc = JObject.Parse(json);
                var el = doc.Children().First().Children().First();
                var p = JsonConvert.DeserializeObject<Pack>(el.ToString());
                if (p == null)
                    throw new Exception("Could not deserialize pack.mcmeta");
                Pack = p;
            }


        }
    }

    public enum DatapackVersion
    {
        v113 = 4,
        v115 = 5,
        v1162 = 6,
        v117 = 7,
        v118 = 8,
        v1182 = 9,
        v119 = 10,
        v1194 = 12,
        v120 = 15
    };
}

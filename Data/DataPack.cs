using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MinecraftDatapackEditor.Data.Dimensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace MinecraftDatapackEditor.Data
{
    public class DataPack
    {
        #region CreateNew
        public static DataPack CreateNew(string path, string namesp = "minecraft", DatapackVersion version = DatapackVersion.v120)
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

            var dp = new DataPack(path);
            dp.Namespaces = new Namespace[] { new Namespace(namesp) };

            dp.CreateFolders(true);
            dp.Load();
            return dp;
        }

        public static DataPack CreateNew(string path, DatapackVersion version) =>
            CreateNew(path, version: version);

        public static DataPack CreateNew(string path, DataPack preset)
        {
            var dp = CreateNew(path, (DatapackVersion)preset.Pack.Format);
            dp.Namespaces = preset.Namespaces;
            dp.Save();
            return dp;
        }
        #endregion

        private string _path;

        public string Name { get; set; }
        public Pack Pack { get; set; }
        public Namespace[] Namespaces { get; set; }

        public DataPack(string path)
        {
            _path = path;
        }

        public void Save(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = _path;

            var dirs = Directory.GetDirectories(_path);
            var files = Directory.GetFiles(_path);
            string archiveDir = Path.Combine(_path, "Archive");

            Directory.Delete(archiveDir, true);
            Directory.CreateDirectory(archiveDir);

            foreach (var dir in dirs)
                Directory.Move(dir, Path.Combine(archiveDir, Path.GetDirectoryName(dir)));

            foreach (var file in files)
                File.Move(file, Path.Combine(archiveDir, Path.GetFileName(file)));

            bool successful = false;

            CreateFolders();

            if (successful)
                Directory.Delete(Path.Combine(archiveDir), true);
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

            var namespaceDirs = Directory.GetDirectories(Path.Combine(path, "data"));
            Namespaces = new Namespace[namespaceDirs.Length];

            for (int i = 0; i < namespaceDirs.Length; i++)
            {
                string namespacePath = namespaceDirs[i];
                var ns = new Namespace(Path.GetFileName(namespacePath));

                // Dimension

                var dimensionDirs = Directory.GetFiles(Path.Combine(namespacePath, "dimension"), "*.json");
                var dimList = new List<Dimension>();
                for (int d = 0; d < dimensionDirs.Length; d++)
                {
                    string dimensionPath = dimensionDirs[d];
                    Dimension dim;
                    try
                    {
                        using (var stream = File.OpenRead(dimensionPath))
                        using (var reader = new StreamReader(stream))
                            dim = JsonConvert.DeserializeObject<Dimension>(reader.ReadToEnd());
                        dim.Name = Path.GetFileNameWithoutExtension(dimensionPath);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    if (dim != null)
                        dimList.Add(dim);
                }
                ns.Dimensions = dimList.ToArray();





                Namespaces[i] = ns;
            }
            Console.WriteLine("Load complete.");
        }

        private void CreateFolders(bool createAll = false)
        {
            Directory.CreateDirectory(Path.Combine(_path, "data"));

            foreach (var ns in Namespaces)
            {
                string namesp = ns.Name;

                Directory.CreateDirectory(Path.Combine(_path, "data", namesp));

                if ((ns.Dimensions != null && ns.Dimensions.Length > 0) || createAll)
                    Directory.CreateDirectory(Path.Combine(_path, "data", namesp, "dimension"));
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

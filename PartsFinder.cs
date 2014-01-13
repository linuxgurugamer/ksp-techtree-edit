﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AVTTLoaderStandalone
{
    class PartsFinder
    {

        public List<string> Parts { get; private set; }

        public PartsFinder()
        {
            Parts = new List<string>();
        }

        public List<string> FindParts(string directory)
        {
            Parts = new List<string>();
            var dir = new DirectoryInfo(@directory);
            foreach (var file in dir.GetFiles("*.cfg", SearchOption.AllDirectories))
            {
                var parts = GetParts(file);
                if (parts != null && parts.Count >= 1)
                {
                    foreach (var p in parts)
                    {
                        Parts.Add(p);
                    }
                };
            }
            return Parts;
        }

        public string FindPartsString(string directory)
        {
            return FindParts(directory).Aggregate("", (current, part) => current + (part + ", "));
        } 

        static List<string> GetParts(FileSystemInfo file)
        {
            var parts = new List<string>();
            var reader = new StreamReader(file.FullName);
            string line;
            var isPart = false;
            var depth = 0;

            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();

                if (line == "PART") isPart = true;

                if (!isPart) continue;

                if (line == "{") depth++;

                if (line == "}") depth--;

                if (depth != 1) continue;

                if (!line.StartsWith("name = ")) continue;

                var partName = line.Remove(0, line.LastIndexOf(char.Parse("=")) + 1).Trim();
                if (partName.Length >= 1) parts.Add(partName);
            }
            return parts;
        }
    }
}

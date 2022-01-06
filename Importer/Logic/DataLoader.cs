using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Importer.Logic
{
    public class DataLoader
    {
        public async Task<List<Mod>> GetFileAsync(string path)
        {
            string text = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<Mod>>(text);
        }
    }
}

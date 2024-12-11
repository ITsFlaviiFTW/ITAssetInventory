using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ITAssetInventory.Models;

namespace ITAssetInventory.Data
{
    public class AssetRepository
    {
        private readonly string _dataFilePath;

        public AssetRepository(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        public List<Asset> LoadAssets()
        {
            if (!File.Exists(_dataFilePath))
                return new List<Asset>(); // Return empty list if no file found

            string json = File.ReadAllText(_dataFilePath);
            var assets = JsonConvert.DeserializeObject<List<Asset>>(json);
            return assets ?? new List<Asset>();
        }

        public void SaveAssets(List<Asset> assets)
        {
            string json = JsonConvert.SerializeObject(assets, Formatting.Indented);
            File.WriteAllText(_dataFilePath, json);
        }
    }
}

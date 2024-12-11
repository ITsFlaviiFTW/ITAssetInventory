namespace ITAssetInventory.Models
{
    public class Asset
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Asset() { }

        public Asset(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}

using SamStoreWPFJson_BOs;
using System.Text.Json;

namespace SamStoreWPFJson_DAOs
{
    public class SamProductDAO
    {
        private readonly string _jsonFilePath;
        private List<SamProduct> _products;

        public SamProductDAO()
        {
            _jsonFilePath = @"H:\Class\P_R_N\SamStoreWPFJson-20250725T060713Z-1-001\SamStoreWPFJson\SamStoreWPFJson_JSONs\SamProducts.json";
            LoadProductsFromJson();
        }

        private void LoadProductsFromJson()
        {
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        _products = new List<SamProduct>();
                    }
                    else
                    {
                        _products = JsonSerializer.Deserialize<List<SamProduct>>(json) ?? new List<SamProduct>();
                    }
                }
                else
                {
                    _products = new List<SamProduct>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading SamProducts.json: {ex.Message}", ex);
            }
        }

        public List<SamProduct> GetAllProducts()
        {
            LoadProductsFromJson(); // Reload to ensure latest data
            return _products.ToList();
        }

        public SamProduct? GetProductById(int id)
        {
            LoadProductsFromJson();
            return _products.FirstOrDefault(p => p.ProductID == id);
        }
    }
}

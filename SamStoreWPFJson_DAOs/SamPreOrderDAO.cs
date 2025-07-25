using SamStoreWPFJson_BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamStoreWPFJson_DAOs
{
    public class SamPreOrderDAO
    {
        private readonly string _jsonFilePath;
        private List<SamPreOrder> _preOrders;
        private readonly SamProductDAO _productDAO;

        public SamPreOrderDAO()
        {
            // Use the direct path to the JSON file in the project directory
            _jsonFilePath = @"H:\Class\P_R_N\SamStoreWPFJson-20250725T060713Z-1-001\SamStoreWPFJson\SamStoreWPFJson_JSONs\SamPreOrders.json";
            _productDAO = new SamProductDAO();
            LoadPreOrdersFromJson();
        }

        private bool LoadPreOrdersFromJson()
        {
            try
            {
                if (File.Exists(_jsonFilePath))
                {
                    string json = File.ReadAllText(_jsonFilePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        _preOrders = new List<SamPreOrder>();
                    }
                    else
                    {
                        _preOrders = JsonSerializer.Deserialize<List<SamPreOrder>>(json) ?? new List<SamPreOrder>();
                        
                        // Load product information for each pre-order
                        var products = _productDAO.GetAllProducts();
                        foreach (var preOrder in _preOrders)
                        {
                            preOrder.Product = products.FirstOrDefault(p => p.ProductID == preOrder.ProductID);
                        }
                    }
                }
                else
                {
                    _preOrders = new List<SamPreOrder>();
                }

                // Always save after loading/initializing to ensure file exists with proper content
                return SavePreOrdersToJson();
            }
            catch (Exception)
            {
                // Initialize empty list if loading fails
                _preOrders = new List<SamPreOrder>();
                return false;
            }
        }

        private bool SavePreOrdersToJson()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(_preOrders, options);
                File.WriteAllText(_jsonFilePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SamPreOrder> GetAllPreOrders()
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                // Return empty list instead of throwing exception
                return _preOrders?.ToList() ?? new List<SamPreOrder>();
            }
            catch (Exception)
            {
                return new List<SamPreOrder>();
            }
        }

        public SamPreOrder? GetPreOrderById(int id)
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                // Return null if not found instead of throwing exception
                return _preOrders?.FirstOrDefault(p => p.PreOrderID == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SamPreOrder> SearchPreOrder(string searchText)
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                if (_preOrders == null || _preOrders.Count == 0)
                {
                    return new List<SamPreOrder>();
                }

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return _preOrders.ToList();
                }

                return _preOrders
                    .Where(p => p.PreOrderNo.ToLower().Contains(searchText.ToLower()) ||
                                (p.CustomerPhone != null && p.CustomerPhone.ToLower().Contains(searchText.ToLower())))
                    .ToList();
            }
            catch (Exception)
            {
                return new List<SamPreOrder>();
            }
        }

        public SamPreOrder? AddNewPreOrder(SamPreOrder preOrder)
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                // Check for duplicates but don't throw exception
                if (_preOrders.Any(p => p.PreOrderID == preOrder.PreOrderID && preOrder.PreOrderID != 0))
                {
                    return null; // ID already exists
                }

                if (_preOrders.Any(p => p.PreOrderNo == preOrder.PreOrderNo))
                {
                    return null; // PreOrderNo already exists
                }

                // Generate new ID if not provided
                if (preOrder.PreOrderID == 0)
                {
                    preOrder.PreOrderID = _preOrders.Count > 0 ? _preOrders.Max(p => p.PreOrderID) + 1 : 1;
                }

                _preOrders.Add(preOrder);
                bool saved = SavePreOrdersToJson();
                
                return saved ? preOrder : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeletePreOrder(int preOrderID)
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                var preOrder = _preOrders.FirstOrDefault(p => p.PreOrderID == preOrderID);
                if (preOrder != null)
                {
                    _preOrders.Remove(preOrder);
                    return SavePreOrdersToJson();
                }

                return false; // Pre-order not found
            }
            catch (Exception)
            {
                return false;
            }
        }

        public SamPreOrder? UpdatePreOrder(SamPreOrder preOrder)
        {
            try
            {
                // Reload pre-orders to ensure we have the latest data
                LoadPreOrdersFromJson();

                var existingPreOrder = _preOrders.FirstOrDefault(p => p.PreOrderID == preOrder.PreOrderID);
                if (existingPreOrder != null)
                {
                    // Check for duplicate PreOrderNo, excluding the current pre-order
                    if (_preOrders.Any(p => p.PreOrderNo == preOrder.PreOrderNo && p.PreOrderID != preOrder.PreOrderID))
                    {
                        return null; // PreOrderNo already in use
                    }

                    existingPreOrder.PreOrderNo = preOrder.PreOrderNo;
                    existingPreOrder.DepositAmount = preOrder.DepositAmount;
                    existingPreOrder.TotalAmount = preOrder.TotalAmount;
                    existingPreOrder.CustomerName = preOrder.CustomerName;
                    existingPreOrder.CustomerAddress = preOrder.CustomerAddress;
                    existingPreOrder.CustomerPhone = preOrder.CustomerPhone;
                    existingPreOrder.Status = preOrder.Status;
                    existingPreOrder.CreatedAt = preOrder.CreatedAt;
                    existingPreOrder.UpdatedAt = preOrder.UpdatedAt;
                    existingPreOrder.ProductID = preOrder.ProductID;

                    bool saved = SavePreOrdersToJson();
                    return saved ? existingPreOrder : null;
                }

                return null; // Pre-order not found
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

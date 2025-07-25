using SamStoreWPFJson_BOs;
using SamStoreWPFJson_Repositories.Implements;
using SamStoreWPFJson_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SamStoreWPFJson
{
    /// <summary>
    /// Interaction logic for OrderManagementWindow.xaml
    /// </summary>
    public partial class OrderManagementWindow : Window
    {
        private readonly ISamPreOrderRepo _preOrderRepo;
        private readonly ISamProductRepo _productRepo;

        public OrderManagementWindow(Member member)
        {
            InitializeComponent();

            _preOrderRepo = new SamPreOrderRepo();
            _productRepo = new SamProductRepo();

            var role = member.RoleID;
            switch (role)
            {
                case 1:
                    break;
                case 2:
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get all pre-orders and project to anonymous type for DataGrid display
            this.dtgPreOrder.ItemsSource = _preOrderRepo.GetAllPreOrders().Select(p => new
            {
                p.PreOrderID,
                p.PreOrderNo,
                ProductName = p.Product != null ? p.Product.ProductName : "Unknown",  // Handle null Product
                p.DepositAmount,
                p.TotalAmount,
                p.CustomerName,
                p.CustomerAddress, 
                p.CustomerPhone,
                p.Status,
                p.CreatedAt,
                p.UpdatedAt
            }).ToList();

            // Set up product combo box
            this.cbbProduct.ItemsSource = _productRepo.GetAllProducts();
            this.cbbProduct.DisplayMemberPath = "ProductName";  // Show product name in dropdown
            this.cbbProduct.SelectedValuePath = "ProductID";    // Use product ID as value
        }

        private void dtgPreOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgPreOrder.SelectedItem != null)
            {
                dynamic row = dtgPreOrder.SelectedItem;
                int preOrderId = row.PreOrderID;

                SamPreOrder? preOrder = _preOrderRepo.GetPreOrderById(preOrderId);
                if (preOrder != null)
                {
                    txtPreOrderID.Text = preOrder.PreOrderID.ToString();
                    txtPreOrderNo.Text = preOrder.PreOrderNo;
                    txtDepositAmount.Text = preOrder.DepositAmount.ToString();
                    txtTotalAmount.Text = preOrder.TotalAmount.ToString();
                    txtCustomerName.Text = preOrder.CustomerName;
                    txtCustomerAddress.Text = preOrder.CustomerAddress;
                    txtCustomerPhone.Text = preOrder.CustomerPhone;
                    txtStatus.Text = preOrder.Status;
                    dpCreatedAt.Text = preOrder.CreatedAt.ToString();
                    dpUpdatedAt.Text = preOrder.UpdatedAt.ToString();
                    cbbProduct.SelectedValue = preOrder.ProductID;
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private bool ValidateNull()
        {
            if (string.IsNullOrEmpty(txtPreOrderNo.Text) ||
                string.IsNullOrEmpty(txtDepositAmount.Text) ||
                string.IsNullOrEmpty(txtTotalAmount.Text) ||
                string.IsNullOrEmpty(txtCustomerName.Text) ||
                string.IsNullOrEmpty(txtCustomerAddress.Text) ||
                string.IsNullOrEmpty(txtCustomerPhone.Text) ||
                string.IsNullOrEmpty(dpCreatedAt.Text) ||
                string.IsNullOrEmpty(dpUpdatedAt.Text) ||
                string.IsNullOrEmpty(txtStatus.Text) ||
                string.IsNullOrEmpty(cbbProduct.SelectedValue.ToString()))
            {
                MessageBox.Show("All fields are required");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateDate(out DateOnly? createdAt, out DateOnly? updatedAt)
        {
            createdAt = null;
            updatedAt = null;
            bool createEmpty = string.IsNullOrEmpty(dpCreatedAt.Text);
            bool updateEmpty = string.IsNullOrEmpty(dpUpdatedAt.Text);
            if (createEmpty || updateEmpty)
            {
                MessageBox.Show("CreatedAt and UpdatedAt cannot be empty");
                return false;
            }
            try
            {
                createdAt = DateOnly.Parse(dpCreatedAt.Text);
                updatedAt = DateOnly.Parse(dpUpdatedAt.Text);

                bool isTrue = updatedAt >= createdAt && createdAt >= DateOnly.FromDateTime(DateTime.Now);
                if (!isTrue)
                {
                    MessageBox.Show("UpdatedAt >= CreatedAt >= CurrentDate");
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid date format. Please enter valid dates or leave empty.");
                return false;
            }
        }

        private bool ValidateCustomerName(string name)
        {
            if (!(5 < name.Trim().Length || name.Trim().Length > 50))
            {
                MessageBox.Show("CustomerName is in the range of 5 – 50 characters");
                return false;
            }

            string[] words = name.Split(' ');
            foreach (string word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    continue;
                }

                char firstChar = word[0];

                if (!((firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= '1' && firstChar <= '9')))
                {
                    MessageBox.Show("Each word of the CustomerName must begin with a capital letter or a digit (1-9).");
                    return false;
                }
            }

            foreach (char c in name)
            {
                if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == ' '))
                {
                    MessageBox.Show("CustomerName is not allowed with special characters.");
                    return false;
                }
            }
            return true;
        }

        private void ValidatePreOrderNo()
        {
            if (!string.IsNullOrEmpty(txtPreOrderNo.Text))
            {
                var existingPreOrders = _preOrderRepo.GetAllPreOrders();
                
                // For Add operation (when PreOrderID is empty or 0)
                bool isAddOperation = string.IsNullOrEmpty(txtPreOrderID.Text) || txtPreOrderID.Text == "0";
                
                if (isAddOperation)
                {
                    // Check if PreOrderNo already exists
                    if (existingPreOrders.Any(p => p.PreOrderNo == txtPreOrderNo.Text))
                    {
                        MessageBox.Show($"Warning: PreOrderNo '{txtPreOrderNo.Text}' already exists. The system will handle this appropriately.", "Duplicate PreOrderNo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    // For Update operation, check if PreOrderNo exists for other records
                    if (int.TryParse(txtPreOrderID.Text, out int currentPreOrderId))
                    {
                        if (existingPreOrders.Any(p => p.PreOrderNo == txtPreOrderNo.Text && p.PreOrderID != currentPreOrderId))
                        {
                            MessageBox.Show($"Warning: PreOrderNo '{txtPreOrderNo.Text}' is already used by another pre-order. The system will handle this appropriately.", "Duplicate PreOrderNo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }


        private void ValidatePreOrderID()
        {
            if (!string.IsNullOrEmpty(txtPreOrderID.Text))
            {
                if (!int.TryParse(txtPreOrderID.Text, out int preOrderId))
                {
                    MessageBox.Show("PreOrderID must be a valid number.");
                }
                else if (preOrderId <= 0)
                {
                    MessageBox.Show("PreOrderID must be a positive number.");
                }
            }
        }
        private void LoadData(string text)
        {
            dtgPreOrder.ItemsSource = _preOrderRepo.SearchPreOrder(text).Select(p => new
            {
                p.PreOrderID,
                p.PreOrderNo,
                ProductName = p.Product.ProductName,
                p.DepositAmount,
                p.TotalAmount,
                p.CustomerName,
                p.CustomerAddress,
                p.CustomerPhone,
                p.Status,
                p.CreatedAt,
                p.UpdatedAt,
            }).ToList();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Run non-blocking validations first (show messages but don't stop)
            ValidatePreOrderID();
            ValidatePreOrderNo();

            bool vNull = ValidateNull();
            bool vDate = ValidateDate(out DateOnly? createdAt, out DateOnly? updatedAt);
            bool vName = ValidateCustomerName(txtCustomerName.Text);

            if (vNull && vDate && vName)
            {
                SamPreOrder preOrder = new SamPreOrder
                {
                    PreOrderID = txtPreOrderID.Text == "" ? 0 : int.Parse(txtPreOrderID.Text),
                    PreOrderNo = txtPreOrderNo.Text,
                    DepositAmount = decimal.Parse(txtDepositAmount.Text),
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    CustomerName = txtCustomerName.Text,
                    CustomerAddress = txtCustomerAddress.Text,
                    CustomerPhone = txtCustomerPhone.Text,
                    Status = txtStatus.Text,
                    CreatedAt = createdAt.Value,
                    UpdatedAt = updatedAt.Value,
                    ProductID = int.Parse(cbbProduct.SelectedValue.ToString())
                };

                var result = _preOrderRepo.AddNewPreOrder(preOrder);
                if (result != null)
                {
                    MessageBox.Show("Add new pre order success");
                    LoadData(result.PreOrderNo);
                }
                else
                {
                    MessageBox.Show("Failed to add new pre order. Please check for duplicate IDs or PreOrderNo.");
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int preOrderID = int.Parse(txtPreOrderID.Text);
            if (_preOrderRepo.DeletePreOrder(preOrderID))
            {
                MessageBox.Show("Success in delete a pre order");
            }
            else
            {
                MessageBox.Show("Fail to delete a pre order");
            }
            txtPreOrderID.Clear();
            txtPreOrderNo.Clear();
            txtDepositAmount.Clear();
            txtTotalAmount.Clear();
            txtCustomerName.Clear();
            txtCustomerAddress.Clear();
            txtCustomerPhone.Clear();
            txtStatus.Clear();
            dpCreatedAt.Text = null;
            dpUpdatedAt.Text = null;
            cbbProduct.SelectedValue = null;
            LoadData("");
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Run non-blocking validations first (show messages but don't stop)
            ValidatePreOrderID();
            ValidatePreOrderNo();

            bool vNull = ValidateNull();
            bool vDate = ValidateDate(out DateOnly? createdAt, out DateOnly? updatedAt);
            bool vName = ValidateCustomerName(txtCustomerName.Text);

            if (vNull && vDate && vName)
            {
                SamPreOrder preOrder = new SamPreOrder
                {
                    PreOrderID = int.Parse(txtPreOrderID.Text),
                    PreOrderNo = txtPreOrderNo.Text,
                    DepositAmount = decimal.Parse(txtDepositAmount.Text),
                    TotalAmount = decimal.Parse(txtTotalAmount.Text),
                    CustomerName = txtCustomerName.Text,
                    CustomerAddress = txtCustomerAddress.Text,
                    CustomerPhone = txtCustomerPhone.Text,
                    Status = txtStatus.Text,
                    CreatedAt = createdAt.Value,
                    UpdatedAt = updatedAt.Value,
                    ProductID = int.Parse(cbbProduct.SelectedValue.ToString())
                };

                var updatedPreOrder = _preOrderRepo.UpdatePreOrder(preOrder);
                if (updatedPreOrder != null)
                {
                    MessageBox.Show("Success in update pre order");
                    LoadData(updatedPreOrder.PreOrderNo);
                }
                else
                {
                    MessageBox.Show("Failed to update pre order. Please check for duplicate PreOrderNo or other issues.");
                }
            }
        }
    }
}

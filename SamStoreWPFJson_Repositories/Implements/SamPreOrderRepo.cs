using SamStoreWPFJson_BOs;
using SamStoreWPFJson_DAOs;
using SamStoreWPFJson_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_Repositories.Implements
{
    public class SamPreOrderRepo : ISamPreOrderRepo
    {
        private readonly SamPreOrderDAO _preOrderDAO;

        public SamPreOrderRepo()
        {
            _preOrderDAO = new SamPreOrderDAO();
        }

        public SamPreOrder AddNewPreOrder(SamPreOrder preOrder)
        {
            return _preOrderDAO.AddNewPreOrder(preOrder);
        }

        public bool DeletePreOrder(int preOrderID)
        {
            return _preOrderDAO.DeletePreOrder(preOrderID);
        }

        public List<SamPreOrder> GetAllPreOrders()
        {
            return _preOrderDAO.GetAllPreOrders();
        }

        public SamPreOrder? GetPreOrderById(int id)
        {
            return _preOrderDAO.GetPreOrderById(id);
        }

        public List<SamPreOrder> SearchPreOrder(string searchText)
        {
            return _preOrderDAO.SearchPreOrder(searchText);
        }

        public SamPreOrder UpdatePreOrder(SamPreOrder preOrder)
        {
            return _preOrderDAO.UpdatePreOrder(preOrder);
        }
    }
}

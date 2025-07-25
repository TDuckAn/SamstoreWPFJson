using SamStoreWPFJson_BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_Repositories.Interfaces
{
    public interface ISamPreOrderRepo
    {
        List<SamPreOrder> GetAllPreOrders();
        List<SamPreOrder> SearchPreOrder(string searchText);
        SamPreOrder AddNewPreOrder(SamPreOrder preOrder);
        bool DeletePreOrder(int preOrderID);
        SamPreOrder UpdatePreOrder(SamPreOrder preOrder);
        SamPreOrder? GetPreOrderById(int id);
    }
}

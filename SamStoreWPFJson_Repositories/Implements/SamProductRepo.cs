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
    public class SamProductRepo : ISamProductRepo
    {
        private readonly SamProductDAO _productDAO;

        public SamProductRepo()
        {
            _productDAO = new SamProductDAO();
        }

        public List<SamProduct> GetAllProducts()
        {
            return _productDAO.GetAllProducts();
        }
    }
}

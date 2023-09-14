using ShopNet.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.BLL.Services.Abstract
{
    public class BaseService
    {
        private protected StoreContext _context;
        public BaseService(StoreContext context)
        {
            _context = context;
        }
    }
}

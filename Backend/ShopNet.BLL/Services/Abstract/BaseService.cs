using ShopNet.DAL.Data;

namespace ShopNet.BLL.Services.Abstract
{
    public class BaseService
    {
        protected StoreContext _context;

        public BaseService(StoreContext context)
        {
            _context = context;
        }
    }
}
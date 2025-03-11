using iMARSARLIMS.Interface.Store;
using iMARSARLIMS.Model.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.StoreController
{
    [Route("api/[controller]")]
    [ApiController]
    public class indentDetailController : BaseController<indentDetail>
    {
        private readonly ContextClass db;
        private readonly IItemMasterStoreServices _itemMasterStoreServices;

        public indentDetailController(ContextClass context, ILogger<BaseController<indentDetail>> logger, IItemMasterStoreServices ItemMasterStoreServices) : base(context, logger)
        {
            db = context;
            this._itemMasterStoreServices = ItemMasterStoreServices;
        }
        protected override IQueryable<indentDetail> DbSet => db.indentDetail.AsNoTracking().OrderBy(o => o.id);

    }
}

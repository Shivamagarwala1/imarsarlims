using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class labDepartmentController : BaseController<labDepartment>
    {
        private readonly ContextClass db;
        private readonly IlabDepartmentServices _labDepartmentServices;

        public labDepartmentController(ContextClass context, ILogger<BaseController<labDepartment>> logger, IlabDepartmentServices labDepartmentServices) : base(context, logger)
        {
            db = context;
            this._labDepartmentServices = labDepartmentServices;
        }
        protected override IQueryable<labDepartment> DbSet => db.labDepartment.AsNoTracking().OrderBy(o => o.id);

        [HttpPost("SaveUpdateLabDepartment")]
        public async Task<ServiceStatusResponseModel> SaveUpdateLabDepartment(labDepartment Department)
        {
            try
            {
                var result = await _labDepartmentServices.SaveUpdateLabDepartment(Department);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("UpdateLabDepartmentStatus")]
        public async Task<ServiceStatusResponseModel> UpdateLabDepartmentStatus(int id,byte status, int Userid)
        {
            try
            {
                var result = await _labDepartmentServices.UpdateLabDepartmentStatus(id,status,Userid);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        [HttpPost("UpdateDepartmentOrder")]
        public async Task<ServiceStatusResponseModel> UpdateDepartmentOrder(List<DepartmentOrderModel> DepartmentOrder,string type)
        {
            try
            {
                var result = await _labDepartmentServices.UpdateDepartmentOrder(DepartmentOrder,type);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}

using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers.MasterController
{
    [Route("api/[controller]")]
    [ApiController]
    public class machineObservationMappingController : BaseController<machineObservationMapping>
    {
        private readonly ContextClass db;
        private readonly ImachineMasterServices _machineMasterServices;

        public machineObservationMappingController(ContextClass context, ILogger<BaseController<machineObservationMapping>> logger, ImachineMasterServices machineMasterServices) : base(context, logger)
        {
            db = context;
            this._machineMasterServices = machineMasterServices;
        }
        protected override IQueryable<machineObservationMapping> DbSet => db.machineObservationMapping.AsNoTracking().OrderBy(o => o.id);
        [HttpPost("SaveUpdateMapping")]
        public async Task<ServiceStatusResponseModel> SaveUpdateMapping(List<machineObservationMapping> MachineMapping)
        {
            try
            {
                var result = await _machineMasterServices.SaveUpdateMapping(MachineMapping);
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }

        [HttpGet("GetMachineMapping")]
        public async Task<ServiceStatusResponseModel> GetMachineMapping()
        {
            try
            {
                var result= await (from mom in db.machineObservationMapping
                             join mm in db.machineMaster on mom.machineId equals mm.id
                             join ob in db.itemObservationMaster on mom.labTestID equals ob.id
                             where mom.isActive==1
                                   select new
                             {
                                 mom.id,
                                 mm.machineName,
                                 TestName=ob.labObservationName,
                                 mom.suffix,
                                 mom.roundUp,
                                 mom.assay,
                                 mom.isOrderable,
                                 mom.isActive,
                                 mom.multiplication
                             }).ToListAsync();
                var resultGroupBy = result
    .GroupBy(r => r.assay)
    .Select(g => new
    {
        Assay = g.Key, // This is the group key (i.e., 'assay')
        Id = g.FirstOrDefault()?.id, // Get the first item's id from the group
        MachineName = g.FirstOrDefault()?.machineName, // Get the first machineName
        TestName = string.Join(", ", g.Select(r => r.TestName)), // Join TestName values
        Suffix = g.FirstOrDefault()?.suffix,
        RoundUp = g.FirstOrDefault()?.roundUp,
        IsOrderable = g.FirstOrDefault()?.isOrderable,
        IsActive = g.FirstOrDefault()?.isActive,
        Multiplication = g.FirstOrDefault()?.multiplication
    }).ToList();

                return new ServiceStatusResponseModel
                {
                    Success = true,
                    Data = resultGroupBy
                };
            }
            catch (Exception ex)
            {
                return new ServiceStatusResponseModel
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? "An error occurred."
                };
            }
        }
    }
}

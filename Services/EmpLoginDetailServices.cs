using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class EmpLoginDetailServices : IEmpLoginDetailServices
    {
        private readonly ContextClass db;
        private readonly ILogger<BaseController<empMaster>> logger;

        public EmpLoginDetailServices(ContextClass context, ILogger<BaseController<empMaster>> logger)
        {
            db = context;
            this.logger = logger;
        }

        public async Task<ServiceStatusResponseModel> SaveLoginDetails(empLoginDetails empLoginDetail)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    if (empLoginDetail.id == 0)
                    {
                       var  employeeLoginData = CreateEmployeeLogin(empLoginDetail);
                        await db.empLoginDetails.AddAsync(employeeLoginData);
                    }
                    else
                    {
                        // Handle updating logic here if required
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var result = db.empMaster.ToList();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Data = result
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    logger.LogError(ex, "Error while saving login details.");
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.InnerException?.Message ?? "An error occurred."
                    };
                }
            }
        }
        private empLoginDetails CreateEmployeeLogin(empLoginDetails loginDetails)
        {
            return new empLoginDetails
            {
                id = loginDetails.id,
                empId = loginDetails.empId,
                centreId = loginDetails.centreId,
                ipAddress = loginDetails.ipAddress,
                macAddress = loginDetails.macAddress,
                browserName = loginDetails.browserName,
                hostName = loginDetails.hostName,
                userName = loginDetails.userName,
                empName = loginDetails.empName,
                roleId = loginDetails.roleId,
                logindatetime = loginDetails.logindatetime,
                creadteddate = loginDetails.creadteddate
            };
        }


    }
}

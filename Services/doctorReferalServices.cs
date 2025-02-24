using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.VisualBasic;

namespace iMARSARLIMS.Services
{
    public class doctorReferalServices : IdoctorReferalServices
    {
        private readonly ContextClass db;
        public doctorReferalServices(ContextClass context, ILogger<BaseController<doctorReferalMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IdoctorReferalServices.SaveUpdateReferDoctor(doctorReferalMaster refDoc)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var msg = "";
                    if(refDoc.doctorId== 0)
                    {
                        db.doctorReferalMaster.Add(refDoc);
                        msg = "Saved Successful"; 
                    }
                    else
                    {
                        db.doctorReferalMaster.Update(refDoc);
                        msg = "Updated Successful";
                    }
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = msg
                    };
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
}

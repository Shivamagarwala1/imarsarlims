using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class organismAntibioticTagMasterServices : IorganismAntibioticTagMasterServices
    {
        private readonly ContextClass db;

        public organismAntibioticTagMasterServices(ContextClass context, ILogger<BaseController<organismAntibioticTagMaster>> logger)
        {
            db = context;
        }
        async Task<ServiceStatusResponseModel> IorganismAntibioticTagMasterServices.OrganismAntibioticeMapping(List<organismAntibioticTagMaster> organismAntibioticTag)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                if(organismAntibioticTag == null)
                {
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = "Data not available"
                    };
                }
                try
                {
                    var organismid = organismAntibioticTag.Select(x => x.organismId).Distinct();
                    var oldOrganismAntibioticMpping = db.organismAntibioticTagMaster.Where(o => organismid.Contains(o.organismId)).ToList();
                    db.organismAntibioticTagMaster.RemoveRange(oldOrganismAntibioticMpping);
                    await db.SaveChangesAsync();
                    var OrganismAntibioticMpping = organismAntibioticTag.Select(organismAntibioticTagData).ToList();
                    db.organismAntibioticTagMaster.AddRange(OrganismAntibioticMpping);
                    await db.SaveChangesAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = true,
                        Message = "Saved SuccessFul"
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

        private organismAntibioticTagMaster organismAntibioticTagData(organismAntibioticTagMaster organismAntibioticTag)
        {
            return new organismAntibioticTagMaster
            {
                id = organismAntibioticTag.id,
                organismId = organismAntibioticTag.organismId,
                antibiticId = organismAntibioticTag.antibiticId,
                centreId = organismAntibioticTag.centreId,
                createdById = organismAntibioticTag.createdById,
                createdDateTime  = organismAntibioticTag.createdDateTime
            };
        }

    }
}

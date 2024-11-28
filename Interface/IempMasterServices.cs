using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IempMasterServices
    {
        Task<ActionResult<List<LoginResponseModel>>> EmpLogin(LoginRequestModel loginRequestModel);
        Task<ActionResult<ServiceStatusResponseModel>> SaveEmployee(empMaster empmaster);
        Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document);
    }
}

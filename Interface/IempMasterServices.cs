using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface IempMasterServices
    {
        Task<ServiceStatusResponseModel> EmpLogin(LoginRequestModel loginRequestModel);
        Task<ActionResult<ServiceStatusResponseModel>> SaveEmployee(empMaster empmaster);
        Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document);
        Task<ServiceStatusResponseModel> DownloadImage(int emplpyeeid);
        Task<ServiceStatusResponseModel> EmployeeWiseCentre(int EmplyeeId);
        Task<ServiceStatusResponseModel> EmployeeWiseRole(int EmplyeeId);
        Task<ServiceStatusResponseModel> EmployeeWiseMenu(string EmplyeeId, string RoleId, string CentreId, int MenuType);
        Task<ServiceStatusResponseModel> GetAllMenu();
        Task<ServiceStatusResponseModel> forgetPassword(string Username);
        Task<ServiceStatusResponseModel> UpdatePassword(int Employeeid, string Password);
        Task<ServiceStatusResponseModel> UpdateEmployeeStatus(int EmplyeeId, byte status, int UserId);


    }
}

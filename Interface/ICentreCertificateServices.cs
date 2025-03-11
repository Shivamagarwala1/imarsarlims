using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Interface
{
    public interface ICentreCertificateServices
    {
        Task<ServiceStatusResponseModel> GenerateCertificate(CentreCertificate Certificate);
        Task<ServiceStatusResponseModel> UploadCertificate(int CentreId,string Certificate);
        Task<ServiceStatusResponseModel> UploadAggreement(int CentreId,string AggrimenstDocumnet);
        Task<ServiceStatusResponseModel> UploadDocument(IFormFile Document);
        Task<ServiceStatusResponseModel> GetAggreement(int centreId);
        byte[] DownloadCertificate(int Id);
    }
}

﻿using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IcentreMasterServices
    {
        Task<ServiceStatusResponseModel> SaveCentreDetail(centreMaster centremaster);
        Task<ServiceStatusResponseModel> UpdateCentreStatus(int CentreId, bool status, int UserId);
        Task<ServiceStatusResponseModel> GetParentCentre();
        Task<ServiceStatusResponseModel> GetRateType();
        Task<ServiceStatusResponseModel> GetCentreData(int centreId);
    }
}

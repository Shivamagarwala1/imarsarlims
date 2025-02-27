﻿using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IPatientReportServices
    {
        byte[] GetPatientReportType1(string TestId);
        byte[] GetPatientReportType2(string TestId);
        byte[] GetPatientReportType3(string TestId);
        Task<ServiceStatusResponseModel> ReportHoldUnHold(string TestId, int isHold, int holdBy, string holdReason);
    }
}

namespace iMARSARLIMS.Interface
{
    public interface IPatientReportServices
    {
        byte[] GetPatientReportType1(string TestId);
        byte[] GetPatientReportType2(string TestId);
    }
}

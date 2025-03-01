namespace iMARSARLIMS.Interface
{
    public interface Ihistoreportservices
    {
        byte[] GetHistoReport(string testId);
        byte[] GetMicroReport(string testId);
    }
}

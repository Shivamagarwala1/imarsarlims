using iMARSARLIMS.Model.Transaction;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface Itnx_testcommentServices
    {
        Task<ServiceStatusResponseModel> SaveTestComment(tnx_testcomment CommentData);
    }
}

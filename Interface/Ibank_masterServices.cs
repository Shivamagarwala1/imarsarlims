using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface Ibank_masterServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateBankMaster(bank_master Bank);
        Task<ServiceStatusResponseModel> UpdateBankStatus(int id, byte status, int userId);
    }
}

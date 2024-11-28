using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Interface
{
    public interface IobservationReferenceRangesServices
    {
        Task<ServiceStatusResponseModel> SaveUpdateReferenceRange(List<observationReferenceRanges> ObservationReferenceRanges);
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace iMARSARLIMS.Model.Transaction
{
    [Table(nameof(tnx_Observations_Histo))]
    public class tnx_Observations_Histo : Audit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int histoObservationId { get; set; }
        public int? testId { get; set; }
        public string? clinicalHistory { get; set; }
        public string? specimen { get; set; }
        public string? gross { get; set; }
        public string? typesFixativeUsed { get; set; }
        public string? blockKeys { get; set; }
        public string? stainsPerformed { get; set; }
        public string? biospyNumber { get; set; }
        public string? microscopy { get; set; }
        public string? finalImpression { get; set; }
        public string? comment { get; set; }
        public DateTime dtEntry { get; set; }
    }
}

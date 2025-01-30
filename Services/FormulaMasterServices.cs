using iMARSARLIMS.Controllers;
using iMARSARLIMS.Interface;
using iMARSARLIMS.Model.Master;
using iMARSARLIMS.Response_Model;

namespace iMARSARLIMS.Services
{
    public class FormulaMasterServices : IFormulaMasterServices
    {
        private readonly ContextClass db;
        public FormulaMasterServices(ContextClass context, ILogger<BaseController<formulaMaster>> logger)
        {
            db = context;
        }
        public async Task<ServiceStatusResponseModel> SaveUpdateFormula(formulaMaster Formula)
        {
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var count= db.formulaMaster.Where(f=> f.observationId==Formula.observationId && f.itemId==Formula.itemId).Count();
                    if (count == 0)
                    {
                        db.formulaMaster.Add(Formula);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Saved Successful"
                        };
                    }
                    else
                    {
                        var data= db.formulaMaster.Where(f => f.observationId == Formula.observationId && f.itemId == Formula.itemId).FirstOrDefault();
                        data.formula = Formula.formula;
                        data.FormulaText = Formula.FormulaText;
                        data.updateById= Formula.updateById;
                        data.updateDateTime= DateTime.Now;
                        db.formulaMaster.Update(data);
                        await db.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ServiceStatusResponseModel
                        {
                            Success = true,
                            Message = "Updated Successful"
                        };
                    }
                    
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ServiceStatusResponseModel
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }
            }
        }
    }
}

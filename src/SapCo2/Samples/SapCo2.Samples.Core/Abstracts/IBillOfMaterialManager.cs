using System.Threading.Tasks;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Abstracts
{
    public interface IBillOfMaterialManager:IPrintable<BomOutputParameter>,IPrintable<Topmat>
    {
        Task<BomOutputParameter> GetBillOfMaterialAsync(string materialCode, string plantCode, string alias = null);
    }
}

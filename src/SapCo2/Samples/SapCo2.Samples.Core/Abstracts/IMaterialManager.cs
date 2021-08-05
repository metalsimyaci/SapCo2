using System.Collections.Generic;
using System.Threading.Tasks;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Abstracts
{
    public interface IMaterialManager: IPrintable<List<Material>>,IPrintable<Material>
    {
        Task<Material> GetMaterialAsync(string materialCode);
        Task<Material> GetMaterialWithSubTableAsync(string materialCode);
        Task<List<Material>> GetMaterialsByPrefixAsync(string materialCodePrefix, int recordCount = 5);
        Task<List<Material>> GetMaterialsByPrefixWithSubTablesAsync(string materialCodePrefix, int recordCount = 5);
    }
}

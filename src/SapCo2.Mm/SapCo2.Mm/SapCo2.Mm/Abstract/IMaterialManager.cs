using System.Collections.Generic;
using System.Threading.Tasks;
using SapCo2.Mm.Entity;

namespace SapCo2.Mm.Abstract
{
    public interface IMaterialManager
    {
        Task<Material> GetMaterialAsync(string materialCode, MaterialQueryOptions options = null,
            bool getUnsafeFields = true);

        Task<List<Material>> GetMaterialsAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0);
    }
}

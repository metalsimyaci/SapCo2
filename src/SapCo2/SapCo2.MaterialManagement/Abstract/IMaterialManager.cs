using System.Collections.Generic;
using System.Threading.Tasks;
using SapCo2.MaterialManagement.Entity;

namespace SapCo2.MaterialManagement.Abstract
{
    public interface IMaterialManager
    {
        Task<Material> GetMaterialAsync(string materialCode, MaterialQueryOptions options = null,
            bool getUnsafeFields = true);

        Task<List<Material>> GetMaterialsAsync(string materialCodePrefix,
            MaterialQueryOptions options = null, bool getUnsafeFields = true, int rowCount = 0);
    }
}

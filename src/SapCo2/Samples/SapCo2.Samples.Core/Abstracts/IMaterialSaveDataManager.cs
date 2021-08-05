using System.Threading.Tasks;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Abstracts
{
    public interface IMaterialSaveDataManager:IPrintable<MaterialSaveDataBapiOutputParameter>
    {
        Task<MaterialSaveDataBapiOutputParameter> CreateMaterialAsync();
    }
}

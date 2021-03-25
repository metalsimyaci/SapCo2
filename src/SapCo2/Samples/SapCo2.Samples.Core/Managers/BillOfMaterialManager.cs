using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Managers
{
    public class BillOfMaterialManager: IBillOfMaterialManager
    {
        private readonly IServiceProvider _serviceProvider;

        public BillOfMaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<BomOutputParameter> GetBillOfMaterialAsync(string materialCode, string plantCode,string alias=null)
        {
            var inputParameter = new BomInputParameter
            {
                Aumgb = "0",
                Capid = "PP01",
                Datuv = DateTime.Now,
                Emeng = "1",
                Mktls = "x",
                Mehrs = "x",
                Stpst = "0",
                Svwvo = "x",
                Werks = plantCode,
                Vrsvo = "x",
                Stlal = "1",
                Stlan = "1",
                Mtnrv = materialCode
            };
            using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
            if(!string.IsNullOrWhiteSpace(alias))
                client.UseServer(alias);
            BomOutputParameter bomResult = await client.ExecuteRfcAsync<BomInputParameter, BomOutputParameter>("CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
            return bomResult;
        }

        public void Print(BomOutputParameter model)
        {
            if (model == null)
            {
                Console.WriteLine("BillOfMaterial Not Found!");
                return;
            }

            Console.WriteLine($"Material:\t{model.Topmat.Code} - {model.Topmat.Definition} ");
            Console.WriteLine("".PadLeft(20, '='));
            foreach (Stb stb in model.StbData)
                Console.WriteLine($"{stb.Name}\t {stb.SubItemCode}-{stb.SubItemName}");
        }

        public void Print(Topmat model)
        {
            if (model == null)
            {
                Console.WriteLine("BillOfMaterial Not Found!");
                return;
            }
            Console.WriteLine($"Material:\t{model.Code} - {model.Definition} successfully exploded.");
        }
    }
}

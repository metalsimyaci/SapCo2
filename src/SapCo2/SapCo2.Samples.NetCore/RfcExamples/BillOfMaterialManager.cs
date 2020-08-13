using System;
using SapCo2.Core.Abstract;
using SapCo2.Samples.NetCore.RfcExamples.Models;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;

namespace SapCo2.Samples.NetCore.RfcExamples
{
    public sealed class BillOfMaterialManager
    {
        private readonly IServiceProvider _serviceProvider;

        public BillOfMaterialManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BomOutputParameter GetBillOfMaterial(string materialCode, string plantCode)
        {
            using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

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
            using IReadRfc rfcFunction = _serviceProvider.GetService<IReadRfc>();
            BomOutputParameter bomResult = rfcFunction.GetRfc<BomOutputParameter, BomInputParameter>(connection, "CS_BOM_EXPL_MAT_V2_RFC", inputParameter);
            return bomResult;
        }

        public void Print(BomOutputParameter bom)
        {
            if(bom==null)
                Console.WriteLine("BillOfMaterial Not Found!");

            Console.WriteLine($"Material:\t{bom.Topmat.Code} - {bom.Topmat.Definition} ");
            Console.WriteLine("".PadLeft(20, '='));
            foreach (Stb stb in bom.StbData) 
                Console.WriteLine($"{stb.Name}\t {stb.SubItemCode}-{stb.SubItemName}");
        }
    }
}

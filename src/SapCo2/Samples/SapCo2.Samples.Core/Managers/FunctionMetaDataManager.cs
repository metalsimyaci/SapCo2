using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Models;
using SapCo2.Samples.Core.Abstracts;

namespace SapCo2.Samples.Core.Managers
{
    public class FunctionMetaDataManager:IFunctionMetaDataManager
    {
        private readonly IServiceProvider _serviceProvider;

        public FunctionMetaDataManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public List<ParameterMetaData> GetFunctionMetaData(string functionName)
        {
            using IRfcClient client = _serviceProvider.GetRequiredService<IRfcClient>();
            List<ParameterMetaData> result = client.ReadFunctionMetaData(functionName);
            
            return result;
        }
        public void Print(List<ParameterMetaData> model)
        {
            if (!model?.Any()??true)
            {
                Console.WriteLine("\nFunction Parameter Meta Data Not Found!");
                return;
            }

            Console.WriteLine("".PadLeft(20, '='));
            Console.WriteLine($"======= Function Parameter Meta Data List ================");
            var counter = 1;
            foreach (ParameterMetaData item in model)
            {
                Console.WriteLine($"\n{counter}.Parameter:{item.Name}-{item.Description}-{item.Type}-{item.NumericLength}");
                Print(item.Fields);
                counter++;
            }
        }
        public void Print(List<FieldMetaData> model)
        {
            if (!model?.Any()??true)
            {
                Console.WriteLine("Function Field Meta Data Not Found!");
                return;
            }

            Console.WriteLine($"======= Function Field Meta Data List ================");
            var counter = 1;
            foreach (FieldMetaData fieldMetaData in model)
            {
                Console.WriteLine($"{counter}.Field:{fieldMetaData.Name}-{fieldMetaData.Type}-{fieldMetaData.NucLength}");
                counter++;
            }
        }
    }
}

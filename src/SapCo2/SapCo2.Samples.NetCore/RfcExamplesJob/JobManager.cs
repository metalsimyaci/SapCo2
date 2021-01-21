using System;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Core.Abstract;
using SapCo2.Samples.NetCore.RfcExamplesJob.Models;

namespace SapCo2.Samples.NetCore.RfcExamplesJob
{
     public sealed class JobManager
     {
          private readonly IServiceProvider _serviceProvider;

        public JobManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public GetJobOutputParameter GetJobs()
        {
            using IRfcConnection connection = _serviceProvider.GetService<IRfcConnection>();
            connection.Connect();

            var inputParameter = new GetJobInputParameter
            {
                StartDate = $"{DateTime.Today:yyyyMMdd}",
                EndDate = $"{DateTime.Today:yyyyMMdd}",
                Status = "A",
                ProgramName = "Z*",
                ClientCode = "200"
            };
            using IReadRfc rfcFunction = _serviceProvider.GetService<IReadRfc>();
            GetJobOutputParameter result = rfcFunction.GetRfc<GetJobOutputParameter, GetJobInputParameter>(connection, "ZBC_GET_JOBS", inputParameter);
            return result;
        }


        public void Print(GetJobOutputParameter output)
        {
            if (output == null)
            {
                Console.WriteLine("\n Material Color Not Found");
                return;
            }
            Console.WriteLine("\n## Job List ##");
            foreach (JobStatus jobItem in output.JobStatuses)
            {
                Console.WriteLine($"\n JobName:\t{jobItem.JobName}, JobNo:\t{jobItem.JobNo}, TransactionCode:\t{jobItem.TransactionCode}, TransactionCodeDefinition:\t{jobItem.TransactionCodeDefinition}");
            }
            Console.WriteLine("\n## Job List  END ##");

        }
     }
}

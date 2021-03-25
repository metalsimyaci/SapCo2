using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SapCo2.Abstract;
using SapCo2.Samples.Core.Abstracts;
using SapCo2.Samples.Core.Models;

namespace SapCo2.Samples.Core.Managers
{
    public class JobManager : IJobManager
    {
        private readonly IServiceProvider _serviceProvider;

        public JobManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<GetJobOutputParameter> GetJobsAsync()
        {
            var inputParameter = new GetJobInputParameter
            {
                StartDate = $"{DateTime.Today:yyyyMMdd}",
                EndDate = $"{DateTime.Today:yyyyMMdd}",
                Status = "A",
                ProgramName = "Z*",
                ClientCode = "200"
            };

            using IRfcClient sapClient = _serviceProvider.GetRequiredService<IRfcClient>();
            return await sapClient.ExecuteRfcAsync<GetJobInputParameter, GetJobOutputParameter>("ZBC_GET_JOBS", inputParameter);
        }
        public void Print(GetJobOutputParameter model)
        {
            Console.WriteLine("\n## Job List ##");

            if (model == null || ((model?.JobStatuses?.GetLength(0) ?? 0) <= 0))
            {
                Console.WriteLine($"\n Job Not Found");
                return;
            }

            foreach (JobStatus jobItem in model.JobStatuses)
            {
                Console.WriteLine($"\n JobName:\t{jobItem.JobName}, JobNo:\t{jobItem.JobNo}, TransactionCode:\t{jobItem.TransactionCode}, TransactionCodeDefinition:\t{jobItem.TransactionCodeDefinition}");
            }

            Console.WriteLine("\n## Job List  END ##");
        }
    }
}

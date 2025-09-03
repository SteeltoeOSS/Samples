using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace VoteHandler
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var host = StreamHost
              .CreateDefaultBuilder<VoteHandler>(args)
              .ConfigureServices(svc=> svc.AddSingleton<IVotingService, DefaultVotingService>())
              .Build();
            await host.RunAsync();
        }

        [EnableBinding(typeof(ISink))]
        public class VoteHandler
        {
            private readonly IVotingService votingService;
            public VoteHandler(IVotingService service)
            {
                votingService = service;
            }

            [StreamListener(ISink.INPUT)]
            public void Handle(Vote vote)
            {
                votingService.Record(vote);
            }
        }
    }
}

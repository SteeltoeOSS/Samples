using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.Handler.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using Steeltoe.Stream.StreamHost;
using System.Threading.Tasks;

namespace TransformProcessor;

public class Program
{
    static async Task Main(string[] args)
    {
        var host = StreamHost
            .CreateDefaultBuilder<TransformProcessor>(args)
            .ConfigureServices(svc=> svc.AddSingleton<IVotingService, DefaultVotingService>())
            .Build();
        await host.RunAsync();
    }

    [EnableBinding(typeof(IProcessor))]
    public class TransformProcessor
    {
        private readonly IVotingService votingService;
        public TransformProcessor(IVotingService service)
        {
            votingService = service;
        }

        [StreamListener(IProcessor.INPUT)]
        [SendTo(IProcessor.OUTPUT)]
        public VoteResult Handle(Vote vote)
        {
            return votingService.Record(vote);
        }
    }

}
using Microsoft.Extensions.Logging;
using System;

namespace VoteHandler
{
    public interface IVotingService
    {
        void Record(Vote vote);
    }

    public class DefaultVotingService : IVotingService
    {
        private readonly ILogger<DefaultVotingService> _logger;

        public DefaultVotingService(ILogger<DefaultVotingService> logger)
        {
            _logger = logger;
        }

        public void Record(Vote vote)
        {
            Console.WriteLine("Received a vote for " + vote.Choice);
        }
    }
}
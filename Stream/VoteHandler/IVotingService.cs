using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Received a vote for {choice}", vote.Choice);
        }
    }
}
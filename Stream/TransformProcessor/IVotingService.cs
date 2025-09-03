using Microsoft.Extensions.Logging;

namespace TransformProcessor;

public interface IVotingService
{
    VoteResult Record(Vote vote);
}

public class DefaultVotingService : IVotingService
{
    private readonly ILogger<DefaultVotingService> _logger;

    public DefaultVotingService(ILogger<DefaultVotingService> logger)
    {
        _logger = logger;
    }

    public VoteResult Record(Vote vote)
    {
        _logger.LogInformation("Received a vote for {choice}", vote.Choice);
        return new VoteResult { Result = vote.Choice.ToUpper() };
    }
}
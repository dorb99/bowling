using bowlingApp.Models;
using bowlingApp.Constants;

namespace bowlingApp.Repository
{
    public interface IBowlingRepository
    {
        Task<Game> CreateNewGameAsync(string name);
        Task<Game?> GetGameAsync(int gameId);
        Task<Game?> AddFrameAsync(Game game, Frame newFrame);
        Task<List<HighScore>> GetTopHighScoresAsync(int limit = BowlingConstants.DefaultHighScoreLimit);
        Task AddHighScoreAsync(HighScore newScore, int limit = BowlingConstants.DefaultHighScoreLimit);
        Task<Game?> UpdateGameScoreAsync(Game game);
    }
}

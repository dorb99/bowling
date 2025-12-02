using bowlingApp.Models;

namespace bowlingApp.Services
{
    public interface IBowlingValidationService
    {
        /// <summary>
        /// Validates a roll input for a given frame index.
        /// </summary>
        /// <param name="input">The roll input to validate.</param>
        /// <param name="currentFrameIndex">The current frame index (0-9).</param>
        /// <returns>An error message if validation fails, otherwise null.</returns>
        string? ValidateRollInput(RollInput input, int currentFrameIndex);
    }
}


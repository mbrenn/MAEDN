using BrettSpielMeister.Logic;
using BrettSpielMeister.States;

namespace MAEDN.Rules
{
    public static class Extension
    {
        /// <summary>
        /// Converts the player state to the converted Player state
        /// </summary>
        /// <param name="playerState">The player state to be converted</param>
        /// <returns>The converted player state</returns>
        public static MaednPlayerState GetMaednPlayerState(this PlayerState playerState)
            => (MaednPlayerState) playerState;

        /// <summary>
        /// Converts the player state to the converted Player state
        /// </summary>
        /// <param name="playerState">The player state to be converted</param>
        /// <returns>The converted player state</returns>
        public static MaednPlayerState GetMaednPlayerState(this PlayerSet playerSet)
            => playerSet.State.GetMaednPlayerState();
    }
}
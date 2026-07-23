using FMODUnity;

namespace CorgiAudio
{
    /// <summary>
    /// Generic interface for connecting project-specific GameEvents to FMOD events.
    /// Each project implements this interface with its own event types.
    /// The package never references project-specific types directly.
    /// </summary>
    public interface IAudioEventBinding
    {
        /// <summary>Human-readable key for this binding (e.g., "OnJump", "OnPaintStroke").</summary>
        string EventKey { get; }

        /// <summary>The FMOD event to play when the bound game event fires.</summary>
        EventReference FmodEvent { get; }

        /// <summary>Called by AudioEventMappingSO.BindAll() at game start. Subscribes to the project's GameEvent.</summary>
        void Subscribe();

        /// <summary>Called by AudioEventMappingSO.UnbindAll() at game end. Unsubscribes from the project's GameEvent.</summary>
        void Unsubscribe();
    }
}
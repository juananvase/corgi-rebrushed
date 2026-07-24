using System;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CorgiAudio
{
    /// <summary>
    /// Manages music states and transitions between them using FMOD parameters.
    /// Music states are configured in the Inspector as a list of named EventReferences.
    /// </summary>
    public class FmodMusicController : MonoBehaviour
    {
        [System.Serializable]
        public class MusicState
        {
            [HorizontalGroup("Row", Width = 120)]
            public string Name;

            [HorizontalGroup("Row")]
            public EventReference FmodEvent;

            [HorizontalGroup("Row", Width = 60), Tooltip("Value sent to the MusicState FMOD parameter.")]
            public float ParameterValue;
        }

        [FoldoutGroup("Music States")]
        [SerializeField]
        private List<MusicState> _states = new();

        [FoldoutGroup("Settings")]
        [SerializeField, Range(0.1f, 10f)]
        private float _defaultTransitionTime = 2f;

        private string _currentState;

        public string CurrentState => _currentState;

        public void SetMusicState(string stateName, float transitionTime = -1f)
        {
            var state = _states.Find(s => s.Name == stateName);
            if (state == null)
            {
                Debug.LogWarning($"[CorgiAudio] Music state '{stateName}' not found.");
                return;
            }

            if (transitionTime <= 0f)
                transitionTime = _defaultTransitionTime;

            _currentState = stateName;

            // Update the FMOD parameter so the adaptive music system reacts.
            RuntimeManager.StudioSystem.setParameterByName("MusicState", state.ParameterValue);

            // Play the music event if it is not already playing.
            // For a simple approach, we stop the current and start the new one.
            // For adaptive music with a single event and stems, only the parameter matters.
            if (!state.FmodEvent.IsNull)
            {
                var instance = RuntimeManager.CreateInstance(state.FmodEvent);
                instance.start();
                instance.release();
            }

            Debug.Log($"[CorgiAudio] Music state changed to '{stateName}' (transition: {transitionTime}s).");
        }

        public void SetIntensity(float value)
        {
            RuntimeManager.StudioSystem.setParameterByName("MusicIntensity", Mathf.Clamp01(value));
        }

        public void StopMusic(float fadeOut = 1f)
        {
            // Let FMOD handle the fade via a snapshot or parameter.
            RuntimeManager.StudioSystem.setParameterByName("MusicIntensity", 0f);
            _currentState = null;
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestState(string stateName)
        {
            SetMusicState(stateName);
        }
#endif
    }
}
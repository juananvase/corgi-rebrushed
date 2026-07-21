using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CorgiAudio
{
    /// <summary>
    /// Receives game data (e.g., PlayerSpeed, TimeScale) and updates FMOD global parameters (RTPCs).
    /// Other scripts call SetParameter() to push values; this driver forwards them to FMOD.
    /// </summary>
    public class FmodParameterDriver : MonoBehaviour
    {
        [System.Serializable]
        public class ParameterBinding
        {
            [HorizontalGroup("Row", Width = 140)]
            public string Name;

            [HorizontalGroup("Row"), Tooltip("FMOD global parameter name (e.g., 'PlayerSpeed').")]
            public string FmodParameter;
        }

        [FoldoutGroup("Parameters")]
        [SerializeField]
        private List<ParameterBinding> _bindings = new();

        private readonly Dictionary<string, ParameterBinding> _lookup = new();

        private void Awake()
        {
            foreach (var b in _bindings)
            {
                if (!_lookup.ContainsKey(b.Name))
                    _lookup[b.Name] = b;
            }
        }

        /// <summary>
        /// Updates an FMOD global parameter by its logical name.
        /// </summary>
        /// <param name="name">Logical name matching a configured ParameterBinding (e.g., "PlayerSpeed").</param>
        /// <param name="value">Value to send to FMOD.</param>
        public void SetParameter(string name, float value)
        {
            if (!_lookup.TryGetValue(name, out var binding))
            {
                Debug.LogWarning($"[CorgiAudio] Parameter '{name}' not configured in FmodParameterDriver.");
                return;
            }

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(binding.FmodParameter, value);
        }

        /// <summary>
        /// Direct update by FMOD parameter name, bypassing lookup.
        /// </summary>
        public void SetParameterRaw(string fmodName, float value)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(fmodName, value);
        }

        /// <summary>
        /// Convenience method for boolean parameters (e.g., IsGrounded).
        /// </summary>
        public void SetParameterBool(string name, bool value)
        {
            SetParameter(name, value ? 1f : 0f);
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestSetParameter(string name, float value)
        {
            SetParameter(name, value);
        }
#endif
    }
}
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CorgiAudio
{
    /// <summary>
    /// Activates and deactivates FMOD snapshots for audio reactivity
    /// (e.g., Painting Mode, Pause, Underwater).
    /// </summary>
    public class FmodSnapshotController : MonoBehaviour
    {
        [System.Serializable]
        public class SnapshotEntry
        {
            [HorizontalGroup("Row", Width = 140)]
            public string Name;

            [HorizontalGroup("Row"), Tooltip("FMOD snapshot path (e.g., 'snapshot:/Snp_PaintingMode').")]
            public string SnapshotPath;
        }

        [FoldoutGroup("Snapshots")]
        [SerializeField]
        private List<SnapshotEntry> _entries = new();

        private readonly Dictionary<string, FMOD.Studio.EventInstance> _activeSnapshots = new();

        public void SetSnapshot(string name, float intensity = 1f, float transitionTime = 1f)
        {
            var entry = _entries.Find(e => e.Name == name);
            if (entry == null)
            {
                Debug.LogWarning($"[CorgiAudio] Snapshot '{name}' not found in configuration.");
                return;
            }

            if (_activeSnapshots.ContainsKey(name))
                return; // Already active.

            try
            {
                var snapshot = FMODUnity.RuntimeManager.CreateInstance(entry.SnapshotPath);
                if (!snapshot.hasHandle())
                {
                    Debug.LogWarning($"[CorgiAudio] Failed to get snapshot handle for '{entry.SnapshotPath}'.");
                    return;
                }

                // Set intensity (0 = off, 1 = full effect) and start.
                snapshot.setVolume(intensity);
                snapshot.start();
                _activeSnapshots[name] = snapshot;

                Debug.Log($"[CorgiAudio] Snapshot '{name}' activated (intensity: {intensity}, transition: {transitionTime}s).");
            }
            catch (Exception e)
            {
                Debug.LogError($"[CorgiAudio] Error activating snapshot '{name}': {e.Message}");
            }
        }

        public void ReleaseSnapshot(string name, float transitionTime = 0.5f)
        {
            if (!_activeSnapshots.TryGetValue(name, out var snapshot))
                return;

            if (!snapshot.hasHandle())
            {
                _activeSnapshots.Remove(name);
                return;
            }

            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _activeSnapshots.Remove(name);

            Debug.Log($"[CorgiAudio] Snapshot '{name}' released (transition: {transitionTime}s).");
        }

        public void ReleaseAll()
        {
            foreach (var kvp in _activeSnapshots)
            {
                if (kvp.Value.hasHandle())
                    kvp.Value.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            _activeSnapshots.Clear();
        }

        private void OnDestroy()
        {
            ReleaseAll();
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestSnapshot(string name)
        {
            SetSnapshot(name);
        }

        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestRelease(string name)
        {
            ReleaseSnapshot(name);
        }
#endif
    }
}
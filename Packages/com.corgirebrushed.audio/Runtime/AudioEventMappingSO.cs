using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CorgiAudio
{
    /// <summary>
    /// ScriptableObject that groups all IAudioEventBinding instances.
    /// Drag this into the AudioManager and call BindAll() / UnbindAll().
    /// Sound designers configure mappings in the Inspector via Odin's [InlineEditor].
    /// </summary>
    [CreateAssetMenu(menuName = "Corgi Audio/Event Mapping")]
    public class AudioEventMappingSO : ScriptableObject
    {
        [InlineEditor]
        [SerializeReference]
        [FoldoutGroup("Bindings")]
        public List<IAudioEventBinding> Bindings = new();

        [Button(ButtonSizes.Medium), FoldoutGroup("Bindings")]
        public void BindAll()
        {
            foreach (var binding in Bindings)
            {
                binding?.Subscribe();
            }

            Debug.Log($"[CorgiAudio] Bound {Bindings.Count} audio event bindings.");
        }

        [Button(ButtonSizes.Medium), FoldoutGroup("Bindings")]
        public void UnbindAll()
        {
            foreach (var binding in Bindings)
            {
                binding?.Unsubscribe();
            }

            Debug.Log($"[CorgiAudio] Unbound {Bindings.Count} audio event bindings.");
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            // In the Editor, bind on domain reload so that Play Mode tests work.
            // In builds, the AudioManager explicitly calls BindAll() in Awake.
            if (!Application.isPlaying) return;
            BindAll();
#endif
        }

        private void OnDisable()
        {
            UnbindAll();
        }
    }
}
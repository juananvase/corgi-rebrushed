using System;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CorgiAudio
{
    /// <summary>
    /// Central hub for all FMOD audio operations.
    /// Place this component on the AudioManager prefab.
    /// No other system should call RuntimeManager directly — everything goes through here.
    /// </summary>
    [AddComponentMenu("Corgi Audio/Fmod Audio Manager")]
    public class FmodAudioManager : MonoBehaviour
    {
        // ──────────────────────────────────────────────
        // Inspector — Bank Loading
        // ──────────────────────────────────────────────
        [FoldoutGroup("Banks")]
        [SerializeField, Tooltip("Bank names to load on Awake (e.g., 'SFX', 'Music', 'Ambience'). Master bank is always loaded automatically.")]
        private List<string> _bankNames = new();

        [FoldoutGroup("Banks")]
        [ShowInInspector, ReadOnly]
        private bool _banksLoaded;

        // ──────────────────────────────────────────────
        // Inspector — Volume Control
        // Controls bus:/ (master) and bus:/Music directly.
        // Range is 0–1 (attenuation only, never amplifies).
        // Applied once on Start().
        // ──────────────────────────────────────────────
        [FoldoutGroup("Volume Control")]
        [SerializeField, Range(0f, 1f)]
        private float _masterVolume = 1f;

        [FoldoutGroup("Volume Control")]
        [SerializeField, Range(0f, 1f)]
        private float _musicVolume = 1f;

        private FMOD.Studio.Bus _masterBus;
        private FMOD.Studio.Bus _musicBus;

        // ──────────────────────────────────────────────
        // Note: AudioEventMappingSO now lives in the project and is self-contained.
        // It binds/unbinds via its own OnEnable/OnDisable. No reference needed here.
        // ──────────────────────────────────────────────
        // Inspector — Runtime State (debug)
        // ──────────────────────────────────────────────
        [FoldoutGroup("Runtime State")]
        [ShowInInspector, ReadOnly]
        private string _currentMusicState = "None";

        [FoldoutGroup("Runtime State")]
        [ShowInInspector, ReadOnly]
        private int _activeSnapshotCount;

        // ──────────────────────────────────────────────
        // Singleton
        // ──────────────────────────────────────────────
        public static FmodAudioManager Instance { get; private set; }

        // ──────────────────────────────────────────────
        // Unity Lifecycle
        // ──────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBanks();

            // Cache bus handles for volume control
            RuntimeManager.StudioSystem.getBus("bus:/", out _masterBus);
            RuntimeManager.StudioSystem.getBus("bus:/Music", out _musicBus);
        }

        private void Start()
        {
            // Apply slider values once, after prefab deserialization is complete.
            ApplyMasterVolume();
            ApplyMusicVolume();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        // ──────────────────────────────────────────────
        // Bank Loading
        // ──────────────────────────────────────────────
        private void LoadBanks()
        {
            try
            {
                foreach (var bankName in _bankNames)
                {
                    RuntimeManager.LoadBank(bankName, true);
                }

                _banksLoaded = true;
                Debug.Log("[CorgiAudio] All banks loaded successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[CorgiAudio] Failed to load banks: {e.Message}");
            }
        }

        // ──────────────────────────────────────────────
        // Volume Control (Bus attenuation)
        // ──────────────────────────────────────────────
        private void ApplyMasterVolume()
        {
            if (_masterBus.hasHandle())
                _masterBus.setVolume(_masterVolume);
        }

        private void ApplyMusicVolume()
        {
            if (_musicBus.hasHandle())
                _musicBus.setVolume(_musicVolume);
        }

        // ──────────────────────────────────────────────
        // Public API — Volume
        // ──────────────────────────────────────────────
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            ApplyMasterVolume();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
        }

        // ──────────────────────────────────────────────
        // Public API — SFX
        // ──────────────────────────────────────────────
        public void PlayOneShot(EventReference fmodEvent)
        {
            if (!_banksLoaded || fmodEvent.IsNull)
                return;

            RuntimeManager.PlayOneShot(fmodEvent);
        }

        public void PlayOneShot(EventReference fmodEvent, Vector3 position)
        {
            if (!_banksLoaded || fmodEvent.IsNull)
                return;

            RuntimeManager.PlayOneShot(fmodEvent, position);
        }

        // ──────────────────────────────────────────────
        // Public API — Parameters (RTPCs)
        // ──────────────────────────────────────────────
        public void SetParameter(string name, float value)
        {
            if (!_banksLoaded)
                return;

            RuntimeManager.StudioSystem.setParameterByName(name, value);
        }

        public void SetParameterImmediate(string name, float value)
        {
            if (!_banksLoaded)
                return;

            RuntimeManager.StudioSystem.setParameterByName(name, value);
        }

        // ──────────────────────────────────────────────
        // Public API — Snapshots
        // ──────────────────────────────────────────────
        public void ActivateSnapshot(string snapshotName, float intensity = 1f)
        {
            if (!_banksLoaded || string.IsNullOrEmpty(snapshotName))
                return;

            var snapshot = RuntimeManager.CreateInstance(snapshotName);
            if (snapshot.hasHandle())
            {
                snapshot.setVolume(intensity);
                snapshot.start();
                _activeSnapshotCount++;
            }
        }

        public void ReleaseSnapshot(string snapshotName)
        {
            if (!_banksLoaded || string.IsNullOrEmpty(snapshotName))
                return;

            var snapshot = RuntimeManager.CreateInstance(snapshotName);
            if (snapshot.hasHandle())
            {
                snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                snapshot.release();
                _activeSnapshotCount--;
            }
        }

        // ──────────────────────────────────────────────
        // Public API — Music State
        // ──────────────────────────────────────────────
        public void SetMusicState(string stateName)
        {
            if (!_banksLoaded || string.IsNullOrEmpty(stateName))
                return;

            _currentMusicState = stateName;
            RuntimeManager.StudioSystem.setParameterByName("MusicState", GetMusicStateHash(stateName));
        }

        private float GetMusicStateHash(string stateName)
        {
            return stateName switch
            {
                "Exploration" => 0f,
                "Painting" => 1f,
                "MainMenu" => 2f,
                _ => 0f
            };
        }

        // ──────────────────────────────────────────────
        // Editor Helpers
        // ──────────────────────────────────────────────
#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestPlaySFX()
        {
            Debug.Log("[CorgiAudio] Test: Playing a one-shot. Ensure a valid EventReference is assigned in the binding.");
        }

        [Button(ButtonSizes.Medium), FoldoutGroup("Testing")]
        private void TestLoadBanks()
        {
            LoadBanks();
        }
#endif
    }
}
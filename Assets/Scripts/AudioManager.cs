using DG.Tweening;
using UnityEngine;

namespace Kulami
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Music")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _menuMusicClip;
        [SerializeField] private AudioClip _gameMusicClip;

        private float _musicVolume;

        [Header("Sound Effects")]
        [SerializeField] private AudioSource _soundEffectsSource;
        [SerializeField] private AudioClip _marblePlacedClip;
        [SerializeField] private AudioClip _tileMovedClip;

        public void Awake()
        {
            Instance = this;

            _musicVolume = _musicSource.volume;
        }

        public void PlayMenuMusic()
        {
            if (_musicSource.clip == _menuMusicClip)
                return;

            _musicSource.clip = _menuMusicClip;
            _musicSource.Play();
        }

        public void PlayGameMusic()
        {
            if (_musicSource.clip == _gameMusicClip)
                return;

            _musicSource.clip = _gameMusicClip;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        public void PlayMarblePlacedSound()
        {
            _soundEffectsSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _soundEffectsSource.PlayOneShot(_marblePlacedClip);
        }

        public void PlayTileMovedSound()
        {
            _soundEffectsSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            _soundEffectsSource.PlayOneShot(_tileMovedClip);
        }

        public void FadeMusic(float duration)
        {
            DOTween.To(() => _musicSource.volume, x => _musicSource.volume = x, 0f, duration);
        }

        public void ResetMusicVolume()
        {
            _musicSource.volume = _musicVolume;
        }
    }
}

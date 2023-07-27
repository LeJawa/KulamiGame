using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kulami.Graphics
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private Material _screenTransitionMaterial;
        [SerializeField] private Image _firstFrameImage;

        [SerializeField] private float _preTransitionTime = 0.2f;
        [SerializeField] private float _transitionTime = 1f;

        [SerializeField] private string _progressName = "_Progress";
        [SerializeField] private string _partAName = "_PartA";

        public float Duration => _preTransitionTime * 2 + _transitionTime * 2;

        private Coroutine _currentTransition;

        private void Start()
        {
            _firstFrameImage.enabled = true;

            _currentTransition = StartCoroutine(TransitionBCoroutine());
        }

        private IEnumerator TransitionBCoroutine()
        {
            _firstFrameImage.enabled = true;
            yield return new WaitForSeconds(_preTransitionTime);
            _firstFrameImage.enabled = false;

            float currentTime = 0f;
            while (currentTime < _transitionTime)
            {
                currentTime += Time.deltaTime;
                _screenTransitionMaterial.SetInt(_partAName, 0);
                _screenTransitionMaterial.SetFloat(_progressName, Mathf.Clamp01(currentTime / _transitionTime));
                yield return null;
            }
        }

        private IEnumerator TransitionACoroutine()
        {
            float currentTime = 0f;
            while (currentTime < _transitionTime)
            {
                currentTime += Time.deltaTime;
                _screenTransitionMaterial.SetInt(_partAName, 1);
                _screenTransitionMaterial.SetFloat(_progressName, Mathf.Clamp01(1 - currentTime / _transitionTime));
                yield return null;
            }
        }

        private IEnumerator FullTransitionCoroutine()
        {
            yield return TransitionACoroutine();
            yield return TransitionBCoroutine();
        }

        public void Play()
        {
            if (_currentTransition != null)
                StopCoroutine(_currentTransition);

            _currentTransition = StartCoroutine(FullTransitionCoroutine());
        }
    }
}

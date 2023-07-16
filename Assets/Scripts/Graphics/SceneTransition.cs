using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        private bool _partA = false;

        private void Start()
        {
            _firstFrameImage.enabled = true;

            StartCoroutine(TransitionBCoroutine());
        }

        private IEnumerator TransitionBCoroutine()
        {
            _partA = false;

            _firstFrameImage.enabled = true;
            yield return new WaitForSeconds(_preTransitionTime);
            _firstFrameImage.enabled = false;

            yield return MainBodyTransitionCoroutine();
        }

        private IEnumerator MainBodyTransitionCoroutine()
        {
            float currentTime = 0f;
            while (currentTime < _transitionTime)
            {
                currentTime += Time.deltaTime;
                _screenTransitionMaterial.SetInt(_partAName, _partA ? 1 : 0);
                _screenTransitionMaterial.SetFloat(_progressName, Mathf.Clamp01(currentTime / _transitionTime));
                yield return null;
            }
        }

        private IEnumerator TransitionACoroutine()
        {
            _partA = true;
            yield return MainBodyTransitionCoroutine();
        }

        private IEnumerator FullTransitionCoroutine()
        {
            yield return TransitionACoroutine();
            yield return TransitionBCoroutine();
        }

        public void Play()
        {
            StartCoroutine(FullTransitionCoroutine());
        }
    }
}

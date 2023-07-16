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

        [SerializeField] private string _propertyName = "_Progress";


        private void Start()
        {
            _firstFrameImage.enabled = true;

            StartCoroutine(TransitionCoroutine());
        }

        private IEnumerator TransitionCoroutine()
        {
            yield return new WaitForSeconds(_preTransitionTime);
            _firstFrameImage.enabled = false;

            float currentTime = 0f;
            while (currentTime < _transitionTime)
            {
                currentTime += Time.deltaTime;
                _screenTransitionMaterial.SetFloat(_propertyName, Mathf.Clamp01(currentTime / _transitionTime));
                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class MarbleGO : MonoBehaviour
    {
        [SerializeField] private Transform _circle;

        private Vector3 _circleScale;

        [SerializeField] private ParticleSystem _spawnParticles;

        // Start is called before the first frame update
        void Start()
        {
            InitializeVariables();
            AnimateAppearance();
        }

        private void InitializeVariables()
        {
            _circleScale = _circle.localScale;
        }

        private void AnimateAppearance()
        {
            _circle.localScale = Vector3.zero;
            LeanTween.scale(_circle.gameObject, _circleScale, 0.8f).setEaseOutElastic();

            // particle system is disabled at the moment
            //_spawnParticles.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class SpawnEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;

        public void Play()
        {
            foreach (var particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}

using Kulami.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Helpers
{
    public class MarbleSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _marblePrefab;

        [SerializeField] private ParticleSystem _particleSystem;

        GameObject _marble;

        private void SpawnMarble()
        {
            _marble = Instantiate(_marblePrefab, Vector3.zero, Quaternion.identity);
            _particleSystem.Play();
        }

        void Update()
        {
            if (InputManager.Instance.AnyKeyDown)
            {
                if (_marble == null)
                    SpawnMarble();
                else
                    Destroy(_marble);
            }
        }
    }
}

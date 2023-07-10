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


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                if (_marble == null)
                    SpawnMarble();
                else
                    Destroy(_marble);
            }
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class MarbleGO : MonoBehaviour
    {
        [SerializeField] private Transform _circle;

        private Vector3 _circleScale;

        [SerializeField] private SpawnEffect _spawnEffect;

        [SerializeField] private Sprite _P1Sprite;
        [SerializeField] private Sprite _P2Sprite;

        // Start is called before the first frame update
        void Start()
        {
            InitializeVariables();
            AnimateAppearance();
        }

        public void SetPlayer(Player player)
        {
            if (player == Player.One)
            {
                _circle.GetComponent<SpriteRenderer>().sprite = _P1Sprite;
            }
            else 
            {
                _circle.GetComponent<SpriteRenderer>().sprite = _P2Sprite;
            }
        }

        private void InitializeVariables()
        {
            _circleScale = _circle.localScale;
        }

        private void AnimateAppearance()
        {
            _circle.localScale = Vector3.zero;
            _circle.transform.DOScale(_circleScale, 0.8f).SetEase(Ease.OutElastic);

            // particle system is disabled at the moment
            _spawnEffect.Play();
        }
    }
}

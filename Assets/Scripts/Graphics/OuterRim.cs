using UnityEngine;
using UnityEngine.UI;

namespace Kulami.Graphics
{
    public class OuterRim : MonoBehaviour
    {
        [SerializeField] private Sprite _p1Sprite;
        [SerializeField] private Sprite _p2Sprite;

        private Image _imageRenderer;

        private void Awake()
        {
            _imageRenderer = GetComponent<Image>();
        }

        public void SetPlayer(Player player)
        {
            _imageRenderer.sprite = player == Player.One ? _p1Sprite : _p2Sprite;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

    }
}
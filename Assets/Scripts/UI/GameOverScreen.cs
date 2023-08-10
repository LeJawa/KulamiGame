using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using System;
using Kulami.Graphics;

namespace Kulami.UI
{
    public class GameOverScreen : MonoBehaviour
    {


        [Header("Setup")]
        [SerializeField] private TextMeshProUGUI _winnerText;
        private RectTransform _winnerRectTransform;
        [SerializeField] private RectTransform _backButton;
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private CanvasGroup _scoreP1;
        [SerializeField] private CanvasGroup _scoreP2;

        [Header("Start")]
        [SerializeField] private float _scoreFadeTime = 0.4f;
        [SerializeField] private float _startDelay = 0f;
        [SerializeField] private float _gameOverAnimationDuration = 1f;
        [SerializeField] private Ease _startEase = Ease.OutBounce;
        [SerializeField] private float _delayBetweenGameOverAndButtons = 0.5f;
        [SerializeField] private float _buttonStartAnimationDuration = 1f;


        private Vector3 _buttonStartScale;
        private Vector3 _winnerStartScale;

        private float _backgroundStartAlpha;

        private Sequence _gameOverSequence;
        private Tween _backButtonStartTween;

        private bool _isAnimating = false;


        private void Awake()
        {
            _winnerRectTransform = _winnerText.GetComponent<RectTransform>();

            _buttonStartScale = _backButton.localScale;
            _winnerStartScale = _winnerRectTransform.localScale;
            _backgroundStartAlpha = _backgroundImage.color.a;
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(string winnerText, Color winnerTextColor)
        {
            gameObject.SetActive(true);
            _winnerText.text = winnerText;
            _winnerText.color = winnerTextColor;

            StartCoroutine(StartAnimation());
        }

        public void Hide()
        {
            if (!_isAnimating)
            {
                gameObject.SetActive(false);
            }
        }

        private IEnumerator StartAnimation()
        {
            _isAnimating = true;
            yield return AnimateTitleStart();
        }

        private IEnumerator AnimateTitleStart()
        {
            _backButton.localScale = Vector3.zero;
            _winnerRectTransform.localScale = Vector3.zero;

            var transparentColor = _backgroundImage.color;
            transparentColor.a = 0f;
            _backgroundImage.color = transparentColor;

            _scoreP1.DOFade(0f, _scoreFadeTime).SetEase(Ease.InOutQuad);
            _scoreP2.DOFade(0f, _scoreFadeTime).SetEase(Ease.InOutQuad).OnComplete(GameUI.Instance.MoveScoresToGameOverPosition);

            yield return new WaitForSeconds(_startDelay);

            _gameOverSequence = DOTween.Sequence();
            _gameOverSequence.Append(_winnerRectTransform.DOScale(_winnerStartScale, _gameOverAnimationDuration).SetEase(_startEase));
            _gameOverSequence.Join(_backgroundImage.DOFade(_backgroundStartAlpha, _gameOverAnimationDuration).SetEase(Ease.InOutQuad));
            _gameOverSequence.AppendInterval(_delayBetweenGameOverAndButtons);
            _gameOverSequence.Append(_backButton.DOScale(_buttonStartScale, _buttonStartAnimationDuration).SetEase(_startEase));
            _gameOverSequence.Join(_scoreP1.DOFade(1f, _scoreFadeTime).SetEase(Ease.InOutQuad));
            _gameOverSequence.Join(_scoreP2.DOFade(1f, _scoreFadeTime).SetEase(Ease.InOutQuad));
            _gameOverSequence.OnComplete(() => _isAnimating = false);

            _gameOverSequence.Play();
        }

        private void StopTweens()

        {
            _backButtonStartTween.Kill();
            _gameOverSequence.Kill();
        }

        public void ShowWithoutAnimation()
        {
            if (!_isAnimating)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
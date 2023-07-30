using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kulami
{
    public class StartMenu : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _newGameButton;
        [SerializeField] private RectTransform _exitButton;

        [Header("Start")]
        [SerializeField] private float _startDelay = 1f;
        [SerializeField] private float _titleStartAnimationDuration = 1f;
        [SerializeField] private Ease _startEase = Ease.OutBounce;
        [SerializeField] private float _delayBetweenTitleStartAndButtons = 0.5f;
        [SerializeField] private float _buttonStartAnimationDuration = 1f;


        [Header("Rotation")]
        [SerializeField] private float _rotationAmount = 15f;
        [SerializeField] private float _rotationTime = 1.5f;
        [SerializeField] private Ease _rotationEase = Ease.InOutCubic;
        [SerializeField] private RotateMode _rotateMode = RotateMode.Fast;

        [Header("Scaling")]
        [SerializeField] private float _scaleAmount = 0.1f;
        [SerializeField] private float _scaleTime = 1.5f;
        [SerializeField] private Ease _scaleEase = Ease.InOutCubic;

        private Vector3 _titleStartScale;
        private Vector3 _buttonStartScale;

        private Sequence _titleRotationSequence;
        private Sequence _titleScalingSequence;
        private Tween _titleStartTween;
        private Tween _newGameButtonStartTween;
        private Tween _exitButtonStartTween;


        private void Awake()
        {
            _titleStartScale = _title.localScale;
            _buttonStartScale = _newGameButton.localScale;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StopTweens();
                StartCoroutine(AnimateTitleStart());
            }
        }


        private IEnumerator Start()
        {
            DoTitleRotation();
            yield return AnimateTitleStart();

        }

        private IEnumerator AnimateTitleStart()
        {
            _title.localScale = Vector3.zero;
            _newGameButton.localScale = Vector3.zero;
            _exitButton.localScale = Vector3.zero;
            yield return new WaitForSeconds(_startDelay);
            _titleStartTween = _title.DOScale(_titleStartScale, _titleStartAnimationDuration).SetEase(_startEase).OnComplete(() => { DoTitleScaling(); });
            yield return new WaitForSeconds(_delayBetweenTitleStartAndButtons);
            _newGameButtonStartTween = _newGameButton.DOScale(_buttonStartScale, _buttonStartAnimationDuration).SetEase(_startEase);
            _exitButtonStartTween = _exitButton.DOScale(_buttonStartScale, _buttonStartAnimationDuration).SetEase(_startEase);
        }

        private void DoTitleRotation()
        {
            _titleRotationSequence = DOTween.Sequence();

            var rotation = Random.Range(0, _rotationAmount);
            _titleRotationSequence.Append(_title.DORotate(new Vector3(0, 0, rotation), _rotationTime, _rotateMode).SetEase(_rotationEase));

            rotation = Random.Range(0, _rotationAmount);
            _titleRotationSequence.Append(_title.DORotate(new Vector3(0, 0, -rotation), _rotationTime, _rotateMode).SetEase(_rotationEase));
            _titleRotationSequence.OnComplete(DoTitleRotation);

            _titleRotationSequence.Play();
        }

        private void DoTitleScaling()
        {
            _titleScalingSequence = DOTween.Sequence();

            var scaling = Random.Range(1, 1+_scaleAmount);
            _titleScalingSequence.Append(_title.DOScale(_titleStartScale * scaling, _scaleTime).SetEase(_scaleEase));

            scaling = Random.Range(1 - _scaleAmount, 1);
            _titleScalingSequence.Append(_title.DOScale(_titleStartScale * scaling, _scaleTime).SetEase(_scaleEase));
            _titleScalingSequence.OnComplete(DoTitleScaling);
            _titleScalingSequence.Play();
        }

        private void StopTweens()
        {
            _exitButtonStartTween.Kill();
            _newGameButtonStartTween.Kill();
            _titleStartTween.Kill();
            _titleRotationSequence.Kill();
            _titleScalingSequence.Kill();
        }
        
    }
}

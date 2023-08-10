using DG.Tweening;
using Kulami.Control;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kulami.UI
{
    public class MainMenu : MonoBehaviour
    {
        private struct UIElementAnimationData
        {
            public RectTransform RectTransform;
            public Vector3 StartScale;
            public Tween Tween;
        }


        [Header("Setup")]
        [SerializeField] private RectTransform _title;
        [SerializeField] private RectTransform _buttons;

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

        private Sequence _titleRotationSequence;
        private Sequence _titleScalingSequence;
        private Tween _titleStartTween;

        private List<UIElementAnimationData> _uiElementAnimationDataList = new();


        private void Awake()
        {
            _titleStartScale = _title.localScale;

            var animatableElements = _buttons.GetComponentsInChildren<RectTransform>().ToList();

            animatableElements = animatableElements.Where(element => element.CompareTag("AnimatableUI")).ToList();

            foreach (var button in animatableElements)
            {
                _uiElementAnimationDataList.Add(new UIElementAnimationData
                {
                    RectTransform = button,
                    StartScale = button.localScale
                });
            }

        }

        private void Update()
        {
#if UNITY_EDITOR
            if (InputManager.Instance.GetTestDown())
            {
                StartAnimation();
            }
#endif
        }

        private void StartAnimation()
        {
            StopTweens();
            StartCoroutine(AnimateTitleStart());
        }

        private IEnumerator Start()
        {
            yield return AnimateTitleStart();

        }



        private IEnumerator AnimateTitleStart()
        {
            _title.localScale = Vector3.zero;

            foreach (var uiElementAnimationData in _uiElementAnimationDataList)
            {
                uiElementAnimationData.RectTransform.localScale = Vector3.zero;
            }

            yield return new WaitForSeconds(_startDelay);
            _titleStartTween = _title.DOScale(_titleStartScale, _titleStartAnimationDuration).SetEase(_startEase).OnComplete(() => { DoTitleScaling(); });

            yield return new WaitForSeconds(_delayBetweenTitleStartAndButtons);

            for (int i = 0; i < _uiElementAnimationDataList.Count; i++)
            {
                var uiElementAnimationData = _uiElementAnimationDataList[i];
                uiElementAnimationData.Tween = uiElementAnimationData.RectTransform.DOScale(uiElementAnimationData.StartScale, _buttonStartAnimationDuration).SetEase(_startEase);
            }

            DoTitleRotation();
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

            var scaling = Random.Range(1, 1 + _scaleAmount);
            _titleScalingSequence.Append(_title.DOScale(_titleStartScale * scaling, _scaleTime).SetEase(_scaleEase));

            scaling = Random.Range(1 - _scaleAmount, 1);
            _titleScalingSequence.Append(_title.DOScale(_titleStartScale * scaling, _scaleTime).SetEase(_scaleEase));
            _titleScalingSequence.OnComplete(DoTitleScaling);
            _titleScalingSequence.Play();
        }

        private void StopTweens()
        {
            _titleStartTween.Kill();
            _titleRotationSequence.Kill();
            _titleScalingSequence.Kill();

            foreach (var uiElementAnimationData in _uiElementAnimationDataList)
            {
                uiElementAnimationData.Tween.Kill();
            }
        }

    }
}

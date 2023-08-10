using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.UI
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private OuterRim _outerRim;

        [SerializeField]
        private UICounter _playerOneScore;
        [SerializeField]
        private UICounter _playerTwoScore;

        private RectTransform _playerOneScoreRectTransform;
        private RectTransform _playerTwoScoreRectTransform;

        [SerializeField] bool _disableOuterRim = true;

        public Player CurrentPlayer 
        { 
            set
            {
                _outerRim.SetPlayer(value);
                Show();
            } 
        }

        public int PlayerOneScore
        { 
            set
            {
                _playerOneScore.SetScore(value);
            }
        }
        public int PlayerTwoScore
        {
            set
            {
                _playerTwoScore.SetScore(value);
            }
        }

        private void Start()
        {
            _playerOneScoreRectTransform = _playerOneScore.transform.parent.GetComponent<RectTransform>();
            _playerTwoScoreRectTransform = _playerTwoScore.transform.parent.GetComponent<RectTransform>();

            Hide();

            if (_disableOuterRim)
            {
                _outerRim.Disable();
            }

            GameOptions.Instance.showScoresDuringGameChanged += ShowScoresDuringGameChanged;
        }

        private void ShowScoresDuringGameChanged(bool showScores)
        {
            _playerOneScoreRectTransform.gameObject.SetActive(showScores);
            _playerTwoScoreRectTransform.gameObject.SetActive(showScores);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        [SerializeField] private Vector3 _playerOneScorePosition = new(25, -25, 0);
        [SerializeField] private Vector3 _playerTwoScorePosition = new(-25, -25, 0);

        [SerializeField] private Vector3 _playerOneScoreGameOverPosition = new(483, -606, 0);
        [SerializeField] private Vector3 _playerTwoScoreGameOverPosition = new(-483, -606, 0);

        public void MoveScoresToGameOverPosition()
        {
            _playerOneScoreRectTransform.anchoredPosition3D = _playerOneScoreGameOverPosition;
            _playerTwoScoreRectTransform.anchoredPosition3D = _playerTwoScoreGameOverPosition;

            _playerOneScoreRectTransform.gameObject.SetActive(true);
            _playerTwoScoreRectTransform.gameObject.SetActive(true);
        }

        private void MoveScoresToInitialPosition()
        {
            _playerOneScoreRectTransform.anchoredPosition3D = _playerOneScorePosition;
            _playerTwoScoreRectTransform.anchoredPosition3D = _playerTwoScorePosition;

            _playerOneScoreRectTransform.gameObject.SetActive(GameOptions.Instance.ShowScoresDuringGame);
            _playerTwoScoreRectTransform.gameObject.SetActive(GameOptions.Instance.ShowScoresDuringGame);
        }

        public void Initialize()
        {
            MoveScoresToInitialPosition();
        }
    }
}

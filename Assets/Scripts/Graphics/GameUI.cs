using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        private OuterRim _outerRim;

        [SerializeField]
        private UICounter _playerOneScore;
        [SerializeField]
        private UICounter _playerTwoScore;

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
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}

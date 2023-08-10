using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kulami
{
    public class GameOptions : MonoBehaviour
    {
        public static GameOptions Instance { get; private set; }

        private string _showPossibleMovesKey = "ShowPossibleMoves";
        [SerializeField]
        private bool _showPossibleMoves = true;
        private string _showScoresDuringGameKey = "ShowScoresDuringGame";
        [SerializeField]
        private bool _showScoresDuringGame = true;
        private string _showTileOwnershipKey = "ShowTileOwnership";
        [SerializeField]
        private bool _showTileOwnership = true;

        public bool ShowPossibleMoves => _showPossibleMoves;
        public bool ShowScoresDuringGame => _showScoresDuringGame;
        public bool ShowTileOwnership => _showTileOwnership;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (Instance != this)
            {
                Destroy(this);
            }

            _showPossibleMoves = PlayerPrefs.GetInt(_showPossibleMovesKey, 1) == 1;
            _showScoresDuringGame = PlayerPrefs.GetInt(_showScoresDuringGameKey, 1) == 1;
            _showTileOwnership = PlayerPrefs.GetInt(_showTileOwnershipKey, 1) == 1;
        }
        
        public void SetShowPossibleMoves(Toggle showPossibleMoves)
        {
            _showPossibleMoves = showPossibleMoves.isOn;
            PlayerPrefs.SetInt(_showPossibleMovesKey, showPossibleMoves ? 1 : 0);
        }


        public void SetShowScoresDuringGame(Toggle showScoresDuringGame)
        {
            _showScoresDuringGame = showScoresDuringGame.isOn;
            PlayerPrefs.SetInt(_showScoresDuringGameKey, showScoresDuringGame ? 1 : 0);
        }


        public void SetShowTileOwnership(Toggle showTileOwnership)
        {
            _showTileOwnership = showTileOwnership.isOn;
            PlayerPrefs.SetInt(_showTileOwnershipKey, showTileOwnership ? 1 : 0);
        }

    }
}

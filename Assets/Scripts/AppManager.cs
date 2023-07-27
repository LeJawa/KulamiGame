using System;
using UnityEngine;

namespace Kulami
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; private set; }
        private enum AppState
        {
            MainMenu,
            OptionsMenu,
            Game,
            Credits
        }

        private AppState _state = AppState.MainMenu;
        private AppState _previousState = AppState.MainMenu;
        private bool _stateChanged = false;

        private AppState State
        {
            get => _state;
            set
            {
                _previousState = _state;
                _state = value;
                _stateChanged = true;
            }
        }

        private void Update()
        {
            HandleStateChanged();
        }

        private void HandleStateChanged()
        {
            if (_stateChanged)
            {
                _stateChanged = false;

                switch (State)
                {
                    case AppState.MainMenu:
                        break;
                    case AppState.OptionsMenu:
                        break;
                    case AppState.Game:
                        break;
                    case AppState.Credits:
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}

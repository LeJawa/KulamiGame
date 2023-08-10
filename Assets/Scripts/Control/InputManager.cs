using Kulami.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Control
{
    public abstract class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public abstract bool AnyInputDown { get; }
        public abstract Vector3 PrimaryCursorPosition { get; }
        public abstract float ZoomAmount { get; }

        public abstract bool GetTestDown();

        public abstract bool GetToggleGameOverScreenDown();
        public abstract bool GetToggleGameOverScreenUp();

        public abstract bool GetPrimaryCursorDown();

        public abstract bool GetPrimaryCursorUp();

        public abstract bool GetPrimaryCursor();


    }
}

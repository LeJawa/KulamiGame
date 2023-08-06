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

        public abstract bool AnyKeyDown { get; }
        public abstract Vector3 MousePosition { get; }
        public abstract float ZoomAmount { get; }

        public abstract bool GetKeyDown(KeyCode key);

        public abstract bool GetKeyUp(KeyCode key);

        public abstract bool GetMouseButtonDown(int button);

        public abstract bool GetMouseButtonUp(int button);

        public abstract bool GetMouseButton(int button);


    }
}

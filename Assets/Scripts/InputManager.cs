using Kulami.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public bool anyKeyDown => Input.anyKeyDown;
        public Vector3 mousePosition => Input.mousePosition;
        public Vector2 mouseScrollDelta => Input.mouseScrollDelta;

        public bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public bool GetKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }

        public bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }


    }
}

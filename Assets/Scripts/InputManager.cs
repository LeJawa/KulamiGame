using Kulami.Game;
using System;
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

        private bool _isDragging = false;
        private Vector3 _clickPosition;


        private void Update()
        {
            if (GetMouseButtonDown(0))
            {
                _clickPosition = MousePosition;
            }

            if (GetMouseButton(0))
            {
                if (!_isDragging && Vector3.Distance(_clickPosition, MousePosition) > 5f)
                {
                    _isDragging = true;
                }
            }

            if (GetMouseButtonUp(0))
            {
                StartCoroutine(IsNoLongerDragging());
            }
        }

        private IEnumerator IsNoLongerDragging() // Delay to ensure dragging clicks are not registered as normal clicks
        {
            yield return null;
            _isDragging = false;
        }


        public bool AnyKeyDown => Input.anyKeyDown;
        public Vector3 MousePosition => Input.mousePosition;
        public Vector2 MouseScrollDelta => Input.mouseScrollDelta;

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

        public bool IsCorrectClick => !_isDragging;
    }
}

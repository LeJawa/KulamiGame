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

<<<<<<< HEAD
        private bool _isDragging = false;
        private Vector3 _clickPosition;

        private float _zoomAmount = 0f;

        public bool IsCorrectClick => !_isDragging;

        public bool IsPinching { get => _isPinching; set => _isPinching = value; }
        public bool IsDragging
        {
            get => _isDragging; 
            set
            {
#if UNITY_ANDROID || UNITY_IOS
                if (IsPinching)
                {
                    return;
                }
#endif
                _isDragging = value;
            }
        }



        private void Update()
        {
            DetermineZoomAmount();

            if (GetMouseButtonDown(0))
            {
                _clickPosition = MousePosition;
            }

            if (GetMouseButton(0))
            {
                if (!IsDragging && Vector3.Distance(_clickPosition, MousePosition) > 5f)
                {
                    IsDragging = true;
                }
            }

            if (GetMouseButtonUp(0))
            {
                StartCoroutine(IsNoLongerDragging());
            }
        }

#if UNITY_ANDROID || UNITY_IOS
        private bool _isPinching = false;
        private Vector2 _pinchStart;
        private Vector2 _pinchEnd;
        private float _initialPinchDistance;
#endif

        private void DetermineZoomAmount()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            _zoomAmount = Input.mouseScrollDelta.y;
#endif
#if UNITY_ANDROID || UNITY_IOS
            if (!_isPinching && Input.touchCount > 1)
            {
                _pinchStart = Input.GetTouch(0).position;
                _pinchEnd = Input.GetTouch(1).position;
                _initialPinchDistance = Vector2.Distance(_pinchStart, _pinchEnd);
                _isPinching = true;
            }

            if (_isPinching)
            {
                if (Input.touchCount < 2)
                {
                    _isPinching = false;
                    _zoomAmount = 0f;
                    return;
                }

                var currentPinchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                _zoomAmount = currentPinchDistance / _initialPinchDistance;


            }
#endif
        }

        private IEnumerator IsNoLongerDragging() // Delay to ensure dragging clicks are not registered as normal clicks
        {
            yield return null;
            IsDragging = false;
        }

        public bool AnyKeyDown => Input.anyKeyDown;
        public Vector3 MousePosition => Input.mousePosition;
        public float ZoomAmount => _zoomAmount;
=======
        public bool anyKeyDown => Input.anyKeyDown;
        public Vector3 mousePosition => Input.mousePosition;
        public Vector2 mouseScrollDelta => Input.mouseScrollDelta;
>>>>>>> parent of 6ad512b (Fix accidental clicks when dragging)

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
<<<<<<< HEAD
=======


>>>>>>> parent of 6ad512b (Fix accidental clicks when dragging)
    }
}

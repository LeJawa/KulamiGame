using Cinemachine;
using Cinemachine.Utility;
using DG.Tweening;
using Kulami.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Compatibility;

namespace Kulami.Graphics
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraController2D : MonoBehaviour
    {
        public float _zoom;
        [SerializeField] private float _zoomMultiplier = 50f;
        [SerializeField] private float _zoomMin = 5f;
        [SerializeField] private float _zoomMax = 100f;
        private float _zoomVelocity = 0f;

        [SerializeField] private float _moveTime = 0.25f;
        [SerializeField] private float _boundary = 10f;
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _moveVelocity = Vector3.zero;

        private CinemachineVirtualCamera _camera;
        private Camera _mainCamera;

        private Vector3 _cameraPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;

        private Vector3 _anchorPosition;

        private bool _canZoom = true;

        void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _zoom = _camera.m_Lens.OrthographicSize;
            _targetPosition = transform.position;
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!GameManager.Instance.IsPlaying)
                return;

            if (_canZoom)
                HandleCameraZoom();

            if (InputManager.Instance.GetMouseButtonDown(0))
            {
                _anchorPosition = InputManager.Instance.MousePosition;
                _cameraPosition = transform.position;
                //_canZoom = false;
            }

            if (InputManager.Instance.GetMouseButton(0))
            {
                InputManager.Instance.IsDragging = true;
            }

            if (InputManager.Instance.GetMouseButtonUp(0))
            {
                _canZoom = true;
                InputManager.Instance.IsDragging = false;
            }

            if (InputManager.Instance.IsDragging)
            {
                _currentPosition = InputManager.Instance.MousePosition;
                LeftMouseDrag();
                if ((transform.position - _targetPosition).magnitude < 0.1f)
                {
                    transform.position = _targetPosition;
                }
            }

#if UNITY_EDITOR || UNITY_STANDALONE
            _camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize, _zoom, ref _zoomVelocity, _moveTime);
#endif
#if UNITY_ANDROID || UNITY_IOS
            _camera.m_Lens.OrthographicSize = _zoom;
#endif


            if (Mathf.Abs(_zoomVelocity) < 5e-5)
            {
                _camera.m_Lens.OrthographicSize = _zoom;
                transform.position = _targetPosition;
                _zoomVelocity = 0;
                _moveVelocity = Vector3.zero;
            }

            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _moveVelocity, _moveTime);

            var clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -_boundary, _boundary);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -_boundary, _boundary);

            transform.position = clampedPosition;            
        }

        private void LeftMouseDrag()
        {
            Vector3 direction = _mainCamera.ScreenToWorldPoint(_currentPosition) - _mainCamera.ScreenToWorldPoint(_anchorPosition);

            direction *= -1;

            _targetPosition = _cameraPosition + direction;
        }
#if UNITY_ANDROID || UNITY_IOS
        public bool IsZooming = false;
        private float _initialOrthographicSize;
#endif
        private void HandleCameraZoom()
        {
            float scroll = -InputManager.Instance.ZoomAmount;
            Vector3 mousePosition = InputManager.Instance.MousePosition;

            if (scroll != 0)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                _zoom += scroll * _zoomMultiplier;
                _zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax);

                if (_zoom != _zoomMin && _zoom != _zoomMax)
                {
                    _targetPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
                    _targetPosition.z = -10;
                }
#endif
#if UNITY_ANDROID || UNITY_IOS
                if (!IsZooming)
                {
                    IsZooming = true;
                    _initialOrthographicSize = _camera.m_Lens.OrthographicSize;
                }
                else
                {
                    _zoom = _initialOrthographicSize * scroll;
                    _zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax);
                }
#endif
            }
            else
            {
                #if UNITY_ANDROID || UNITY_IOS
                IsZooming = false;
                #endif
            }

        }
    }
}

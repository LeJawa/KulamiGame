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
    public class CameraZoom2D : MonoBehaviour
    {
        private float _zoom;
        [SerializeField] private float _zoomMultiplier = 50f;
        [SerializeField] private float _zoomMin = 5f;
        [SerializeField] private float _zoomMax = 100f;
        [SerializeField] private float _zoomTime = 0.25f;
        private float _zoomVelocity = 0f;

        [SerializeField] private float _moveTime = 0.25f;
        private Vector3 _startPosition = new Vector3(0, 0, -10);
        [SerializeField] private Vector3 _boundariesMax = new Vector3(10, 10, 0);
        [SerializeField] private Vector3 _boundariesMin = new Vector3(-10, -10, 0);
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _moveVelocity = Vector3.zero;

        private CinemachineVirtualCamera _camera;
        private Camera _mainCamera;

        private Vector3 _cameraPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;

        [SerializeField] private float _dragSpeed = 2f;

        [SerializeField]
        private bool _isDragging = false;
        private Vector3 _anchorPosition;

        void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _zoom = _camera.m_Lens.OrthographicSize;
            _position = _startPosition;
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (!GameManager.Instance.IsPlaying)
                return;

            //HandleCameraZoom();

            if (Input.GetMouseButtonDown(0))
            {
                _anchorPosition = Input.mousePosition;
                _cameraPosition = transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                _currentPosition = Input.mousePosition;
                LeftMouseDrag();
                _isDragging = true;
            }

            if (_isDragging)
            {
                //transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _dragSpeed);
                transform.position = Vector3.Lerp(transform.position, _targetPosition, _dragSpeed);
                if (transform.position == _targetPosition)
                    _isDragging = false;
            }




            //_camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize, _zoom, ref _zoomVelocity, _zoomTime);

            //transform.position = Vector3.SmoothDamp(transform.position, _position, ref _moveVelocity, _moveTime);
        }

        private void LeftMouseDrag()
        {
            // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
            // anyways.  
            Vector3 direction = Camera.main.ScreenToWorldPoint(_currentPosition) - Camera.main.ScreenToWorldPoint(_anchorPosition);

            // Invert direction to that terrain appears to move with the mouse.
            direction = direction * -1;

            _targetPosition = _cameraPosition + direction;
        }

        private void HandleCameraZoom()
        {
            float scroll = -Input.mouseScrollDelta.y;
            Vector3 mousePosition = Input.mousePosition;

            if (scroll != 0)
            {
                _zoom += scroll * _zoomMultiplier;
                _zoom = Mathf.Clamp(_zoom, _zoomMin, _zoomMax);

                if (_zoom != _zoomMin && _zoom != _zoomMax)
                {
                    _position = _mainCamera.ScreenToWorldPoint(mousePosition);
                    _position.z = -10;
                }
            }
            
            if (Mathf.Abs(_zoomVelocity) < 5e-5)
            {
                _camera.m_Lens.OrthographicSize = _zoom;
                transform.position = _position;
                _zoomVelocity = 0;
                _moveVelocity = Vector3.zero;
            }
            

        }
    }
}

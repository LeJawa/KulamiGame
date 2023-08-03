using Cinemachine;
using DG.Tweening;
using Kulami.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Vector3 _startPosition = new Vector3(0, 0, -10);
        private Vector3 _position;
        private Vector3 _moveVelocity = Vector3.zero;

        private CinemachineVirtualCamera _camera;
        private Camera _mainCamera;

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

            HandleCameraZoom();
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

            _camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize, _zoom, ref _zoomVelocity, _zoomTime);

            transform.position = Vector3.SmoothDamp(transform.position, _position, ref _moveVelocity, _zoomTime);
            
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

using Cinemachine;
using Kulami.Control;
using UnityEngine;

namespace Kulami.Graphics
{

    // Script adapted from https://pressstart.vip/tutorials/2018/07/12/44/pan--zoom.html

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PanZoom : MonoBehaviour
    {
        private Vector3 touchStart;
        [SerializeField] private float zoomOutMin = 1;
        [SerializeField] private float zoomOutMax = 8;
        private float _zoomTarget;
        [SerializeField] private float _zoomMobileMultiplier = 0.01f;
        [SerializeField] private float _zoomMouseMultiplier = 10f;
        private float _zoomVelocity = 0f;

        [SerializeField] private float _zoomTime = 0.25f;

        private CameraController _camera;
        private Camera _mainCamera;

        public bool IsIdle => !_isPanning && !_isZooming;

        private bool _isZooming = false;
        private bool _isPanning = false;
        [SerializeField]
        private float minDetectionDistance = 0.2f;

        private void Start()
        {
            _camera = GetComponent<CameraController>();

            _mainCamera = Camera.main;

            _zoomTarget = _camera.Zoom;
        }

        // Update is called once per frame
        private void Update()
        {
            if (InputManager.Instance.GetPrimaryCursorDown())
            {
                touchStart = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (InputManager.Instance.GetPrimaryCursor())
            {
                Vector3 direction = touchStart - _mainCamera.ScreenToWorldPoint(InputManager.Instance.PrimaryCursorPosition);

                if (direction.magnitude > minDetectionDistance)
                {
                    _camera.Position += direction;
                    _isPanning = true;
                }
                
            }
            else
            {
                _isPanning = false;
            }

#if UNITY_ANDROID || UNITY_IOS
            ZoomMobile();
#endif
#if UNITY_EDITOR || UNITY_STANDALONE 
            ZoomMouse();
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        private void ZoomMobile()
        {
            if (Input.touchCount > 1)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                _camera.Zoom = Mathf.Clamp(_camera.Zoom - difference * _zoomMobileMultiplier, zoomOutMin, zoomOutMax);

                _isZooming = true;
            }
            else
            {
                
                _isZooming = false;
            }
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        private void ZoomMouse()
        {
            var increment = Input.GetAxis("Mouse ScrollWheel") * _zoomMouseMultiplier;

            if (increment != 0)
            {
                _zoomTarget = Mathf.Clamp(_camera.Zoom - increment, zoomOutMin, zoomOutMax);
                _isZooming = true;
            }

            _camera.Zoom = Mathf.SmoothDamp(_camera.Zoom, _zoomTarget, ref _zoomVelocity, _zoomTime);

            if (Mathf.Abs(_camera.Zoom - _zoomTarget) < 0.01f)
            {
                _camera.Zoom = _zoomTarget;
                _zoomVelocity = 0f;
                _isZooming = false;
            }

        }
#endif
    }

}

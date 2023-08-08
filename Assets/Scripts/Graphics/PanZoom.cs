using Cinemachine;
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
        private float _zoom;
        [SerializeField] private float _zoomMobileMultiplier = 0.01f;
        [SerializeField] private float _zoomMouseMultiplier = 10f;
        private float _zoomVelocity = 0f;

        [SerializeField] private float _zoomTime = 0.25f;

        private CinemachineVirtualCamera _camera;
        private Camera _mainCamera;

        private void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _camera.transform.position += direction;
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
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                _camera.m_Lens.OrthographicSize = Mathf.Clamp(_camera.m_Lens.OrthographicSize - difference * _zoomMobileMultiplier, zoomOutMin, zoomOutMax);
            }
        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        private void ZoomMouse()
        {
            var increment = Input.GetAxis("Mouse ScrollWheel") * _zoomMouseMultiplier;

            if (increment != 0)
                _zoom = Mathf.Clamp(_camera.m_Lens.OrthographicSize - increment, zoomOutMin, zoomOutMax);

            _camera.m_Lens.OrthographicSize = Mathf.SmoothDamp(_camera.m_Lens.OrthographicSize, _zoom, ref _zoomVelocity, _zoomTime);

        }
#endif
    }

}

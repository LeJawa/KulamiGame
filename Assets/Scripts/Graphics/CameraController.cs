using Kulami.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Graphics
{
    public class CameraController : MonoBehaviour
    {
        public float panSpeed = 20f;
        public float zoomSpeed = 20f;
        public float zoomMin = 5f;
        public float zoomMax = 100f;
        public float panBorderPercentage = 0.1f;

        public Vector2 boundaries = new Vector2(5.5f, 5.5f);

        private float _panBorderThickness;

        private Camera _camera;

        void Start()
        {
            _camera = GetComponent<Camera>();
            _panBorderThickness = Mathf.Min(Screen.width, Screen.height) * panBorderPercentage;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.Instance.IsPlaying)
                return;

            HandleCameraZoom();
            //HandleCameraPanning();
        }

        private void HandleCameraPanning()
        {
            Vector3 panDirection = Vector3.zero;

            if (Input.mousePosition.x <= _panBorderThickness)
                panDirection += Vector3.left;
            else if (Input.mousePosition.x >= Screen.width - _panBorderThickness)
                panDirection += Vector3.right;

            if (Input.mousePosition.y <= _panBorderThickness)
                panDirection += Vector3.down;
            else if (Input.mousePosition.y >= Screen.height - _panBorderThickness)
                panDirection += Vector3.up;

            panDirection.Normalize();
            var normalizedPanSpeed = panSpeed / 10 * _camera.orthographicSize;

            transform.Translate(panDirection * normalizedPanSpeed * Time.deltaTime, Space.World);

            // Keep camera within boundaries
            var cameraPosition = transform.position;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, -boundaries.x, boundaries.x);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, -boundaries.y, boundaries.y);
            transform.position = cameraPosition;
        }

        private void HandleCameraZoom()
        {
            float scroll = -Input.mouseScrollDelta.y;

            if (scroll != 0)
            {
                float zoomAmount = scroll * zoomSpeed * Time.deltaTime;

                float initialZoom = _camera.orthographicSize;
                float zoom = initialZoom + zoomAmount;
                zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
                _camera.orthographicSize = zoom;

                if (!(zoom == zoomMin || zoom == zoomMax))
                {
                    Vector3 targetPosition;
                    float lerpAmount = 0.1f;

                    if (zoomAmount > 0)
                    {
                        targetPosition = new Vector3(0, 0, _camera.transform.position.z);

                        float remainingZoomDistance = zoomMax - initialZoom - 1; // - 1 in order to get to 0,0 before zooming out
                        float zoomDistanceMoved = zoom - initialZoom;
                        lerpAmount = zoomDistanceMoved / remainingZoomDistance;

                    }
                    else
                    {
                        var mousePosition = Input.mousePosition;
                        var worldPosition = _camera.ScreenToWorldPoint(mousePosition);
                        targetPosition = new Vector3(worldPosition.x, worldPosition.y, _camera.transform.position.z);
                    }

                    _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, lerpAmount);
                }
            }
        }
    }
}

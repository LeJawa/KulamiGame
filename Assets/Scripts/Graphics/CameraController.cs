using System;
using UnityEngine;

namespace Kulami.Graphics
{
    public class CameraController : MonoBehaviour
    {
        private PanZoom _panZoom;
        private Cinemachine.CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            _panZoom = GetComponent<PanZoom>();
        }

        public bool IsIdle => _panZoom.IsIdle;

        public Vector3 Position
        {
            get => _camera.transform.position;
            set => _camera.transform.position = value;
        }

        public void SetPosition(Vector2 position)
        {
            _camera.transform.position = new Vector3(position.x, position.y, _camera.transform.position.z);

        }

        public float Zoom
        {
            get => _camera.m_Lens.OrthographicSize;
            set => _camera.m_Lens.OrthographicSize = value;
        }
    }
}

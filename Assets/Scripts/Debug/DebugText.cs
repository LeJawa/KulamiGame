using Kulami.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Debug
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _text;

        [SerializeField] private CameraController2D _cameraController;


        private void Update()
        {
            _text.text = $"Zoom Amount: {InputManager.Instance.ZoomAmount}\n" +
                $"IsPinching: {InputManager.Instance.IsPinching}\n" +
                $"IsDragging: {InputManager.Instance.IsDragging}\n" +
                $"IsZooming: {_cameraController.IsZooming}\n" +
                $"_zoom: {_cameraController._zoom}";
        }
    }
}

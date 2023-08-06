using Kulami;
using Kulami.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kulami
{
    public class Cursor : MonoBehaviour
    {
        private Camera _camera;

        private float _boundary = 10;

        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            var cursorPosition = _camera.ScreenToWorldPoint(InputManager.Instance.MousePosition);
            cursorPosition.z = 0;
            cursorPosition = Vector3.ClampMagnitude(cursorPosition, _boundary);

            transform.position = cursorPosition;
        }
    }
}

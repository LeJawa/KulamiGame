using UnityEngine;

namespace Kulami.Control
{
    public class ComputerInputManager : InputManager
    {
        public override bool AnyKeyDown => Input.anyKeyDown;
        public override Vector3 MousePosition => Input.mousePosition;
        public override float ZoomAmount => Input.mouseScrollDelta.y;

        public override bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public override bool GetKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }

        public override bool GetMouseButtonDown(int button)
        {
            return Input.GetMouseButtonDown(button);
        }

        public override bool GetMouseButtonUp(int button)
        {
            return Input.GetMouseButtonUp(button);
        }

        public override bool GetMouseButton(int button)
        {
            return Input.GetMouseButton(button);
        }


    }
}

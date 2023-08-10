using UnityEngine;

namespace Kulami.Control
{
    public class ComputerInputManager : InputManager
    {
        public override bool AnyInputDown => Input.anyKeyDown;
        public override Vector3 PrimaryCursorPosition => Input.mousePosition;
        public override float ZoomAmount => Input.mouseScrollDelta.y;

        public override bool GetPrimaryCursorDown()
        {
            return Input.GetMouseButtonDown(0);
        }

        public override bool GetPrimaryCursorUp()
        {
            return Input.GetMouseButtonUp(0);
        }

        public override bool GetPrimaryCursor()
        {
            return Input.GetMouseButton(0);
        }

        public override bool GetTestDown()
        {
            return Input.GetKeyDown(KeyCode.T);
        }

        public override bool GetToggleGameOverScreenDown()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public override bool GetToggleGameOverScreenUp()
        {
            return Input.GetKeyUp(KeyCode.Space);
        }
    }
}

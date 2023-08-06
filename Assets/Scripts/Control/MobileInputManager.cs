using System;
using UnityEngine;

namespace Kulami.Control
{
    public class MobileInputManager : InputManager
    {
        public override bool AnyInputDown => throw new NotImplementedException();

        public override Vector3 PrimaryCursorPosition => throw new NotImplementedException();

        public override float ZoomAmount => throw new NotImplementedException();

        public override bool GetPrimaryCursor()
        {
            throw new NotImplementedException();
        }

        public override bool GetPrimaryCursorDown()
        {
            throw new NotImplementedException();
        }

        public override bool GetPrimaryCursorUp()
        {
            throw new NotImplementedException();
        }

        public override bool GetTestDown()
        {
            throw new NotImplementedException();
        }

        public override bool GetToggleGameOverScreenDown()
        {
            throw new NotImplementedException();
        }

        public override bool GetToggleGameOverScreenUp()
        {
            throw new NotImplementedException();
        }
    }
}

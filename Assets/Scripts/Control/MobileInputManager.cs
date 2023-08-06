using System;
using UnityEngine;

namespace Kulami.Control
{
    public class MobileInputManager : InputManager
    {
        public override bool AnyKeyDown => throw new NotImplementedException();

        public override Vector3 MousePosition => throw new NotImplementedException();

        public override float ZoomAmount => throw new NotImplementedException();

        public override bool GetKeyDown(KeyCode key)
        {
            throw new NotImplementedException();
        }

        public override bool GetKeyUp(KeyCode key)
        {
            throw new NotImplementedException();
        }

        public override bool GetMouseButton(int button)
        {
            throw new NotImplementedException();
        }

        public override bool GetMouseButtonDown(int button)
        {
            throw new NotImplementedException();
        }

        public override bool GetMouseButtonUp(int button)
        {
            throw new NotImplementedException();
        }
    }
}

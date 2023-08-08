using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kulami.Control
{
    public class MobileInputManager : InputManager
    {
        public override bool AnyInputDown => GetPrimaryCursorDown();

        private List<int> _currentTouchIds = new List<int>();


        private void Update()
        {

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (_currentTouchIds.Contains(Input.touches[i].fingerId))
                    {
                        if (Input.touches[i].phase == TouchPhase.Ended)
                        {
                            _currentTouchIds.Remove(Input.touches[i].fingerId);
                        }
                    }
                    else
                    {
                        // New touch
                        _currentTouchIds.Add(Input.touches[i].fingerId);

                        if (Input.touches[i].phase != TouchPhase.Began)
                        {
                            Debug.LogWarning("New touch is not in Began phase"); // Should not happen
                        }
                    }
                }
            }
        }

        private Touch? GetTouchWithId(int id)
        {
            if (Input.touchCount == 0) return null;

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].fingerId == id)
                {
                    return Input.touches[i];
                }
            }

            return null;
        }

        private Touch? GetPrimaryTouch()
        {
            if (_currentTouchIds.Count == 0) return null;

            return GetTouchWithId(_currentTouchIds[0]);
        }

        public override Vector3 PrimaryCursorPosition
        {
            get
            {
                if (GetPrimaryTouch() is Touch touch)
                {
                    return touch.position;
                }

                return Vector3.zero;
            }
        }

        public override float ZoomAmount
        {
            get
            {
                if (Input.touchCount < 2) return 0;

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                var totalDelta = (touchZero.deltaPosition + touchOne.deltaPosition).magnitude;

                return totalDelta;
            }
        }

        public override bool GetPrimaryCursor()
        {
            return _currentTouchIds.Count > 0;
        }

        public override bool GetPrimaryCursorDown()
        {
            return GetPrimaryTouch()?.phase == TouchPhase.Began;
        }

        public override bool GetPrimaryCursorUp()
        {
            return GetPrimaryTouch()?.phase == TouchPhase.Ended;
        }

        public override bool GetTestDown()
        {
            // No testing on mobile
            return false;
        }

        public override bool GetToggleGameOverScreenDown()
        {
            throw new NotImplementedException(); // TODO: Implement GetToggleGameOverScreenDown
        }

        public override bool GetToggleGameOverScreenUp()
        {
            throw new NotImplementedException(); // TODO: Implement GetToggleGameOverScreenUp
        }
    }
}

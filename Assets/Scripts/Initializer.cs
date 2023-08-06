using Kulami.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kulami
{
    public class Initializer : MonoBehaviour
    {
        void Start()
        {
            InitializeInput();
            Destroy(this);
        }

        private void InitializeInput()
        {
            var inputGameObject = new GameObject("InputManager");

#if UNITY_EDITOR || UNITY_STANDALONE
            inputGameObject.AddComponent<ComputerInputManager>();
#elif UNITY_ANDROID || UNITY_IOS
            inputGameObject.AddComponent<MobileInputManager>();
#endif
        }
    }
}

using Kulami.Control;
using UnityEngine;

namespace Kulami
{
    public class Initializer : MonoBehaviour
    {
        void Start()
        {
            InitializeInput();
            Destroy(gameObject);
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

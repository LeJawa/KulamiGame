using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Cursor : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Get the mouse position in world space
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Fix the mouse position to the grid
            mousePosition.x = Mathf.Round(mousePosition.x - 0.5f) + 0.5f;
            mousePosition.y = Mathf.Round(mousePosition.y - 0.5f) + 0.5f;

            // Set the cursor position
            transform.position = mousePosition;
        }
    }
}
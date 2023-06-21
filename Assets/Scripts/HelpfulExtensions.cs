using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class HelpfulExtensions
    {
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static int GetLeftMostX(this Vector2Int[] list)
        {
            int leftMostX = int.MaxValue;
            foreach (Vector2Int point in list)
            {
                if (point.x < leftMostX)
                    leftMostX = point.x;
            }

            return leftMostX;
        }

        public static int GetRightMostX(this Vector2Int[] list)
        {
            int rightMostX = int.MinValue;
            foreach (Vector2Int point in list)
            {
                if (point.x > rightMostX)
                    rightMostX = point.x;
            }

            return rightMostX;
        }

        public static int GetTopMostY(this Vector2Int[] list)
        {
            int topMostY = int.MinValue;
            foreach (Vector2Int point in list)
            {
                if (point.y > topMostY)
                    topMostY = point.y;
            }

            return topMostY;
        }

        public static int GetBottomMostY(this Vector2Int[] list)
        {
            int bottomMostY = int.MaxValue;
            foreach (Vector2Int point in list)
            {
                if (point.y < bottomMostY)
                    bottomMostY = point.y;
            }

            return bottomMostY;
        }

        public static Vector3 ToVector3(this Vector2Int vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        public static Player Switch(this Player player)
        {
            if (player == Player.One)
                return Player.Two;
            else
                return Player.One;
        }
    }
}
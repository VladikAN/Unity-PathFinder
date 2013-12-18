using UnityEngine;

namespace Assets.Script
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X ,0, Y);
        }
    }
}
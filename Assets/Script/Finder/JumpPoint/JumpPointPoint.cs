using UnityEngine;

namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointPoint : BasePoint
    {
        private int _step = 4;
        private double _cost;
        private readonly JumpPointPoint _parent;

        public JumpPointPoint() : base()
        {
        }

        public JumpPointPoint(int x, int y) : base(x, y)
        {
        }

        public JumpPointPoint(int x, int y, JumpPointPoint parent) : base(x, y)
        {
            _parent = parent;

            if (_parent != null)
            {
                if (X == parent.X)
                {
                    _cost = parent.Cost + Mathf.Abs(Mathf.Abs(Y) - Mathf.Abs(parent.Y));
                }
                else if (Y == parent.Y)
                {
                    _cost = parent.Cost + Mathf.Abs(Mathf.Abs(X) - Mathf.Abs(parent.X));
                }
                else
                {
                    _cost = parent.Cost + (Mathf.Abs(Mathf.Abs(X) - Mathf.Abs(parent.X)) * Mathf.Sqrt(2));
                }
            }
        }

        public int Step { get { return _step; } }
        public double Cost { get { return _cost; } }
        public JumpPointPoint Parent { get { return _parent; } }

        public bool FromLeft { get { return _step == 1 || _step == 4; } }
        public bool FromUp { get { return _step == 3 || _step == 4; } }

        public void NextStep()
        {
            if (_step != 0)
            {
                _step -= 1;
            }
        }
    }
}
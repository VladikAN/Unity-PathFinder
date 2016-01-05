using System;

namespace PathFinder2D.Core.Domain.Finder
{
    public class JumpPoint : FinderPoint
    {
        private int _step = 4;
        private readonly double _cost;
        private readonly JumpPoint _parent;

        public JumpPoint()
        {
        }

        public JumpPoint(int x, int y) : base(x, y)
        {
        }

        public JumpPoint(JumpPoint point, JumpPoint parent) : base(point.X, point.Y)
        {
            _parent = parent;
            _cost = parent == null ? 0 : parent.Cost + Math.Sqrt(Math.Pow(point.X - parent.X, 2) + Math.Pow(point.Y - parent.Y, 2));
        }

        public int Step { get { return _step; } }
        public double Cost { get { return _cost; } }
        public JumpPoint Parent { get { return _parent; } }

        public bool ToLeft { get { return _step == 3 || _step == 2; } }
        public bool ToUp { get { return _step == 4 || _step == 3; } }

        public void NextStep()
        {
            if (_step != 0)
            {
                _step -= 1;
            }
        }
    }
}
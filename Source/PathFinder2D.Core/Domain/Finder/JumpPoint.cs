namespace PathFinder2D.Core.Domain.Finder
{
    public class JumpPoint : FinderPoint
    {
        private int _step = 4;
        private double _cost;
        private readonly JumpPoint _parent;

        public JumpPoint() : base()
        {
        }

        public JumpPoint(int x, int y) : base(x, y)
        {
        }

        public JumpPoint(int x, int y, JumpPoint parent, double cost) : base(x, y)
        {
            _parent = parent;
            _cost = cost;
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
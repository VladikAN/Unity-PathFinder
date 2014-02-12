namespace Assets.Script.Finder.JumpPoint
{
    public class JumpPointPoint : BasePoint
    {
        private int _step = 4;
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
        }

        public int Step { get { return _step; } }
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
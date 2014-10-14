using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeTerrain : ITerrain
    {
        private readonly int _id;
        private readonly float _tX;
        private readonly float _tY;
        private readonly float _rX;
        private readonly float _rZ;

        public FakeTerrain(int id, float tX, float tY, float rX, float rZ)
        {
            _id = id;
            _tX = tX;
            _tY = tY;
            _rX = rX;
            _rZ = rZ;
        }

        public int Id()
        {
            return _id;
        }

        public float TransformX()
        {
            return _tX;
        }

        public float TransformZ()
        {
            return _tY;
        }

        public float RenderX()
        {
            return _rX;
        }

        public float RenderZ()
        {
            return _rZ;
        }
    }
}
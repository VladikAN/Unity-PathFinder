using System.Collections.Generic;
using PathFinder2D.Core.Domain.Terrain;

namespace PathFinder2D.UnitTests.Stubs
{
    public class FakeTerrain : ITerrain
    {
        private readonly int _id;
        private readonly float _x;
        private readonly float _y;
        private readonly float _w;
        private readonly float _h;
        private readonly float _cSize;

        public FakeTerrain(int id, float x, float y, float w, float h, float cSize)
        {
            _id = id;
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _cSize = cSize;
        }

        public int Id()
        {
            return _id;
        }

        public float X()
        {
            return _x;
        }

        public float Y()
        {
            return _y;
        }

        public float Width()
        {
            return _w;
        }

        public float Height()
        {
            return _h;
        }

        public float CellSize()
        {
            return _cSize;
        }

        public IEnumerable<IBlock> GetBlocks()
        {
            return null;
        }
    }
}
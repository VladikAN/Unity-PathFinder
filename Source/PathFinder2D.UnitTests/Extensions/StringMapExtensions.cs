using PathFinder2D.Core.Domain.Map;
using PathFinder2D.Core.Domain.Terrain;
using PathFinder2D.UnitTests.Stubs;

namespace PathFinder2D.UnitTests.Extensions
{
    public static class StringMapExtensions
    {
        public static ITerrain ParseTerrain(this string[] raw)
        {
            var terrain = new FakeTerrain(1, 0, 0, raw[0].Length, raw.Length, 1);
            return terrain;
        }

        public static MapCell[,] ParseMap(this string[] raw)
        {
            var width = raw[0].Length;
            var height = raw.Length;

            var field = new MapCell[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    field[i, j] = new MapCell { Blocked = raw[j][i] == '#' };
                }
            }

            return field;
        }

        public static MapDefinition ParseDefinition(this string[] raw)
        {
            var mapDefinition = new MapDefinition(raw.ParseTerrain(), raw.ParseMap(), 1);
            return mapDefinition;
        }
    }
}
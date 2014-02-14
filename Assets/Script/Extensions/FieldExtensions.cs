namespace Assets.Script.Extensions
{
    public static class FieldExtensions
    {
        public static bool[,] ToBoolMap(this Cell[,] field)
        {
            var width = PathFinderGlobal.TerrainFieldWidth;
            var height = PathFinderGlobal.TerrainFieldHeight;

            var result = new bool[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    result[i, j] = PathFinderGlobal.TerrainField[i, j].Blocked;
                }
            }

            return result;
        }
    }
}
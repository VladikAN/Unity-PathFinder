namespace PathFinder2D.Core.Domain.Terrain
{
    public interface ITerrain
    {
        int Id();

        float TransformX();
        float TransformZ();

        float RenderX();
        float RenderZ();
    }
}
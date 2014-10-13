using PathFinder2D.Core.Domain;
using PathFinder2D.Core.Finder;
using UnityEngine;

namespace PathFinder2D.Core
{
    public interface IPathFinderService
    {
        void RegisterFinder<TFinder>(TFinder instance) where TFinder : class, IFinder;
        TFinder ResolveFinder<TFinder>() where TFinder : class, IFinder;

        void RegisterGizmo<TGizmo>(TGizmo instance) where TGizmo : class, IGizmo;
        TGizmo ResolveGizmo<TGizmo>() where TGizmo : class, IGizmo;

        int InitMap(GameObject terrain, uint cellWidth);
        FinderResult Find<TFinder>(int terrainId, Vector3 start, Vector3 end) where TFinder : class, IFinder;
    }
}
using System.Collections.Generic;

namespace PathFinder2D.Core.Domain.Finder
{
    public class FinderResult
    {
        public IEnumerable<WorldPosition> Path;

        public FinderResult(IEnumerable<WorldPosition> path)
        {
            Path = path;
        }
    }
}
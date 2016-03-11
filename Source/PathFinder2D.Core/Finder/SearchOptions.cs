namespace PathFinder2D.Core.Finder
{
    public enum SearchOptions
    {
        /// <summary>Default search results</summary>
        None = 0,

        /// <summary>Minimized results. Neighbor steps are merged</summary>
        Minimum = 1,

        /// <summary>Maximum details. All steps are saved and added between gaps</summary>
        Maximum = 2
    }
}
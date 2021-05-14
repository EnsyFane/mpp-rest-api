using System.Collections.Generic;

namespace RestApi.Infrastructure.Mapper
{
    public interface IMappingCoordinator
    {
        TDest Map<TSource, TDest>(TSource source);

        IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> source);

        TDest Map<TSource, TDest>(TSource source, TDest dest);
    }
}
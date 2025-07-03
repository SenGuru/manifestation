using System.Collections.Generic;

namespace Manifest.Common.Interfaces.Service.ObjectMapping
{
    public interface IObjectMapperService
    {
        TResult Map<TSource, TResult>(TSource obj);

        TResult Map<TSource, TResult>(TSource src, TResult existing);

        List<TResult> MapList<TSource, TResult>(List<TSource> lst);
    }
}
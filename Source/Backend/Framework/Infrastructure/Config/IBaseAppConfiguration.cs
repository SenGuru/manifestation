/**
Copyright (c) 2016 Foundation.IO (https://github.com/foundationio). All rights reserved.

This work is licensed under the terms of the BSD license.
For a copy, see <https://opensource.org/licenses/BSD-3-Clause>.
**/
using System;
using Microsoft.Extensions.Caching.Distributed;

namespace Framework.Infrastructure.Config
{
    public interface IBaseAppConfiguration
    {
        string ApplicationVersion { get; }

        string DatabaseVersion { get; }

        bool EnableNewFeatures { get; }

        int CacheHighRefreshInMinutes { get; }

        int CachMediumRefreshInMinutes { get; }

        int CachLowRefreshInMinutes { get; }

        bool UseMiniProfiler { get; }

        bool ShowAllLinksWithoutPermissionCheck { get; }

        DistributedCacheEntryOptions CacheHighRefreshOption()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = CacheHighRefreshRelativeTime()
            };
        }

        TimeSpan CacheHighRefreshRelativeTime()
        {
            var time = CacheHighRefreshInMinutes;
            if (time == 0)
                time = 5;
            return new TimeSpan(0, time, 0);
        }

        DistributedCacheEntryOptions CachMediumRefreshOption()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = CachMediumRefreshRelativeTime()
            };
        }

        TimeSpan CachMediumRefreshRelativeTime()
        {
            var time = CachMediumRefreshInMinutes;
            if (time == 0)
                time = 15;

            return new TimeSpan(0, time, 0);
        }

        DistributedCacheEntryOptions CachLowRefreshOption()
        {
            return new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = CachLowRefreshRelativeTime()
            };
        }

        TimeSpan CachLowRefreshRelativeTime()
        {
            var time = CachLowRefreshInMinutes;
            if (time == 0)
                time = 60;

            return new TimeSpan(0, time, 0);
        }
    }
}

using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;

namespace TotalTech.Storage
{
    public static class CacheHelper
    {
        private static IBlobCache cache = Akavache.BlobCache.Secure;
        private static IBlobCache userCache = Akavache.BlobCache.UserAccount;

        public static IBlobCache Cache => cache;
        public static IBlobCache UserCache => userCache;

        public static bool IsCacheNull => Cache == null;
        public static bool IsUserCacheNull => UserCache == null;

        // deletes and then inserts an object in the cache
        public static async Task UpdateCache<T>(String key, T obj)
        {
            if (key == null || obj == null)
            {
                return;
            }

            try
            {
                await cache.Invalidate(key).FirstAsync().ToTask().ContinueWith(async (unit) =>
                {
                    await cache.InsertObject(key, obj)
                         .FirstAsync().ToTask().ConfigureAwait(false);

                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception on UpdateCache: { ex.Message }");
                //Debug.WriteLine("*** " + e.Message);
            }
        }

        // deletes and then inserts an object in the cache
        public static async Task UpdateUserCache<T>(String key, T obj)
        {

            if (key == null || obj == null)
            {
                return;
            }

            try
            {
                await userCache.Invalidate(key).FirstAsync().ToTask().ContinueWith(async (unit) =>
                {

                    await userCache.InsertObject(key, obj)
                         .FirstAsync().ToTask().ConfigureAwait(false);

                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception on UpdateUserCache: { ex.Message }");
                //Debug.WriteLine("*** " + e.Message);
            }
        }

        public static void InitializeCache()
        {
            cache = Akavache.BlobCache.Secure;
        }

        public static void InitializePersistentCache()
        {
            userCache = Akavache.BlobCache.UserAccount;
        }
    }
}
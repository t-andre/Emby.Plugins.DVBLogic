using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Cache
{
	/// <summary> Collection of caches. </summary>
	/// <typeparam name="T"> Generic type parameter. </typeparam>
	public class CacheCollection<T>
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the TSoft.TVServer.Cache.CacheCollection&lt;T&gt;
		/// class. </summary>
		/// <param name="duration"> The duration. </param>
		public CacheCollection(TimeSpan duration)
		{
			this._Duration = duration;
		}
		#endregion

		#region [Fields]

		/// <summary> The duration. </summary>
		private TimeSpan _Duration;
		#endregion

		#region [Public properties]

		/// <summary> Gets a list of caches. </summary>
		/// <value> A List of caches. </value>
		public ConcurrentDictionary<string, DataCache<T>> CacheList { get; } = new ConcurrentDictionary<string, DataCache<T>>(StringComparer.OrdinalIgnoreCase);
		#endregion

		#region [Public methods]

		/// <summary> Clears the cache described by key. </summary>
		/// <param name="key"> The key. </param>
		public void ClearCache(string key)
		{
			this.CacheList.TryRemove(key, out DataCache<T> cache);
		}

		/// <summary> Gets a cache. </summary>
		/// <param name="key"> The key. </param>
		/// <returns> The cache. </returns>
		public DataCache<T> GetCache(string key)
		{
			DataCache<T> cache = null;
			if (!string.IsNullOrWhiteSpace(key) && this.CacheList.TryGetValue(key, out cache))
			{
				cache.IsValid = (DateTime.UtcNow - cache.Date) < this._Duration;
			}
			return cache;
		}

		/// <summary> Sets a cache. </summary>
		/// <param name="key"> The key. </param>
		/// <param name="cache"> The cache. </param>
		/// <param name="list"> The list. </param>
		public void SetCache(string key, DataCache<T> cache, List<T> list)
		{
			cache = cache ?? new DataCache<T>();
			cache.Date = DateTime.UtcNow;
			cache.Data = list;
			this.CacheList.AddOrUpdate(key, cache, (k, v) => cache);
		}
		#endregion
	}
}

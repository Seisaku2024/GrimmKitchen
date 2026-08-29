using System;

namespace Arbor.Pool
{
	internal class ThreadSafeObjectPool<T> where T : class
	{
		private readonly object _Lock = new object();
		private UnityEngine.Pool.ObjectPool<T> m_Pool;

		public ThreadSafeObjectPool(
			Func<T> createFunc,
			Action<T> actionOnGet = null,
			Action<T> actionOnRelease = null,
			Action<T> actionOnDestroy = null,
			bool collectionCheck = true,
			int defaultCapacity = 10,
			int maxSize = 10000)
		{
			m_Pool = new UnityEngine.Pool.ObjectPool<T>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize);
		}

		public T Get()
		{
			lock (_Lock)
			{
				return m_Pool.Get();
			}
		}

		public void Release(T element)
		{
			lock (_Lock)
			{
				m_Pool.Release(element);
			}
		}
	}
}
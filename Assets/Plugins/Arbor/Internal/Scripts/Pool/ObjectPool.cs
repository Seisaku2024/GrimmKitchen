//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;

namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// クラス型インスタンスをプールする。
	/// </summary>
	/// <typeparam name="T">プールする型</typeparam>
#else
	/// <summary>
	/// Pool class type instances.
	/// </summary>
	/// <typeparam name="T">Pool type</typeparam>
#endif
	[Obsolete("use UnityEngine.Pool.ObjectPool")] // The minimum supported Unity version is now 6.0, so UnityEngine.Pool is recommended.
	public class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
	{
		private readonly UnityEngine.Pool.ObjectPool<T> m_Pool;

#if ARBOR_DOC_JA
		/// <summary>
		/// このプールから生成されたインスタンスの総数
		/// </summary>
#else
		/// <summary>
		/// Total number of instances generated from this pool
		/// </summary>
#endif
		public int CountAll
		{
			get
			{
				return m_Pool.CountAll;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このプールから取得され使用中のインスタンスの数
		/// </summary>
#else
		/// <summary>
		/// Number of instances in use retrieved from this pool
		/// </summary>
#endif
		public int CountActive
		{
			get
			{
				return m_Pool.CountActive;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このプールに格納されている未使用のインスタンスの数
		/// </summary>
#else
		/// <summary>
		/// Number of unused instances stored in this pool
		/// </summary>
#endif
		public int CountInactive
		{
			get
			{
				return m_Pool.CountInactive;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 新しいObjectPoolインスタンスを生成する
		/// </summary>
		/// <param name="createFunc">プールが空の時に新しいインスタンスを生成するためのファンクション</param>
		/// <param name="actionOnGet">インスタンスをプールから取り出した時に呼び出されるアクション</param>
		/// <param name="actionOnRelease">インスタンスがプールに戻されたときに呼び出されるアクション</param>
		/// <param name="actionOnDestroy">プールが最大サイズに達したためにインスタンスを破棄するために呼び出されるアクション</param>
		/// <param name="collectionCheck">インスタンスがプールに戻されたときに既にインスタンスが格納されているかどうかを確認するフラグ</param>
		/// <param name="defaultCapacity">スタックが作成されるデフォルトの容量</param>
		/// <param name="maxSize">プールの最大サイズ。プールが最大サイズに達すると、プールに戻そうとしたインスタンスは無視されガベージコレクションされるようになります。</param>
#else
		/// <summary>
		/// Creates a new ObjectPool instance.
		/// </summary>
		/// <param name="createFunc">Used to create a new instance when the pool is empty.</param>
		/// <param name="actionOnGet">Actions called when an instance is taken out of the pool</param>
		/// <param name="actionOnRelease">Action to be called when the instance is returned to the pool</param>
		/// <param name="actionOnDestroy">Action called to destroy an instance because the pool has reached its maximum size</param>
		/// <param name="collectionCheck">Flag to check if the instance is already stored when it is returned to the pool</param>
		/// <param name="defaultCapacity">The default capacity at which the stack is created</param>
		/// <param name="maxSize">Maximum size of the pool. When the pool reaches its maximum size, the instances you try to return to the pool will be ignored and garbage collected.</param>
#endif
		public ObjectPool(
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

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出す。
		/// </summary>
		/// <returns>取り出したインスタンス。</returns>
#else
		/// <summary>
		/// Fetch an instance from the pool.
		/// </summary>
		/// <returns>The retrieved instance.</returns>
#endif
		public T Get()
		{
			return m_Pool.Get();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出し、<see cref="PooledObject{T}"/>を返す。
		/// </summary>
		/// <param name="v">取り出したインスタンス。</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>usingステートメントに<see cref="PooledObject{T}"/>を使用することでスコープから出た際にプールへ返却されるようになる。</remarks>
#else
		/// <summary>
		/// Fetch an instance from the pool and return a <see cref="PooledObject{T}"/>.
		/// </summary>
		/// <param name="v">The retrieved instance.</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>By using <see cref = "PooledObject {T}" /> in the using statement, it will be returned to the pool when it goes out of scope.</remarks>
#endif
		public PooledObject<T> Get(out T v)
		{
			var pooledObject = m_Pool.Get(out v);
			return new PooledObject<T>(pooledObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールへインスタンスを返却する。
		/// </summary>
		/// <param name="element">返却するインスタンス</param>
#else
		/// <summary>
		/// Return the instance to the pool.
		/// </summary>
		/// <param name="element">Instance to return</param>
#endif
		public void Release(T element)
		{
			m_Pool.Release(element);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールしているインスタンスを全てクリアする。
		/// </summary>
#else
		/// <summary>
		/// Clear all pooled instances.
		/// </summary>
#endif
		public void Clear()
		{
			m_Pool.Clear();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールを廃棄する。
		/// </summary>
#else
		/// <summary>
		/// Discard the pool.
		/// </summary>
#endif
		public void Dispose()
		{
			Clear();
		}
	}
}
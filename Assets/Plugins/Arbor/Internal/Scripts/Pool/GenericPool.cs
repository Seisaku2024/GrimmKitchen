//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 汎用型のプール
	/// </summary>
	/// <typeparam name="T">プールする型</typeparam>
#else
	/// <summary>
	/// General purpose pool
	/// </summary>
	/// <typeparam name="T">Pool type</typeparam>
#endif
	[Obsolete("use UnityEngine.Pool.GenericPool")] // The minimum supported Unity version is now 6.0, so UnityEngine.Pool is recommended.
	public class GenericPool<T>
		where T : class, new()
	{
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
		public static T Get()
		{
			return UnityEngine.Pool.GenericPool<T>.Get();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出し、<see cref="PooledObject{T}"/>を返す。
		/// </summary>
		/// <param name="value">取り出したインスタンス。</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>usingステートメントに<see cref="PooledObject{T}"/>を使用することでスコープから出た際にプールへ返却されるようになる。</remarks>
#else
		/// <summary>
		/// Fetch an instance from the pool and return a <see cref="PooledObject{T}"/>.
		/// </summary>
		/// <param name="value">The retrieved instance.</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>By using <see cref = "PooledObject {T}" /> in the using statement, it will be returned to the pool when it goes out of scope.</remarks>
#endif
		public static PooledObject<T> Get(out T value)
		{
			var pooledObject = UnityEngine.Pool.GenericPool<T>.Get(out value);
			return new PooledObject<T>(pooledObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールへインスタンスを返却する。
		/// </summary>
		/// <param name="toRelease">返却するインスタンス</param>
#else
		/// <summary>
		/// Return the instance to the pool.
		/// </summary>
		/// <param name="toRelease">Instance to return</param>
#endif
		public static void Release(T toRelease)
		{
			UnityEngine.Pool.GenericPool<T>.Release(toRelease);
		}
	}
}
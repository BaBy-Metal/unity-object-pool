using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterLove.Collections
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ObjectPool<T>
	{
        /// <summary>
        /// 未使用对象列表
        /// </summary>
		private List<ObjectPoolContainer<T>> list;

        /// <summary>
        /// 已使用对象列表
        /// </summary>
		private Dictionary<T, ObjectPoolContainer<T>> lookup;

        /// <summary>
        /// 传递事件，通过回调函数获取到要调用的物体
        /// </summary>
		private Func<T> factoryFunc;

        /// <summary>
        /// 指针，指向列表里的物体
        /// </summary>
		private int lastIndex = 0;

		public ObjectPool(Func<T> factoryFunc, int initialSize)
		{
			this.factoryFunc = factoryFunc;

			list = new List<ObjectPoolContainer<T>>(initialSize);
			lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

			Warm(initialSize);
		}

		private void Warm(int capacity)
		{
			for (int i = 0; i < capacity; i++)
			{
				CreateConatiner();
			}
		}

        /// <summary>
        /// 开辟对象空间
        /// </summary>
        /// <returns></returns>
		private ObjectPoolContainer<T> CreateConatiner()
		{
			var container = new ObjectPoolContainer<T>();
			container.Item = factoryFunc();
			list.Add(container);
			return container;
		}

        /// <summary>
        /// 调用获取物体
        /// </summary>
        /// <returns></returns>
		public T GetItem()
		{
			ObjectPoolContainer<T> container = null;
			for (int i = 0; i < list.Count; i++)
			{
				lastIndex++;
				if (lastIndex > list.Count - 1) lastIndex = 0;
				
				if (list[lastIndex].Used)
				{
					continue;
				}
				else
				{
					container = list[lastIndex];
					break;
				}
			}

			if (container == null)
			{
				container = CreateConatiner();
			}

			container.Consume();
			lookup.Add(container.Item, container);
			return container.Item;
		}

        /// <summary>
        /// 关闭物体
        /// </summary>
        /// <param name="item">object类型参数</param>
		public void ReleaseItem(object item)
		{
            T tmp = (T)item;
			ReleaseItem(tmp);
		}

        /// <summary>
        /// 关闭物体
        /// </summary>
        /// <param name="item">泛型参数</param>
		public void ReleaseItem(T item)
		{
			if (lookup.ContainsKey(item))
			{
				var container = lookup[item];
				container.Release();
				lookup.Remove(item);
			}
			else
			{
				Debug.LogWarning("This object pool does not contain the item provided: " + item);
			}
		}

		public int Count
		{
			get { return list.Count; }
		}

		public int CountUsedItems
		{
			get { return lookup.Count; }
		}
	}
}


namespace MonsterLove.Collections
{
    /// <summary>
    /// 对象池容器
    /// 使用泛型 用来存放各种类型的单个物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ObjectPoolContainer<T>
	{
		private T item;

		public bool Used { get; private set; }

        /// <summary>
        /// 开启标签方法：
        /// 获取物体后调用 标明已被使用
        /// </summary>
		public void Consume()
		{
			Used = true;
		}

		public T Item
		{
			get
			{
				return item;
			}
			set
			{
				item = value;
			}
		}

        /// <summary>
        /// 释放标签方法：
        /// 调用后将使用标签改为否，该物体释放
        /// </summary>
		public void Release()
		{
			Used = false;
		}
	}
}

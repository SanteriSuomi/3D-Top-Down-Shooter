using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Utility
{
    public class GenericObjectPool<T> : GenericSingleton<GenericObjectPool<T>> where T : Component
    {
        [SerializeField]
        private T prefabToPool = default;
        [SerializeField]
        private int poolSize = 25;
        private Queue<T> pool;

        protected override void Awake()
        {
            base.Awake();
            InitializePools();
        }

        private void InitializePools()
        {
            pool = new Queue<T>(poolSize);
            // Put all the pooled objects under their respective parent.
            Transform parent = new GameObject($"{typeof(T).Name} Pool Objects").transform;
            // Instantiate, deactivate, parent and add the objects to the pool.
            for (var i = 0; i < poolSize; i++)
            {
                T pooledObject = Instantiate(prefabToPool);
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.SetParent(parent);
                pool.Enqueue(pooledObject);
            }
        }

        public T Dequeue()
        {
            // Get an object from the end of the queue and return it.
            T poppedObject = pool.Dequeue();
            poppedObject.gameObject.SetActive(true);
            return poppedObject;
        }

        public void Enqueue(T item)
        {
            // Put an object back to the end of the pool queue.
            T pushedObject = item;
            pushedObject.gameObject.SetActive(false);
            pool.Enqueue(pushedObject);
        }
    }
}
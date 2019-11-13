using System.Collections.Generic;
using UnityEngine;

namespace Shooter.Utility
{
    public class GenericObjectPool<T> : GenericSingleton<GenericObjectPool<T>> where T : MonoBehaviour
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
            GameObject parent = new GameObject($"{typeof(T).Name} Pool Objects");
            pool = new Queue<T>(poolSize);
            for (var i = 0; i < poolSize; i++)
            {
                T pooledObject = Instantiate(prefabToPool);
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.parent = parent.transform;
                pool.Enqueue(pooledObject);
            }
        }

        public T Dequeue()
        {
            T poppedObject = pool.Dequeue();
            poppedObject.gameObject.SetActive(true);
            return poppedObject;
        }

        public void Enqueue(T item)
        {
            T pushedObject = item;
            pushedObject.gameObject.SetActive(false);
            pool.Enqueue(pushedObject);
        }
    }
}
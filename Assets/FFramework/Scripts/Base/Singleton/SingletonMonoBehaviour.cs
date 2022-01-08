using UnityEngine;
namespace JKFramework
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance == null) Instance = this as T;
        }
    }
}
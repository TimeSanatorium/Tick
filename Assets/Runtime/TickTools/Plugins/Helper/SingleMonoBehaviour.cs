using UnityEngine;
/// <summary>
/// 场景单例 跨场景会删除
/// </summary>
namespace Tick{
public class SingleMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T current;
    public static T Current
    {
        get
        {
            if (current == null)
            {
                current = GameObject.FindObjectOfType<T>();
            }
            return current;
        }
        private set { }
    }

    /// <summary>
    /// 强行删除当前的单例
    /// </summary>
    public static void DestoryCurrentObject()
    {
        if (current != null)
        {
            Destroy(current.gameObject);
        }
    }
}
}
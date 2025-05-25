using UnityEngine;
/// <summary>
/// �������ؽű��������� Ĭ���ڼ��س�����ʱ�򲻻������������
/// </summary>
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
                //DontDestroyOnLoad(current.gameObject);
            }
            if (current == null)
            {
                throw new System.Exception($"�����в����ڹ���{typeof(T)}�ű��Ķ���");
            }
            return current;
        }
        private set { }
    }

    /// <summary>
    /// ɾ����ǰ��̬����
    /// </summary>
    public static void DestoryCurrentObject()
    {
        if (current != null)
        {
            Destroy(current.gameObject);
        }
    }
}
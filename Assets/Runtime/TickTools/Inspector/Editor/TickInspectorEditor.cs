using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Tick.Inspector
{
    //��֤�������Ƕ�MonoBehaviour��Ч ����֤����Ҳ��Ч
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class TickInspectorEditor : Editor
    {
        #region Button
        private Object m_ParameterInfo_1;
        public override void OnInspectorGUI()
        {
            // ����Ĭ�ϻ����߼�
            base.OnInspectorGUI();

            // ��ȡĿ�����
            MonoBehaviour targetObject = target as MonoBehaviour;
            //��ȡ���������ϵ����з���
            MethodInfo[] methods = targetObject.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)System.Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));
                if (buttonAttribute != null)
                {
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    switch (parameterInfos.Length)
                    {
                        case 0:
                            DrawButtonParameters_0(method,buttonAttribute,targetObject);
                            break;
                        case 1:
                            DrawButtonParameters_1(method,buttonAttribute,targetObject);
                            break;
                        default:
                            break;
                    }
                    
                }
            }
        }
        private void DrawButtonParameters_0(MethodInfo method,ButtonAttribute buttonAttribute,MonoBehaviour targte)
        {
            // ʹ�� Unity �� GUILayout ���ư�ť
            if (GUILayout.Button(buttonAttribute.ButtonName ?? method.Name))
            {
                // ���÷���
                method.Invoke(targte, null);
            }
        }
        private void DrawButtonParameters_1(MethodInfo method, ButtonAttribute buttonAttribute, MonoBehaviour targte)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            EditorGUILayout.BeginVertical();
            ParameterInfo parameterInfo = parameterInfos[0];
            m_ParameterInfo_1 = (GameObject)EditorGUILayout.ObjectField(parameterInfo.Name, m_ParameterInfo_1, parameterInfos[0].ParameterType, true);
            EditorGUILayout.EndVertical();
            if (GUILayout.Button(buttonAttribute.ButtonName ?? method.Name))
            {
                // ���÷���
                method.Invoke(targte, new object[] { m_ParameterInfo_1 });
            }
        }
        #endregion
    }
}
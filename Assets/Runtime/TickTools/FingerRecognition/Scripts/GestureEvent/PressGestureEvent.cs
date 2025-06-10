using System;
using UnityEngine;
namespace Tick
{ 
    public class PressGestureEvent : MonoBehaviour
    {
        private Action m_onPress;
        public Action OnPressEvent
        {
            get { return m_onPress; }
            set { m_onPress += value; }
        }
        private void OnEnable()
        {
            SingleFingerEventHandle.Current.OnPress += OnPress;
        }
        private void OnPress(GameObject go)
        {
            m_onPress?.Invoke();
        }
        private void OnDisable()
        {
            if (SingleFingerEventHandle.Current != null)
            {
                SingleFingerEventHandle.Current.OnPress -= OnPress;
            }
            
        }
    }
}
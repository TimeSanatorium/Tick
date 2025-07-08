using System;
using UnityEngine;
namespace Tick
{ 
    public class PressGestureEvent : MonoBehaviour
    {
        public bool IsRestrictionSingleFinger;
        public Action<FingerOperationData> OnPressEvent;

        private void OnEnable()
        {
            FingerEventHandle.Current.OnPress += OnPress;
        }
        private void OnPress(FingerOperationData data)
        {
            if (IsRestrictionSingleFinger && data.fingerId != 0)return;
            if (data.CheckCurrentDown != gameObject) return;
            OnPressEvent?.Invoke(data);
        }
        private void OnDisable()
        {
            if (FingerEventHandle.Current != null)
            {
                FingerEventHandle.Current.OnPress -= OnPress;
            }
            
        }
    }
}
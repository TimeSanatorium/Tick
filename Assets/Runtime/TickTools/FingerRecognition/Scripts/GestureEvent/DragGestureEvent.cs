using System;
using UnityEngine;
namespace Tick
{
    public class DragGestureEvent : MonoBehaviour
    {
        public bool IsRestrictionSingleFinger;
        public DragCheckMode _DragCheckMode;
        public float TriggerDistance;

        private DragState dragState;
        public Action<FingerOperationData> OnDragBeginningEvent;
        public Action<FingerOperationData> OnDraggingEvent;
        public Action<FingerOperationData> OnDragEndEvent;

        private int dragFingerID = -1;
        public void SetDragState(DragState dragState)
        {
            this.dragState = dragState;
        }
        private void OnEnable()
        {
            FingerEventHandle.Current.OnFingerHold += OnFingerHold;
            FingerEventHandle.Current.OnFingerUp += OnFingerUp;
        }
        private void OnFingerHold(FingerOperationData data)
        {
            if (IsRestrictionSingleFinger && data.fingerId != 0) return;
            if (data.CheckCurrentDown != gameObject) return;
            if (dragFingerID != -1 && data.fingerId != dragFingerID) return;
            switch (dragState)
            {
                case DragState.WaitDrag:
                    WaitDrag(data);
                    break;
                case DragState.Dragging:
                    Dragging(data);
                    break;
                default:
                    break;
            }
        }
        private void OnFingerUp(FingerOperationData data)
        {
            if (IsRestrictionSingleFinger && data.fingerId != 0) return;
            if (data.CheckCurrentDown != gameObject) return;
            if (dragFingerID != -1 && data.fingerId != dragFingerID) return;
            DragEnd(data);
        }
        private void WaitDrag(FingerOperationData data)
        {
            switch (_DragCheckMode)
            {
                case DragCheckMode.Vertical:
                    if (Mathf.Abs(data.FingerScreenDownPos.y - data.FingerScreenCurrenPos.y) >= TriggerDistance)
                    {
                        SetDragState(DragState.Dragging);
                        dragFingerID = data.fingerId;
                        OnDragBeginningEvent?.Invoke(data);
                        dragFingerID = data.fingerId;
                    }
                    break;
                case DragCheckMode.Horizontal:
                    if (Mathf.Abs(data.FingerScreenDownPos.x - data.FingerScreenCurrenPos.x) >= TriggerDistance)
                    {
                        SetDragState(DragState.Dragging);
                        OnDragBeginningEvent?.Invoke(data);
                        dragFingerID = data.fingerId;
                    }
                    break;
                case DragCheckMode.Total:
                    if (Vector3.Distance(data.FingerScreenDownPos,data.FingerScreenCurrenPos) >= TriggerDistance)
                    {
                        SetDragState(DragState.Dragging);
                        OnDragBeginningEvent?.Invoke(data);
                        dragFingerID = data.fingerId;
                    }
                    break;
                default:
                    break;
            }
        }
        private void Dragging(FingerOperationData data)
        {
            OnDraggingEvent?.Invoke(data);
        }
        private void DragEnd(FingerOperationData data)
        {
            if (dragState != DragState.Dragging) return;
            SetDragState(DragState.WaitDrag);
            dragFingerID = -1;
            OnDragEndEvent?.Invoke(data);
        }
        private void OnDisable()
        {
            if (FingerEventHandle.Current != null)
            {
                FingerEventHandle.Current.OnFingerHold -= OnFingerHold;
                FingerEventHandle.Current.OnFingerUp -= OnFingerUp;
            }
        }
    }
    public enum DragCheckMode
    {
        Vertical,
        Horizontal,
        Total
    }
    public enum DragState
    {
        WaitDrag,
        Dragging
    }
}

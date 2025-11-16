using System;
using UnityEngine.EventSystems;

public static class EventTriggerExtension
{
    public static EventTrigger.Entry AddListener(this EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener((eventData) => action.Invoke(eventData));
        trigger.triggers.Add(entry);
        return entry;
    }
    public static EventTrigger.Entry AddPointerListener(this EventTrigger trigger, EventTriggerType type, Action<PointerEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(eventData => action?.Invoke(eventData as PointerEventData));
        trigger.triggers.Add(entry);
        return entry;
    }
    public static EventTrigger.Entry AddAxisListener(this EventTrigger trigger, EventTriggerType type, Action<AxisEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(eventData => action?.Invoke(eventData as AxisEventData));
        trigger.triggers.Add(entry);
        return entry;
    }
    public static EventTrigger.Entry AddListener<T>(
            this EventTrigger trigger,
            EventTriggerType type,
            Action<T> action) where T : BaseEventData
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(eventData => action?.Invoke(eventData as T));
        trigger.triggers.Add(entry);
        return entry;
    }

    public static bool RemoveListener(this EventTrigger trigger, EventTrigger.Entry entry)
    {
        if (trigger.triggers.Contains(entry))
        {
            trigger.triggers.Remove(entry);
            return true;
        }
        return false;
    }

    #region PointerEventData
    public static EventTrigger.Entry AddPointerClickListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.PointerClick, action);
    public static EventTrigger.Entry AddPointerDownListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.PointerDown, action);
    public static EventTrigger.Entry AddPointerUpListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.PointerUp, action);
    public static EventTrigger.Entry AddPointerEnterListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.PointerEnter, action);
    public static EventTrigger.Entry AddPointerExitListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.PointerExit, action);
    public static EventTrigger.Entry AddBeginDragListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.BeginDrag, action);
    public static EventTrigger.Entry AddDragListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.Drag, action);
    public static EventTrigger.Entry AddEndDragListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.EndDrag, action);
    public static EventTrigger.Entry AddDropListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.Drop, action);
    public static EventTrigger.Entry AddScrollListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.Scroll, action);
    public static EventTrigger.Entry AddInitializePotentialDragListener(this EventTrigger trigger, Action<PointerEventData> action)
        => AddPointerListener(trigger, EventTriggerType.InitializePotentialDrag, action);
    #endregion

    #region BaseEventData
    public static EventTrigger.Entry AddUpdateSelectedListener(this EventTrigger trigger, Action<BaseEventData> action)
        => AddListener(trigger, EventTriggerType.UpdateSelected, action);
    public static EventTrigger.Entry AddSelectListener(this EventTrigger trigger, Action<BaseEventData> action)
        => AddListener(trigger, EventTriggerType.Select, action);
    public static EventTrigger.Entry AddDeselectListener(this EventTrigger trigger, Action<BaseEventData> action)
        => AddListener(trigger, EventTriggerType.Deselect, action);
    public static EventTrigger.Entry AddSubmitListener(this EventTrigger trigger, Action<BaseEventData> action)
        => AddListener(trigger, EventTriggerType.Submit, action);
    public static EventTrigger.Entry AddCancelListener(this EventTrigger trigger, Action<BaseEventData> action)
        => AddListener(trigger, EventTriggerType.Cancel, action);
    #endregion

    #region AxisEventData
    public static EventTrigger.Entry AddMoveListener(this EventTrigger trigger, Action<AxisEventData> action)
        => AddAxisListener(trigger, EventTriggerType.Move, action);
    #endregion
}

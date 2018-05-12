using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventList;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (EventManager.eventManager == null)
            {
                EventManager.eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (EventManager.eventManager == null)
                {
                    Debug.LogError("there is no EventManager in the scene");
                }
                else
                {
                    EventManager.eventManager.Initialize();
                }
            }

            return EventManager.eventManager;
        }
    }

    private void Initialize()
    {
        if (this.eventList == null)
        {
            this.eventList = new Dictionary<string, UnityEvent>();
        }
    }

    public static void Emit(string eventName)
    {
        UnityEvent thatEvent = null;

        if (EventManager.Instance.eventList.TryGetValue(eventName, out thatEvent))
        {
            thatEvent.Invoke();
        }
    }

    public static void On(string eventName, UnityAction listener)
    {
        UnityEvent thatEvent = null;

        if (EventManager.Instance.eventList.TryGetValue(eventName, out thatEvent))
        {
            // add another listener to an existing event
            thatEvent.AddListener(listener);
        }
        else
        {
            // add new event with listener
            thatEvent = new UnityEvent();
            thatEvent.AddListener(listener);
            EventManager.Instance.eventList.Add(eventName, thatEvent);
        }
    }

    public static void Once(string eventName, UnityAction listener)
    {
        throw new System.NotImplementedException();
    }

    public static void Off(string eventName, UnityAction listener)
    {
        if (EventManager.Instance.eventList == null) return;

        UnityEvent thatEvent = null;

        if (EventManager.Instance.eventList.TryGetValue(eventName, out thatEvent))
        {
            thatEvent.RemoveListener(listener);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : Singleton<Notification> {

    public delegate void ResponseEvent();
    public Dictionary<int, ResponseEvent> Events = new Dictionary<int, ResponseEvent>();

    public void RegisterNotification(int eventType, ResponseEvent eventFunc)
    {
        if(Events.ContainsKey(eventType))
        {
            Events[eventType] += eventFunc;
        }
        else
        {
            Events.Add(eventType, eventFunc);
        }
    }

    public void RemoveNotification(int eventType, ResponseEvent eventFunc)
    {
        if(Events.ContainsKey(eventType))
        {
            Events[eventType] -= eventFunc;
        }
        else
        {
            Debug.LogErrorFormat("RemoveEventFunc Error:  EventType {0} is not exist!", eventType);
        }
    }

    public void RemoveNotification(int eventType)
    {
        if (Events.ContainsKey(eventType))
            Events.Remove(eventType);
        else
            Debug.LogErrorFormat("RemoveNotification Error:  EventType {0} is not exist!", eventType);
    }

    public void RemoveNotification()
    {
        Debug.Log(" Remove All Notifications ! ");
        Events.Clear();
    }

    public void TriggerNotification(int eventType)
    {
        foreach (var item in Events)
        {
            if(item.Key==eventType)
            {
                item.Value();
            }
        }
    }
}

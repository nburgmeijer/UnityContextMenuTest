using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventSystem : MonoBehaviour
{
    public enum EventType
    {
        UIMouseClick
    }
    void SubScribe(EventType eventType)
    {

    }
    void UnSubscribe()
    {
        //TODO
    }

    private void Start()
    {
        ContextMenuItem.OnClickItem += ContextMenuItem_OnClickItem;
    }

    private void ContextMenuItem_OnClickItem(object sender, System.EventArgs e)
    {
        GameObject obj = (GameObject)sender;
        print("we clicked on" + obj.name);
    }
}

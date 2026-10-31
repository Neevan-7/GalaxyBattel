using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDebugger : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || EventSystem.current == null) return;

        var data = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, hits);

        if (hits.Count == 0) { Debug.Log("CLICK: nothing hit"); return; }

        string s = "CLICK hits (top first): ";
        foreach (var h in hits) s += h.gameObject.name + " > ";
        Debug.Log(s);
    }
}
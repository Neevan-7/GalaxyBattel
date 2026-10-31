
using UnityEngine;
using TMPro;

public class DropdownDebug : MonoBehaviour
{
    void Start()
    {
        TMP_Dropdown dropdown =
            GetComponent<TMP_Dropdown>();

        Debug.Log("OPTIONS COUNT: " +
                  dropdown.options.Count);

        for (int i = 0; i < dropdown.options.Count; i++)
        {
            Debug.Log("Option " + i + ": " +
                      dropdown.options[i].text);
        }
    }
}

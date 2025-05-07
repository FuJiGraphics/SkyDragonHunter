using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaycastBlockScanner : MonoBehaviour
{
    void Start()
    {
        var graphics = GetComponentsInChildren<Graphic>(true);
        foreach (var g in graphics)
        {
            if (g.raycastTarget)
            {
                Debug.LogWarning($"{g.name} 이 Raycast 막고 있음", g.gameObject);
            }
        }

        var tmps = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in tmps)
        {
            if (tmp.raycastTarget)
            {
                Debug.LogWarning($"{tmp.name} (TMP) 이 Raycast 막고 있음", tmp.gameObject);
            }
        }
    }
}

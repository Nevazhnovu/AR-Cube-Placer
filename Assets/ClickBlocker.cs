using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// This script blocks you from doing stuff while you click UI
/// </summary>
public class ClickBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public static bool isBlocked = false;
    private static Coroutine unblocker;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isBlocked = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unblocker != null) StopCoroutine(unblocker);
        unblocker = StartCoroutine(Unblock());
    }

    private IEnumerator Unblock()
    {
        yield return new WaitForEndOfFrame();
        isBlocked = false;
    }
}

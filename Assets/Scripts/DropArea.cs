using UnityEngine.EventSystems;
using UnityEngine;

public class DropArea :
	MonoBehaviour,
	IPointerEnterHandler,
	IPointerExitHandler
{
	public static Transform selected = null;

	public void OnPointerEnter(PointerEventData eventData)
	{
		selected = transform;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		selected = null;
	}
}

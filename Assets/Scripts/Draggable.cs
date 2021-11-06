using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Draggable :
	MonoBehaviour,
	IBeginDragHandler,
	IDragHandler,
	IEndDragHandler
{
	[SerializeField] TextMeshProUGUI titleTMP;
	private int siblingIndex;
	private Vector2 mouseDelta;

	[SerializeField] private Ghost ghostPrefab;
	private Ghost ghost;

	[SerializeField] private Color DRAG_COLOR;
	[SerializeField] private Color DEFAULT_COLOR;

	private Image image;
	private RectTransform rectTransform;

	private void Start()
	{
		image = GetComponent<Image>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		foreach (Draggable d in FindObjectsOfType<Draggable>())
		{
			d.SetRaycastTarget(false);
		}

		ghost = Instantiate(ghostPrefab);
		ghost.Init(transform, transform.GetSiblingIndex());

		transform.parent = FindObjectOfType<Canvas>().transform;
		mouseDelta = eventData.position - new Vector2(rectTransform.position.x, rectTransform.position.y);

		image.color = DRAG_COLOR;
	}

	public void OnDrag(PointerEventData eventData)
	{
		GetComponent<RectTransform>().position = eventData.position - mouseDelta;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		foreach (Draggable d in FindObjectsOfType<Draggable>())
		{
			d.SetRaycastTarget(true);
		}

		transform.parent = ghost.transform.parent;
		transform.SetSiblingIndex(ghost.transform.GetSiblingIndex());
		ghost.transform.SetAsLastSibling();
		ghost.gameObject.SetActive(false);
		Destroy(ghost.gameObject);

		image.color = DEFAULT_COLOR;
		ListController.UpdateListCounts();
	}

	public void SetTitle(string text)
	{
		titleTMP.SetText(text);
	}

	public string GetTitle()
	{
		return titleTMP.text;
	}

	private void SetRaycastTarget(bool on)
	{
		image.raycastTarget = on;
		titleTMP.raycastTarget = on;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	private const int GAP = 50;
	private Transform target;
	public void Init(Transform target, int initialSiblingIndex)
	{
		transform.SetParent(DropArea.selected, false);
		transform.SetSiblingIndex(initialSiblingIndex);
		this.target = target;
	}

	private void Update()
	{
		if (target == null)
		{
			return;
		}

		if (DropArea.selected != null && DropArea.selected != transform.parent)
		{
			transform.SetParent(DropArea.selected, false);
		}

		float difference = target.position.y - transform.position.y;
		if (Mathf.Abs(difference) > GAP)
		{
			int moveBy = (int)(difference) / GAP;
			if (difference > 0)  // Move Up
			{
				if (transform.GetSiblingIndex() - moveBy > 0)
				{
					transform.SetSiblingIndex(transform.GetSiblingIndex() - moveBy);
				}
				else
				{
					transform.SetAsFirstSibling();
				}
			}
			else                // Move down
			{
				if ((transform.GetSiblingIndex() + moveBy) < transform.parent.childCount - 1)
				{
					transform.SetSiblingIndex(transform.GetSiblingIndex() - moveBy);
				}
				else
				{
					transform.SetAsLastSibling();
				}
			}
		}
	}
}

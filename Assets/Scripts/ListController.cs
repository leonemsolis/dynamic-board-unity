using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

[Serializable]
public class AnimalsLists
{
	public List<AnimalsList> animal_lists;
}

[Serializable]
public class AnimalsList
{
	public string list_name;
	public List<string> content;
}

public class ListController : MonoBehaviour
{
	[SerializeField] private string listName;
	[SerializeField] private TextMeshProUGUI titleTMP;
	[SerializeField] private DropArea content;
	[SerializeField] private Draggable elementPrefab;

	public static void LoadLists()
	{
		string filePath = Application.streamingAssetsPath + "/animals.json";
		string json = File.ReadAllText(filePath);
		AnimalsLists animalLists = JsonUtility.FromJson<AnimalsLists>(json);
		foreach (AnimalsList al in animalLists.animal_lists)
		{
			foreach (ListController lc in FindObjectsOfType<ListController>())
			{
				if (lc.listName == al.list_name)
				{
					lc.InitList(al.content);
				}
			}
		}
	}

	public static void SaveLists()
	{
		string filePath = Application.streamingAssetsPath + "/animals.json";
		AnimalsLists lists = new AnimalsLists();
		lists.animal_lists = new List<AnimalsList>();

		foreach (ListController lc in FindObjectsOfType<ListController>())
		{
			AnimalsList list = new AnimalsList();
			list.list_name = lc.listName;
			list.content = new List<string>();

			for (int i = 0; i < lc.content.transform.childCount; i++)
			{
				Draggable d = lc.content.transform.GetChild(i).GetComponent<Draggable>();
				if (d != null)
				{
					list.content.Add(d.GetTitle());
				}
			}


			lists.animal_lists.Add(list);
		}

		string json = JsonUtility.ToJson(lists);
		File.WriteAllText(filePath, json);

	}

	public static void UpdateListCounts()
	{
		foreach (ListController lc in FindObjectsOfType<ListController>())
		{
			lc.UpdateCount();
		}
	}

	private void InitList(List<string> animals)
	{
		for (int i = content.transform.childCount - 1; i >= 0; i--)
		{
			content.transform.GetChild(i).gameObject.SetActive(false);
			Destroy(content.transform.GetChild(i).gameObject);
		}

		foreach (string animal in animals)
		{
			Draggable newElement = Instantiate(elementPrefab);
			newElement.SetTitle(animal);
			newElement.transform.SetParent(content.transform, false);
		}
		UpdateCount();
	}

	private void UpdateCount()
	{
		int counter = 0;
		for (int i = 0; i < content.transform.childCount; i++)
		{
			if (content.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				counter++;
			}
		}
		titleTMP.SetText(listName + "\n" + "Number of elements: " + counter);
	}
}

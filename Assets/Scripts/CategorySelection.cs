using UnityEngine;
using System.Collections;
using System;

public class CategorySelection : MonoBehaviour
{

    [SerializeField]
    private SelectionSlider selectionSlider;
	private Categories categories;

    // Use this for initialization
    void Start()
    {
        selectionSlider.OnBarFilled += OnChosenCategory;
		GameObject categoriesGameObject = GameObject.Find("CategoryGameObject");
		categories = categoriesGameObject.GetComponent<Categories>();
    }

    public void OnChosenCategory()
    {
		TextMesh textMesh = gameObject.GetComponentInChildren<TextMesh>();
		string category = textMesh.text;
        Debug.Log("Chosen Item:" + category);
		categories.loadSceneWithVideos(category);
    }
}

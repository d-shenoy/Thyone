using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MenuSelector : MonoBehaviour
{
    [SerializeField]
    private SelectionSlider selectionSlider;

	public  event Action OnMenuInvoked;

    // Use this for initialization
    void Start()
    {
        selectionSlider.OnBarFilled += OnMenuSelected;
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void OnMenuSelected()
	{
		//ScreenFader fader = gameObject.GetComponent<ScreenFader>();
        //fader.fadeIn = false;
        //SceneManager.LoadScene("Menu");
		if(OnMenuInvoked != null)
		{
			OnMenuInvoked();
		}
	}
}

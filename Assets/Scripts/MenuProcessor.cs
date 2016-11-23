using UnityEngine;
using System.Collections;
using System;

public class MenuProcessor : MonoBehaviour
{

    [SerializeField]
    private SelectionSlider selectionSlider;
    private LayersManipulation layermanipulator;
    public event Action exitPressed;
    public event Action browsePressed;
    public event Action faqPressed;
    public event Action searchPressed;

    // Use this for initialization
    void Start()
    {
        switch (gameObject.name)
        {
            case "Browse":
                Debug.Log("Menu Browse pressed");            
                selectionSlider.OnBarFilled += onBrowsePressed;
                break;
            case "Exit":
                selectionSlider.OnBarFilled += onExitPressed;
                break;
            case "Faq":
                selectionSlider.OnBarFilled += onFAQPressed;
                break;
            case "Search":
                selectionSlider.OnBarFilled += onSearchPressed;
                break;
        }
    }

    private void onExitPressed()
    {
        if (exitPressed != null)
        {
            exitPressed();
        }
    }

    private void onBrowsePressed()
    {
        if (browsePressed != null)
        {
            browsePressed();
        }
    }

    private void onFAQPressed()
    {
        if (faqPressed != null)
        {
            faqPressed();
        }
    }

    private void onSearchPressed()
    {
        if (searchPressed != null)
        {
            searchPressed();
        }
    }
}

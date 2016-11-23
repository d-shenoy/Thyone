using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// This class works similarly to the SelectionRadial class except
// it has a physical manifestation in the scene.  This can be
// either a UI slider or a mesh with the SlidingUV shader.  The
// functions as a bar that fills up whilst the user looks at it
// and holds down the Fire1 button.
public class SelectionSlider : MonoBehaviour
{
    public event Action OnBarFilled;                                    // This event is triggered when the bar finishes filling.


    [SerializeField]
    private float Duration = 2f;                     // The length of time it takes for the bar to fill.
    //[SerializeField]
    //private AudioSource Audio;                       // Reference to the audio source that will play effects when the user looks at it and when it fills.
    //[SerializeField]
    //private AudioClip OnOverClip;                    // The clip to play when the user looks at the bar.
    //[SerializeField]
    //private AudioClip OnFilledClip;                  // The clip to play when the bar finishes filling.
    [SerializeField]
    private Slider Slider;                           // Optional reference to the UI slider (unnecessary if using a standard Renderer).
    [SerializeField]
    private VrInteractiveItem InteractiveItem;       // Reference to the VRInteractiveItem to determine when to fill the bar.
    [SerializeField]
    //private VRInput VRInput;                         // Reference to the VRInput to detect button presses.
    //[SerializeField]
    private GameObject BarCanvas;                    // Optional reference to the GameObject that holds the slider (only necessary if DisappearOnBarFill is true).
    [SerializeField]
    private Renderer Renderer;                       // Optional reference to a renderer (unnecessary if using a UI slider).
    [SerializeField]
    //private SelectionRadial SelectionRadial;         // Optional reference to the SelectionRadial, if non-null the duration of the SelectionRadial will be used instead.
    //[SerializeField]
    //private UIFader UIFader;                         // Optional reference to a UIFader, used if the SelectionSlider needs to fade out.
    //[SerializeField]
    private Collider Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
    [SerializeField]
    private bool DisableOnBarFill;                   // Whether the bar should stop reacting once it's been filled (for single use bars).
    [SerializeField]
    private bool DisappearOnBarFill;                 // Whether the bar should disappear instantly once it's been filled.


    private bool BarFilled;                                           // Whether the bar is currently filled.
    private bool GazeOver;                                            // Whether the user is currently looking at the bar.
    private float Timer;                                              // Used to determine how much of the bar should be filled.
    private Coroutine FillBarRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.


    private const string k_SliderMaterialPropertyName = "_SliderValue"; // The name of the property on the SlidingUV shader that needs to be changed in order for it to fill.


    private void OnEnable()
    {
        //VRInput.OnDown += HandleDown;
        //VRInput.OnUp += HandleUp;

        InteractiveItem.OnOver += HandleOver;
        InteractiveItem.OnOut += HandleOut;
    }


    private void OnDisable()
    {
        //VRInput.OnDown -= HandleDown;
        //VRInput.OnUp -= HandleUp;

        InteractiveItem.OnOver -= HandleOver;
        InteractiveItem.OnOut -= HandleOut;
    }


    private void Update()
    {
        //if (!UIFader)
        //    return;

        // If this bar is using a UIFader turn off the collider when it's invisible.
        //Collider.enabled = UIFader.Visible;
    }


    public IEnumerator WaitForBarToFill()
    {
        // If the bar should disappear when it's filled, it needs to be visible now.
        if (BarCanvas && DisappearOnBarFill)
            BarCanvas.SetActive(true);

        // Currently the bar is unfilled.
        BarFilled = false;

        // Reset the timer and set the slider value as such.
        Timer = 0f;
        SetSliderValue(0f);

        // Keep coming back each frame until the bar is filled.
        while (!BarFilled)
        {
            yield return null;
        }

        // If the bar should disappear once it's filled, turn it off.
        if (BarCanvas && DisappearOnBarFill)
            BarCanvas.SetActive(false);
    }


    private IEnumerator FillBar()
    {
        // When the bar starts to fill, reset the timer.
        Timer = 0f;

        // The amount of time it takes to fill is either the duration set in the inspector, or the duration of the radial.
        float fillTime = Duration;

        // Until the timer is greater than the fill time...
        while (Timer < fillTime)
        {
            // ... add to the timer the difference between frames.
            Timer += Time.deltaTime;

            // Set the value of the slider or the UV based on the normalised time.
            SetSliderValue(Timer / fillTime);

            // Wait until next frame.
            yield return null;

            // If the user is still looking at the bar, go on to the next iteration of the loop.
            if (GazeOver)
                continue;

            // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
            Timer = 0f;
            SetSliderValue(0f);
            yield break;
        }

        // If the loop has finished the bar is now full.
        BarFilled = true;

        Debug.Log("Bar Filled");
        // If anything has subscribed to OnBarFilled call it now.
        if (OnBarFilled != null)
            OnBarFilled();

        // Play the clip for when the bar is filled.
        //Audio.clip = OnFilledClip;
        //Audio.Play();

        // If the bar should be disabled once it is filled, do so now.
        if (DisableOnBarFill)
            enabled = false;
    }


    private void SetSliderValue(float sliderValue)
    {
        // If there is a slider component set it's value to the given slider value.
        if (Slider)
            Slider.value = sliderValue;

        // If there is a renderer set the shader's property to the given slider value.
        if (Renderer)
            Renderer.sharedMaterial.SetFloat(k_SliderMaterialPropertyName, sliderValue);
    }


    private void HandleDown()
    {
        // If the user is looking at the bar start the FillBar coroutine and store a reference to it.
        if (GazeOver)
            FillBarRoutine = StartCoroutine(FillBar());
    }


    private void HandleUp()
    {
        // If the coroutine has been started (and thus we have a reference to it) stop it.
        if (FillBarRoutine != null)
            StopCoroutine(FillBarRoutine);

        // Reset the timer and bar values.
        Timer = 0f;
        SetSliderValue(0f);
    }


    private void HandleOver()
    {
        // The user is now looking at the bar.
        GazeOver = true;
		FillBarRoutine = StartCoroutine(FillBar());
        // Play the clip appropriate for when the user starts looking at the bar.
        //Audio.clip = OnOverClip;
        //Audio.Play();
    }


    private void HandleOut()
    {
        // The user is no longer looking at the bar.
        GazeOver = false;

        // If the coroutine has been started (and thus we have a reference to it) stop it.
        if (FillBarRoutine != null)
            StopCoroutine(FillBarRoutine);

        // Reset the timer and bar values.
        Timer = 0f;
        SetSliderValue(0f);
    }
}

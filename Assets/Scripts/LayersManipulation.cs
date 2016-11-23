using UnityEngine;
using System.Collections;

public class LayersManipulation : MonoBehaviour
{
    private Camera vrCamera;
    private Camera vrCameraLeft;
    private Camera vrCameraRight;

    void Awake()
    {
        vrCamera = GameObject.Find("GvrMain/Head/Main Camera").GetComponentInChildren<Camera>();
        vrCameraLeft = GameObject.Find("GvrMain/Head/Main Camera/Main Camera Left").GetComponentInChildren<Camera>();
        vrCameraRight = GameObject.Find("GvrMain/Head/Main Camera/Main Camera Right").GetComponentInChildren<Camera>();
    }

    // Turn on the bit using an OR operation:
    public void showLayer(string layer)
    {
        vrCamera.cullingMask |= 1 << LayerMask.NameToLayer(layer);
        vrCameraLeft.cullingMask |= 1 << LayerMask.NameToLayer(layer);
        vrCameraRight.cullingMask |= 1 << LayerMask.NameToLayer(layer);
    }

    // Turn off the bit using an AND operation with the complement of the shifted int:
    public void hideLayer(string layer)
    {
        vrCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        vrCameraLeft.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
        vrCameraRight.cullingMask &= ~(1 << LayerMask.NameToLayer(layer));
    }

    // Toggle the bit using a XOR operation:
    public void toggleLayer(string layer)
    {
        vrCamera.cullingMask ^= 1 << LayerMask.NameToLayer(layer);
        vrCameraLeft.cullingMask ^= 1 << LayerMask.NameToLayer(layer);
        vrCameraRight.cullingMask ^= 1 << LayerMask.NameToLayer(layer);
    }
}

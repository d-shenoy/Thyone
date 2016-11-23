using UnityEngine;
using System.Collections;
 
public class Circle : MonoBehaviour
{
    public int segments;
    public float xradius;
    public float yradius;
    LineRenderer line;

    int currentSegment = 0;
    float angle = 20.0f;

    void Start ()
    {
        VrInteractiveItem slider = gameObject.GetComponent<VrInteractiveItem>();
        slider.OnOver += drawStart;
        slider.OnOut += drawStop;
        line = gameObject.GetComponent<LineRenderer>();
        line.SetVertexCount (segments + 1);
        line.useWorldSpace = false;
        CreatePoints ();
    }
   
   public void drawStart()
   {
//       draw();
   }

   public void drawStop()
   {
       currentSegment = 0;
   }
   
    void CreatePoints ()
    {
        float x;
        float y;
        float z = 0f;
       
        float angle = 20f;
       
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
                   
            line.SetPosition (i,new Vector3(x,y,z) );
            angle += (360f / segments+1);
			//yield return new WaitForSeconds(1.0f);
        }
    }

    // IEnumerator draw()
    // {
    //     float x = y = z = 0;
    //     while(currentSegment < (segments +1))
    //     {
    //         currentSegment++; 

    //         x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
    //         y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

    //         line.SetPosition (i,new Vector3(x,y,z) );
    //         angle += (360f / segments+1);

    //         yield return currentSegment;
    //     }          
    // }
}
using System.Runtime.CompilerServices;
using UnityEngine;

public class BarLogic : MonoBehaviour
{
    public Renderer barColorRenderer;
    public GameObject ingotObject;

    private string forgeTag = "Forge";
    [SerializeField] bool inForge = false;
    private float heatIncrease = 0.05f;
    private float heatDecrease = 0.02f;
    public float barHeat = 0f;

  
    // Update is called once per frame
    void Update()
    {
        //check if inforge is false, which occurs when the bar is removed. 
        //Then start to reduce the barHeat float over time.
        if (!inForge)
        {
            barHeat -= Time.deltaTime * heatDecrease;
            barHeat = Mathf.Clamp(barHeat, 0f, 1f);
        }

        //Change the bar's color depending on the barheat- increasing how red it is specifically.
        Color color = Color.HSVToRGB(1f, barHeat, 1f);
        barColorRenderer.material.color = color;

        //Set the bar to not be in the forge on the frame it exits.
        inForge = false;

    }

    //As long as the bar is within the forge collider: 
    //Increase barheat over time and set inForge to true.
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(forgeTag))
        {
            barHeat += Time.deltaTime * heatIncrease;
            barHeat = Mathf.Clamp(barHeat, 0f, 1f);

            inForge = true;
        }
    }

}

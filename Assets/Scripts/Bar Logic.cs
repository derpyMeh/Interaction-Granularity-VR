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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!inForge)
        {
            barHeat -= Time.deltaTime * heatDecrease;
            barHeat = Mathf.Clamp(barHeat, 0f, 1f);
        }
        Color color = Color.HSVToRGB(1f, barHeat, 1f);

        barColorRenderer.material.color = color;

        inForge = false;

    }

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

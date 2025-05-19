using UnityEngine;

public class HammeringSecond : MonoBehaviour
{
    private string hammerTag = "Hammer";
    private string ingotTag = "Ingot";

    public AudioSource clinkAudio;

    private bool hammerInside = false;
    private bool ingotPlaced = false;
    private bool hasSwung = false;

    public GameObject chainObj;
    public GameObject ingotObj;
    public ParticleSystem sparkEffect;

    [SerializeField] BarLogic barLogic;
    [SerializeField] GameObject hammerObj;

    public Transform slotPosition;
    private Vector3 lastPosition;

    [SerializeField] int hitThresh;


    private float velocityThreshold = 5.0f;
    private float lastSwingTime = 0f;
    private float swingCd = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = hammerObj.transform.position;


    }

    // Update is called once per frame
    void Update()
    {
        if (barLogic == null)
        {
            barLogic = ingotObj.GetComponent<BarLogic>();
            ingotObj = GameObject.Find("The Bar");

            //if (ingotObj != null && ingotObj.activeInHierarchy)
            //{
            //    barLogic = ingotObj.GetComponent<BarLogic>();
            //}
        }

        
        Vector3 deltaPosition = hammerObj.transform.position - lastPosition;

        // Calculate the velocity (change in position over time)
        float velocity = deltaPosition.magnitude / Time.deltaTime;

        // Update the last position for the next frame
        lastPosition = hammerObj.transform.position;

        // Only trigger if velocity exceeds threshold
        if (!hasSwung && velocity >= velocityThreshold && barLogic.barHeat > 0.2 && ingotPlaced && hammerInside)
        {
            Debug.Log("Player Swung");
            hitThresh++;
            sparkEffect.Play();
            clinkAudio.Play();
            hasSwung = true;
            lastSwingTime = swingCd;

            if (hitThresh >= 5)
            {
                ForgeTest();
                hitThresh = 0;

            }
        }

        if (hasSwung)
        {
            lastSwingTime -= Time.deltaTime;
            if (lastSwingTime < 0f)
            {
                hasSwung = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ingotTag) && other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() != null)
        {
            PlaceInSlot(other.transform);
            Debug.Log("placed slot");
        }
        if (other.CompareTag(hammerTag))
        {
            hammerInside = true;
        }
        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = true;
            Debug.Log("Ingot Placed");
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hammerTag))
        {
            hammerInside = false;
        }

        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = false;
        }
    }

    private void PlaceInSlot(Transform objectTransform)
    {
        Rigidbody objectRigidbody = objectTransform.GetComponent<Rigidbody>();

        // Stop the object from moving freely once placed in the slot
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true; // Disable physics
        }

        // Snap the object into the slot position
        objectTransform.position = slotPosition.position;
        objectTransform.rotation = slotPosition.rotation; // Optional: align rotation as well

   
    }
    private void ForgeTest()
    {
        
        if (ingotPlaced && hammerInside)
        {
            chainObj.SetActive(true);
            Destroy(ingotObj);
            Debug.Log("Chain Spawned");
        }

    }
}

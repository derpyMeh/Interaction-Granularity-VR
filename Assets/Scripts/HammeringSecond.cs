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
        //If barLogic doesn't exist, look for it on the ingotObject
        if (barLogic == null)
        {
            barLogic = ingotObj.GetComponent<BarLogic>();
            ingotObj = GameObject.Find("The Bar");
        }

        
        Vector3 deltaPosition = hammerObj.transform.position - lastPosition;

        // Calculate the velocity (change in position over time)
        float velocity = deltaPosition.magnitude / Time.deltaTime;

        // Update the last position for the next frame
        lastPosition = hammerObj.transform.position;

        //Check if all swing conditions are met: 
        //player hasn't swung yet,
        //swing velocity is high enough,
        //the bar has enough heat,
        //the ingot is placed,
        //and the hammer is inside the trigger zone
        if (!hasSwung && velocity >= velocityThreshold && barLogic.barHeat > 0.2 && ingotPlaced && hammerInside)
        {
            //Increment hit counter
            hitThresh++;

            //Play clink audio and spark effect to emulate hammerstrike on metal
            sparkEffect.Play();
            clinkAudio.Play();

            //Set hasSwung to true, and start the swing cooldown
            hasSwung = true;
            lastSwingTime = swingCd;

            //If the play has correctly hit the bar 5 times, call the ForgeTest method.
            if (hitThresh >= 5)
            {
                ForgeTest();

            }
        }

        //If hasSwung is true, start the logic for the cooldown on a swing.
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
        //If the object has the ingot tag and is an XR Grab Interactable, place it in the slot.
        if (other.CompareTag(ingotTag) && other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() != null)
        {
            PlaceInSlot(other.transform);
        }

        //If the object is tagged as a hammer, mark that the hammer is inside the trigger zone
        if (other.CompareTag(hammerTag))
        {
            hammerInside = true;
        }

        //If the object has the Ingot tag, mark it as placed
        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = true;
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

        //Stop the object from moving freely once placed in the slot
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true; // Disable physics
        }

        //Snap the object into the slot position
        objectTransform.position = slotPosition.position;
    }
    private void ForgeTest()
    {

        //If ingot and hammer are inside, set the chain object as active and destroy the ingot object
        if (ingotPlaced && hammerInside)
        {
            chainObj.SetActive(true);
            Destroy(ingotObj);
        }

    }
}

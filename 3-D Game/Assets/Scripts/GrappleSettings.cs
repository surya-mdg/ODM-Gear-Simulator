using UnityEngine;
using UnityEngine.UI;

public class GrappleSettings : MonoBehaviour
{
    public LayerMask grappleableLayers;
    public PlayerMovement pm;
    public Transform cam;
    public Transform player;
    public Transform grappleCrosshairLeft;
    public Transform grappleCrosshairRight;
    public Transform grappleShootPointLeft;
    public Transform grappleShootPointRight;
    public Slider slider;
    public float maxDistance = 100f;
    public float grappleAngle = 2f;
    public float scrollFactor = 2f;
    public float minDistance = 0.01f;
    public float maxFuelTime = 1500f;
    [SerializeField] private float grappleForce = 100f;
    [SerializeField] private float gravity = 50f;

    [Header("Camera Settings")]
    [SerializeField] new Camera camera;
    [SerializeField] float fov;
    [SerializeField] float fovIncrease;
    [SerializeField] float fovTime;

    public Color aimColor;
    public Color hideColor;

    private GrapplingHook[] gh;

    private void Awake()
    {
        gh = GetComponentsInChildren<GrapplingHook>();
        slider.maxValue = maxFuelTime;
        slider.value = maxFuelTime;
        
    }

    private void Update()
    {
        if(gh[0].attached || gh[1].attached)
        {
            slider.value -= Time.deltaTime;
            pm.jumpGravityMultiplier = 8f;
        }
        else
        {
            pm.jumpGravityMultiplier = gravity;
        }

        if(slider.value<=0)
        {
            gh[0].canGrapple = false;
            gh[1].canGrapple = false;
        }

        if(gh[0].attached && gh[1].attached)
        {
            Vector3 mid = (gh[0].grapplePoint + gh[1].grapplePoint) / 2;
            Vector3 direction = (mid - player.position).normalized;
            float distance = Vector3.Distance(mid, player.position);
            if (distance > 3f)
            {
                player.GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<Rigidbody>().AddForce(grappleForce * direction, ForceMode.Acceleration);
            }
            else
            {
                player.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else if(gh[0].attached && !gh[1].attached)
        {
            Vector3 direction = (gh[0].grapplePoint - player.position).normalized;
            float distance = Vector3.Distance(gh[0].grapplePoint, player.position);
            if (distance > 3f)
            {
                player.GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<Rigidbody>().AddForce(grappleForce * direction, ForceMode.Acceleration);
            }
            else
            {
                player.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else if (gh[1].attached)
        {
            Vector3 direction = (gh[1].grapplePoint - player.position).normalized;
            float distance = Vector3.Distance(gh[1].grapplePoint, player.position);
            if (distance > 3f)
            {
                player.GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<Rigidbody>().AddForce(grappleForce * direction, ForceMode.Acceleration);
            }
            else
            {
                player.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            player.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}

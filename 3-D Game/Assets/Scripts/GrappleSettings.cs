using UnityEngine;

public class GrappleSettings : MonoBehaviour
{
    public LayerMask grappleableLayers;
    public Transform cam;
    public Transform player;
    public Transform grappleCrosshairLeft;
    public Transform grappleCrosshairRight;
    public Transform grappleShootPointLeft;
    public Transform grappleShootPointRight;
    public float maxDistance = 100f;
    public float grappleAngle = 2f;
    public float scrollFactor = 2f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LayerMask grappleableLayers;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform grappleShootPoint;
    [SerializeField] private Transform grappleCrosshair;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float grappleAngle = 2f;
    [SerializeField] private bool shootLeft = false;
    [SerializeField] private bool shootRight = false;

    private SpringJoint joint;
    private LineRenderer lr;
    private RaycastHit hit;
    private Vector3 grapplePoint;
    private Vector3 currentGrapplePosition;
    private bool grappleStatus = false;
    private bool grappling = false;
    public float scrollFactor = 2f;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        grappleAngle += Input.mouseScrollDelta.y * scrollFactor;
        grappleAngle = Mathf.Clamp(grappleAngle, 2f, 28f);

        if (Input.GetMouseButtonDown(0))
        {
            grappling = true;
            StartGrapple();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
            grappling = false;
        }

        if (!grappling)
        {
            CalculateCrosshair();
        }

        UpdateCrosshair();
    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    void StartGrapple()
    {
        if (shootRight)
        {
            grappleStatus = Physics.Raycast(cam.position, cam.forward + (cam.right / grappleAngle), out hit, maxDistance, grappleableLayers);
        }
        else if(shootLeft)
        {
            grappleStatus = Physics.Raycast(cam.position, cam.forward + (-cam.right / grappleAngle), out hit, maxDistance, grappleableLayers);
        }

        if(grappleStatus)
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromGrapple = Vector3.Distance(grapplePoint, player.position);

            joint.maxDistance = distanceFromGrapple * 0.8f;
            joint.minDistance = distanceFromGrapple * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = grappleShootPoint.position;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void DrawGrapple()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, grappleShootPoint.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void UpdateCrosshair()
    {
        if (!grappleStatus)
        {
            RectTransform trans= grappleCrosshair.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector3(0, 0, 0);
        }
        else
        {
            grappleCrosshair.position = Camera.main.WorldToScreenPoint(grapplePoint);
        }
    }

    private void CalculateCrosshair()
    {
        if (shootRight)
        {
            grappleStatus = Physics.Raycast(cam.position, cam.forward + (cam.right / grappleAngle), out hit, maxDistance, grappleableLayers);
        }
        else if (shootLeft)
        {
            grappleStatus = Physics.Raycast(cam.position, cam.forward + (-cam.right / grappleAngle), out hit, maxDistance, grappleableLayers);
        }

        if (grappleStatus)
        {
            Debug.Log("Working");
            grapplePoint = hit.point;
        }
    }
}

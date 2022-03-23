using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private LayerMask grappleableLayers;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform player;
    [SerializeField] private Transform grappleShootPoint;
    [SerializeField] private float maxDistance = 100f;

    private SpringJoint joint;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private Vector3 currentGrapplePosition;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position,cam.forward,out hit, maxDistance, grappleableLayers))
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
}

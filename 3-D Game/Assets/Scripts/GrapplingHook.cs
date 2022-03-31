using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private bool shootLeft = false;
    [SerializeField] private bool shootRight = false;

    private GrappleSettings gs;
    private SpringJoint joint;
    private LineRenderer lr;
    private RaycastHit hit;
    public Vector3 grapplePoint { get; private set; }
    private Vector3 currentGrapplePosition;
    private bool grappleStatus = false;
    private bool grappling = false;
    public bool attached = false;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        gs = GetComponentInParent<GrappleSettings>();
    }

    void Update()
    {
        gs.grappleAngle += Input.mouseScrollDelta.y * gs.scrollFactor;
        gs.grappleAngle = Mathf.Clamp(gs.grappleAngle, 2f, 28f);

        if (Input.GetMouseButtonDown(0))
        {
            grappling = true;
            StartGrapple();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
            grappling = false;
            attached = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            grappling = true;
            StartGrapplePush();
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
            grappling = false;
            attached = false;
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
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }
        else if(shootLeft)
        {
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (-gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }

        if(grappleStatus)
        {
            grapplePoint = hit.point;
            joint = gs.player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromGrapple = Vector3.Distance(grapplePoint, gs.player.position);

            joint.maxDistance = distanceFromGrapple * 0.8f;
            joint.minDistance = distanceFromGrapple * gs.minDistance;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            if (shootLeft)
            {
                currentGrapplePosition = gs.grappleShootPointLeft.position;
            }
            else
            {
                currentGrapplePosition = gs.grappleShootPointRight.position;
            }
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        if (joint != null)
        {
            Destroy(joint);
        }
    }

    void StartGrapplePush()
    {
        if (shootRight)
        {
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }
        else if (shootLeft)
        {
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (-gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }

        if (grappleStatus)
        {
            attached = true;
            grapplePoint = hit.point;
            

            float distanceFromGrapple = Vector3.Distance(grapplePoint, gs.player.position);


            lr.positionCount = 2;
            if (shootLeft)
            {
                currentGrapplePosition = gs.grappleShootPointLeft.position;
            }
            else
            {
                currentGrapplePosition = gs.grappleShootPointRight.position;
            }
        }
    }

    private void DrawGrapple()
    {
        if (!joint && !attached) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        if (shootLeft)
        {
            lr.SetPosition(0, gs.grappleShootPointLeft.position);
        }
        else
        {
            lr.SetPosition(0, gs.grappleShootPointRight.position);
        }
        lr.SetPosition(1, currentGrapplePosition);
    }

    private void UpdateCrosshair()
    {
        if (shootLeft)
        {
            Vector3 direction = (grapplePoint - gs.player.position).normalized;
            float angle = Vector3.Angle(direction, gs.cam.forward);
            if (!grappleStatus)
            {
                RectTransform trans = gs.grappleCrosshairLeft.GetComponent<RectTransform>();
                trans.anchoredPosition = new Vector3(0, 0, 0);
            }
            else
            {
                if (angle > 90f)
                {
                    RectTransform trans = gs.grappleCrosshairLeft.GetComponent<RectTransform>();
                    trans.anchoredPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    gs.grappleCrosshairLeft.position = Camera.main.WorldToScreenPoint(grapplePoint);
                }

            }
        }
        else
        {
            Vector3 direction = (grapplePoint - gs.player.position).normalized;
            float angle = Vector3.Angle(direction, gs.cam.forward);
            if (!grappleStatus)
            {
                RectTransform trans = gs.grappleCrosshairRight.GetComponent<RectTransform>();
                trans.anchoredPosition = new Vector3(0, 0, 0);
            }
            else
            {
                if (angle > 90f)
                {
                    RectTransform trans = gs.grappleCrosshairRight.GetComponent<RectTransform>();
                    trans.anchoredPosition = new Vector3(0, 0, 0);
                }
                else
                {
                    gs.grappleCrosshairRight.position = Camera.main.WorldToScreenPoint(grapplePoint);
                }
            }
        }
    }

    private void CalculateCrosshair()
    {
        if (shootRight)
        {
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }
        else if (shootLeft)
        {
            grappleStatus = Physics.Raycast(gs.cam.position, gs.cam.forward + (-gs.cam.right / gs.grappleAngle), out hit, gs.maxDistance, gs.grappleableLayers);
        }

        if (grappleStatus)
        {
            grapplePoint = hit.point;
        }
    }
}

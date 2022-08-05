using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    /*private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public GrapplingHook gh;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            lr.positionCount = 0;
        }
    }

    private void LateUpdate()
    {
        DrawGrapple();
    }

    private void DrawGrapple()
    {
        if (!gh.joint && !gh.attached) return;

        if (!gh.attached && gh.shootLeft)
        {
            currentGrapplePosition = gh.gs.grappleShootPointLeft.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }
        else if(!gh.attached && gh.shootRight)
        {
            currentGrapplePosition = gh.gs.grappleShootPointRight.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        if (gh.shootLeft)
        {
            var grapplePoint = gh.grapplePoint;
            var gunTipPosition = gh.gs.grappleShootPointLeft.position;
            var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, gh.grapplePoint, Time.deltaTime * 8f);

            for (var i = 0; i < quality + 1; i++)
            {
                var delta = i / (float)quality;
                var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                             affectCurve.Evaluate(delta);

                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }
        else if (gh.shootRight)
        {
            var grapplePoint = gh.grapplePoint;
            var gunTipPosition = gh.gs.grappleShootPointRight.position;
            var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, gh.grapplePoint, Time.deltaTime * 8f);

            for (var i = 0; i < quality + 1; i++)
            {
                var delta = i / (float)quality;
                var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                             affectCurve.Evaluate(delta);

                lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
            }
        }
    }*/
}

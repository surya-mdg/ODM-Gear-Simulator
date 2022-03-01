using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] bool enableBob;
    
    [Header("Bob Settings")]
    [SerializeField, Range(0, 0.01f)] float amplitude = 0.0005f;
    [SerializeField, Range(0, 30)] float frequency = 10f;
    [SerializeField, Range(0, 15)] float breathFrequency = 8f;
    [SerializeField, Range(0, 0.01f)] float breathAmplitude = 0.0005f;

    Transform _camera;
    Transform weapon;
    PlayerMovement playerMove;

    private Vector3 startPosition;
    private float sprintFrequency;

    private void Awake()
    {
        _camera = transform.Find("Main Camera");
        weapon = transform.Find("Weapon Holder");
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();

        startPosition = weapon.localPosition;
        sprintFrequency = 1.5f * frequency;
    }
    
    private void Update()
    {
        if (!enableBob) return;
        if (!playerMove.isGrounded || playerMove.wallRun.isWallRunning) return;

        if(Mathf.Abs(playerMove.rb.velocity.x) < 0.1f && Mathf.Abs(playerMove.rb.velocity.z) < 0.1f)
        {
            PlayMotion(Breathing());
        }
        else
        {
            PlayMotion(FootStepMotion());
        }
        
        ResetPosition();
        _camera.LookAt(FocusAt());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        if (!playerMove.isSprinting)
        {
            pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
            pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        }
        else
        {
            pos.y += Mathf.Sin(Time.time * sprintFrequency) * amplitude;
            pos.x += Mathf.Cos(Time.time * sprintFrequency / 2) * amplitude * 2;
        }
        
        return pos;
    }

    private Vector3 Breathing()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * breathFrequency) * breathAmplitude;

        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        weapon.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (weapon.localPosition == startPosition) return;

        weapon.localPosition = Vector3.Lerp(weapon.localPosition, startPosition, 1 * Time.deltaTime);
    }

    private Vector3 FocusAt()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * 15f;
        return pos;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [Header("Controls")]
    [SerializeField] KeyCode shoot = KeyCode.Mouse0;
    [SerializeField] KeyCode reload = KeyCode.R;

    private void Update()
    {
        if (Input.GetKey(shoot))
        {
            shootInput?.Invoke();
        }

        if (Input.GetKeyDown(reload))
        {
            reloadInput?.Invoke();
        }
    }
}

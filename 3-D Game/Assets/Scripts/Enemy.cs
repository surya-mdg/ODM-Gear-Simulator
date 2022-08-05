using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public MultiAimConstraint ik;
    public Transform blood;
    public bool die = false;

    private void Update()
    {
        if (die)
        {
            Kill();
            die = false;
        }
    }

    public void Kill()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        ik.weight = 0;
        Transform particles = Instantiate(blood, transform.position + new Vector3(0, 2.8f, 0), blood.rotation);
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(2.5f);
        Destroy(particles.gameObject);
        Destroy(this.gameObject);
    }
}

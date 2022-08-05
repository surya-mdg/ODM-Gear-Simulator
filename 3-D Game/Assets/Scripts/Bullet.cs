using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] private float damage = 10f;

    private Vector3 shootDir;

    private void Awake()
    {
        Invoke("Destruct", 5f);
    }

    void Update()
    {
        transform.position += shootDir * bulletSpeed * Time.deltaTime;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach(Collider i in colliders)
        {
            if(i.gameObject.tag=="Player")
            {
                PlayerHealth ph = i.GetComponent<PlayerHealth>();
                if (ph!=null)
                {
                    ph.Indicate();
                    ph.healthBar.value -= damage;
                }
            }
            if(i.gameObject.tag!="Bullet")
            {
                
                Destroy(gameObject);
            }
        }
    }

    public void Direction(Vector3 dir)
    {
        shootDir = dir;
    }

    public void Destruct()
    {
        Destroy(gameObject);
    }
}

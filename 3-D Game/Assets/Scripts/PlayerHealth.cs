using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public Image damage;

    [SerializeField] private float health = 10f;

    private void Awake()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void Indicate()
    {
        damage.gameObject.SetActive(true);
        StartCoroutine("Reset");
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.2f);
        damage.gameObject.SetActive(false);
    }
}

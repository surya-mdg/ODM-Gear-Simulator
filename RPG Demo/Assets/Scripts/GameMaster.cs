using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform player;
    public Transform enemySpawnPoint;
    public Transform playerSpawnPoint;

    public void Kill(GameObject body)
    {
        if (body.tag == "Enemy")
        {
            Destroy(body.gameObject);
            StartCoroutine("Respawn", false);
        }
        else
        {
            body.gameObject.SetActive(false);
            StartCoroutine("Respawn", true);
        }
    }

    IEnumerator Respawn(bool isPlayer)
    {
        yield return new WaitForSeconds(2f);

        if (isPlayer)
        {
            player.transform.position = playerSpawnPoint.position;
            player.GetComponent<PlayerDamage>().Respawn();
            player.gameObject.SetActive(true);
        }
        else
        {
            Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
        }
    }
}

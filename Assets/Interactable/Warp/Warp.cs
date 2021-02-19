using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : Interactable
{
    [Header("Warp to")] 
    public Transform target;
    [Header("Warp settings")]
    [Range(0.1f, 5f)] public float outSpeed = 4f;
    [Range(0f, 10f)] public float delay = 0.5f;
    public override void OnInteract(GameObject player)
    {
        
        StartCoroutine(WarpPlayer(player.transform));
    }

    private IEnumerator WarpPlayer(Transform player)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().detectCollisions = false;
        
        Player playerScript = player.GetComponent<Player>();

        float distance = Vector3.Distance(player.position, transform.position);
        float percent = 0;
        
        while (Vector3.Distance(player.position, transform.position) > 0.01f)
        {
            percent = Vector3.Distance(player.position, transform.position) / distance;
            player.position = Vector3.Lerp(player.position, transform.position, 5f * Time.deltaTime);
            playerScript.panel.color = new Color(0, 0, 0, 1-percent);
            float scale = Map(percent, 0f, 1f, 0.5f, 1f);
            player.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
        playerScript.panel.color = new Color(0, 0, 0, 1);
        yield return null;
        
        player.transform.position = target.position;
        player.transform.localScale = Vector3.one;
        
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Rigidbody>().detectCollisions = true;
        
        yield return new WaitForSeconds(delay);
        
        percent = 1f;
        while (true)
        {
            playerScript.panel.color = new Color(0, 0, 0, percent);
            if (percent < 0) break;
            percent -= Time.deltaTime * outSpeed;
            yield return null;
        }
        playerScript.panel.color = new Color(0, 0, 0, 0);
    }
    
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}

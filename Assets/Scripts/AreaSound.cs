using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int sfxIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFX(sfxIndex, null);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.StopSFXWith(sfxIndex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator anim;
    public string checkPointId;
    public bool isActive;
    private void Start()
    {
         anim = GetComponent<Animator>();
    }
    [ContextMenu("Generate checkpoint id")]
    private void generateId()
    {
        checkPointId = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            activateCheckPoint();
        }
    }

    public void activateCheckPoint()
    {
        if (!isActive)
            AudioManager.instance.PlaySFX(5, transform);
        anim.SetBool("Active", true);
        isActive = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void fadeOut() => anim.SetTrigger("fadeOut");
    public void fadeIn() => anim.SetTrigger("fadeIn");
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Zoom : MonoBehaviour
{
    Animator anim;
    UI ui;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ui = GetComponentInParent<UI>();
    }
    public void zoomIn() => anim.SetTrigger("zoomIn");
    public void zoomOut() => anim.SetTrigger("zoomOut");
    public void checkForZoomOutUI() => ui.checkForZoomOutUI(this.gameObject);
}

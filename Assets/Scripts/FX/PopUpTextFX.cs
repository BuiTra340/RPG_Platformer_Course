using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro myText;
    [SerializeField] private float speed;
    [SerializeField] private float disapearSpeed;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float lifeTime;
    private float textTimer;
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(transform.position.x, transform.position.y + 1, 0), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;
        if(textTimer < 0 )
        {
            float newAlpha = myText.color.a - colorLooseRate * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, newAlpha);
            speed = disapearSpeed;
        }
        if(myText.color.a <= 0)
            Destroy(gameObject);
    }
}

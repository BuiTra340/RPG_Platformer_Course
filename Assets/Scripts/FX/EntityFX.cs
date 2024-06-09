using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{

    [Header("Shake Screen")]
    private CinemachineImpulseSource cinemachine;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material flashMaterial;

    [Header("Ailment color")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] lightingColor;

    [Header("Ailment Particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillyFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private GameObject hitFxCriticalPrefab;

    [Space]
    [SerializeField] private ParticleSystem dustFX;

    [Header("After Image FX")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private int amoutOfAfterImage;
    [SerializeField] private Transform afterImageParent;
    private List<GameObject> afterImageList = new List<GameObject>();
    private float afterImageCooldownTimer;

    private Material originalMat;
    SpriteRenderer sprite;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalMat = sprite.material;
        player = PlayerManager.instance.player;
        cinemachine = GetComponentInParent<CinemachineImpulseSource>();
        pushAfterImageToPool();
    }
    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    private void pushAfterImageToPool()
    {
        for(int i=0;i<amoutOfAfterImage;i++)
        {
            GameObject newAfterImage = Instantiate(afterImagePrefab, afterImageParent);
            afterImageList.Add(newAfterImage);
            newAfterImage.SetActive(false);
        }
    }
    public void getAfterImageFromPool()
    {
        if (afterImageCooldownTimer > 0)
            return;
        afterImageCooldownTimer = afterImageCooldown;
        for(int i=0;i<afterImageList.Count;i++)
        {
            if (!afterImageList[i].activeInHierarchy)
            {
                afterImageList[i].SetActive(true);
                afterImageList[i].transform.position = player.transform.position;
                afterImageList[i].transform.rotation = player.transform.rotation;
                return;
            }
        }
    }
    public void createShakeScreen(Vector3 _shakePower)
    {
        cinemachine.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        cinemachine.GenerateImpulse();
    }
    public void makeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
        {
            sprite.color = Color.clear;
        }
        else
        {
            GetComponentInParent<CharacterStat>().makeInvincible(false);
            sprite.color = Color.white;
        }
    }
    private IEnumerator FlashFX()
    {
        sprite.material = flashMaterial;
        Color currentColor = sprite.color;
        sprite.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        sprite.color = currentColor;
        sprite.material = originalMat;
    }
    private void redColorBlink()
    {
        if(sprite.color != Color.white)
            sprite.color = Color.white;
        else sprite.color = Color.red;
    }
    private void cancelColorChange()
    {
        CancelInvoke();
        sprite.color = Color.white;

        igniteFX.Stop();
        chillyFX.Stop();
        shockFX.Stop();
    }
    public void igniteFxFor(float _duration)
    {
        igniteFX.Play();
        InvokeRepeating("igniteColorFx",0, .3f);
        Invoke("cancelColorChange", _duration);
    }
    private void igniteColorFx()
    {
        if (sprite.color != igniteColor[0])
            sprite.color = igniteColor[0];
        else sprite.color = igniteColor[1];
    }
    public void chillFxFor(float _duration)
    {
        chillyFX.Play();
        InvokeRepeating("chillColorFx", 0, .3f);
        Invoke("cancelColorChange", _duration);
    }
    private void chillColorFx()
    {
        if (sprite.color != chillColor[0])
            sprite.color = chillColor[0];
        else sprite.color = chillColor[1];
    }
    public void lightingFxFor(float _duration)
    {
        shockFX.Play();
        InvokeRepeating("lightingColorFx", 0, .3f);
        Invoke("cancelColorChange", _duration);
    }
    private void lightingColorFx()
    {
        if (sprite.color != lightingColor[0])
            sprite.color = lightingColor[0];
        else sprite.color = lightingColor[1];
    }
    public void createHitFX(Transform _target,bool _critical)
    {
        float zRotation = Random.Range(-90f, 90f);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        float yRotation = 0;
        GameObject hitPrefab = hitFxPrefab;
        if(_critical)
        {
            hitPrefab = hitFxCriticalPrefab;
            if (GetComponentInParent<Entity>().facingDir == -1)
                yRotation = 180;
            zRotation = Random.Range(-40f, 40f);
        }
        GameObject newHitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPosition,yPosition),Quaternion.identity);
        newHitFX.transform.Rotate(0,yRotation,zRotation);
        Destroy(newHitFX, .5f);
    }
    public void createDust()
    {
        if (dustFX != null)
            dustFX.Play();
    }
    public void createPopUpText(string _text)
    {
        float xOffSet = Random.Range(-1f, 1f);
        float yOffSet = Random.Range(1f, 3f);
        GameObject newPopUp = Instantiate(popUpTextPrefab,new Vector3(transform.position.x + xOffSet,transform.position.y + yOffSet),Quaternion.identity);
        newPopUp.GetComponent<TextMeshPro>().text = _text;
    }
}

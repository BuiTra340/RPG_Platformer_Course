using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] sfx;
    public bool playBGM;
    public int bgmIndex;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
    }
    private void Update()
    {
        if(!playBGM)
            StopAllBGM();
        else
        {
            if(!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }
    public void PlaySFX(int _sfxIndex,Transform _source)
    {

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;
        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }
    public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();
    public void StopSFXWith(int _index) => StartCoroutine(decreaseVolume(sfx[_index]));
    private IEnumerator decreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;
        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);
            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }
    public void playRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        bgm[_bgmIndex].Play();
    }

    private void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}

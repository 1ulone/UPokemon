using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instances;

    [System.Serializable]
    public class sfx
    {
        public string tag;
        public AudioClip sfxClip;
    }

    public AudioSource sfxSource,
                       musicSource;
     
    public AudioClip[] musicList;
    public List<sfx> sfxList;

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    void Start()
    {
        instances = this;
        ChangeMusic(0);
        musicSource.Play();

        foreach(sfx s in sfxList)
            sfxDictionary.Add(s.tag.ToUpper(), s.sfxClip);
    }

    public void PlaySfx(string tag)
    {
        if (!sfxDictionary.ContainsKey(tag.ToUpper()))
            return;

        sfxSource.PlayOneShot(sfxDictionary[tag.ToUpper()]);
    }

    public void ChangeMusic(int musId)
    {
        if (musicSource.clip != musicList[musId])
		musicSource.clip = musicList[musId];
    }

}

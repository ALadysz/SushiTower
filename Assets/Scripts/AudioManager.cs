using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for playing sound effects
public class AudioManager : MonoBehaviour
{
    //class so only one function needs to be called to play a sound
    [System.Serializable]
    public struct ClassedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Audio Clips")]
    [SerializeField] private ClassedAudioClip[] audioClips;
    private Dictionary<string, AudioClip> audioClipDict;
    private AudioSource audioSource;

    void Awake()
    {
        //make sure we can actually play audio clips
        audioSource = gameObject.AddComponent<AudioSource>();
        //create dictionary
        audioClipDict = new Dictionary<string, AudioClip>();
        //fill dictionary with our audioclips
        foreach (ClassedAudioClip namedClip in audioClips)
        {
            audioClipDict[namedClip.name] = namedClip.clip;
        }
    }

    public void PlayAudioClip(string clipName)
    {
        //get audio clip if it exists within dictionary
        if (audioClipDict.ContainsKey(clipName))
        {
            audioSource.clip = audioClipDict[clipName];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }
}

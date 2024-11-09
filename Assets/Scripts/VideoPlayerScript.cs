using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    public VideoPlayer VideoSource;
    public VideoClip background;

    void Start()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        if (VideoSource != null)
        {
            VideoSource.clip = background;
            VideoSource.isLooping = true;
            VideoSource.Play();
        }
    }

    public void StopVideo()
    {
        if (VideoSource != null)
        {
            VideoSource.Stop();
        }
    }
}

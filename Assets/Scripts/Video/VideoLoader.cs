using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoLoader : MonoBehaviour {

    [HideInInspector]
    public static VideoClip currentVideo;

    public void LoadClip(VideoClip video)
    {
        currentVideo = video;
        SceneManager.LoadScene("VideoPlayer");
    }
}

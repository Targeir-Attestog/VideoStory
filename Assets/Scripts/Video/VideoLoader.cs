using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoLoader : MonoBehaviour {

    [HideInInspector]
    public static VideoClip currentVideo;

    public static void Load(VideoClip video)
    {
        currentVideo = video;
        SceneManager.LoadScene("VideoPlayer");
    }

    public static void LoadString(string video)
    {
        //currentVideo = video;
        SceneManager.LoadScene("VideoPlayer");
    }
}

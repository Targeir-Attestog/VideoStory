using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using SubtitlesParser.Classes;

public class VideoController : MonoBehaviour {

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;    

    private GameObject subtitles;
    private UnityEngine.UI.Text subtitlesTextGameObject;
    private List<SubtitleItem> subtitleItems;

    public string sceneAfterPlayback;

    // Use this for initialization
    void Start () {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        subtitles = GameObject.FindGameObjectWithTag("Subtitles");
        subtitlesTextGameObject = subtitles.GetComponent<UnityEngine.UI.Text>();

        if (subtitlesTextGameObject != null)  // If there is a subtitles text game object
        {
            subtitlesTextGameObject.text = string.Empty;
        }

        if (VideoLoader.currentVideo != null)
        {
            StartVideoPlayback(VideoLoader.currentVideo);
        }
                
    }
	
	// Update is called once per frame
	void Update () {
        // If there is a subtitles text game object and subtitle items are available for filling it.
        if (subtitlesTextGameObject != null && subtitleItems != null) 
        {
            foreach(var subItem in subtitleItems)
            {
                // Show subtitle items at the predefined times. (TODO: May be optimized.)
                // VideoPlayer measures time in seconds as a double float and subtitle (srt) file in milliseconds as an integer

                int currentVideoTimeInMilliseconds = (int)(videoPlayer.time * 1000);

                if (currentVideoTimeInMilliseconds >= subItem.StartTime && currentVideoTimeInMilliseconds <= subItem.EndTime)
                {
                    string subtitlesToShow = string.Empty;

                    foreach(string line in subItem.Lines)
                    {
                        subtitlesToShow += line + "\n";
                    }

                    subtitlesTextGameObject.text = subtitlesToShow;
                    break;
                }
                else
                {
                    subtitlesTextGameObject.text = string.Empty;
                }
            }
        }

        //If a video has reached the end, leave the video player (Caution: This does obviously not work for looping videos.)
        if (videoPlayer.isPlaying && !videoPlayer.isLooping && videoPlayer.frame >= (long) videoPlayer.frameCount)
        {
            LeaveVideo();
        }

        if (Input.GetButtonUp("Jump"))
        {
            Pause();
        }

        if(Input.GetButtonUp("Cancel"))
        {
            LeaveVideo();
        }
    } 
    
    private void LeaveVideo()
    {
        SceneManager.LoadScene(sceneAfterPlayback);
    }

    private void Pause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }

    public void StartVideoPlayback(VideoClip videoClip)
    {
        //Load subtitles
        LoadSubtitles( videoClip );
        
        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();
        
        //Play Video
        videoPlayer.Play();
    }

    private void LoadSubtitles(VideoClip videoClip)
    {
        var parser = new SubtitlesParser.Classes.Parsers.SrtParser();

        // Text Asset does not support the .srt file extensions. That is why I use .txt instead.
        // However, when loading a TextAsset, file extension shall be omitted when referencing to its file name.
        TextAsset subtitleAsset = Resources.Load<TextAsset>( videoClip.name );

        // Is subtitles are available for the clip, parse them.
        if (subtitleAsset != null)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(subtitleAsset.text);
            MemoryStream stream = new MemoryStream(byteArray);
            subtitleItems = parser.ParseStream(stream, Encoding.UTF8);
        }
    }
}

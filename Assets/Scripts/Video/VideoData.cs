using System;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class VideoData
{
    public VideoClip clip;
    public string menuItemText;

    // If this tag is in the list activeDecisionTags in GameProgressData, this video to be played.
    // Every video clip must have a decision tag (with one or more characters) to be played.
    // If decisionTag is blank, the video never be played.
    public string decisionTag;

    // Tag that will be activated when playing this video, which means that videos marked with this tag, will be available for selection in following menus.
    public string decisionTagToActivate;

    // Name of video dialog to show after playback
    public string dialogAfterPlayback;

    [HideInInspector]
    public bool isWatched;
    
}

using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip defaultClip;
    public VideoClip atacaClip;
    public VideoClip lastimClip;
    public VideoClip ganaClip;
    public VideoClip pierdeClip;

    private bool isCondePierdePlaying = false;
    private bool isCondeGanaPlaying = false;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        PlayDefaultClip();
    }

    public void PlayVideo(VideoClip clip, bool loop)
    {
        videoPlayer.clip = clip;
        videoPlayer.isLooping = loop;
        videoPlayer.Play();
    }

    private void PlayDefaultClip()
    {
        videoPlayer.clip = defaultClip;
        videoPlayer.isLooping = true; // Loop the default clip
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (isCondePierdePlaying)
        {
            videoPlayer.Pause(); // Pause the video when "Conde Pierde" clip ends
            isCondePierdePlaying = false; // Reset the flag
        }
        else if (isCondeGanaPlaying)
        {
            videoPlayer.Pause();
            isCondeGanaPlaying = false;
        }
        else
        {
            PlayDefaultClip(); // Play the default clip when other videos end
        }
    }

    public void OnGameSituationChanged(string situation)
    {
        switch (situation)
        {
            case "Conde ataca":
                isCondePierdePlaying = false;
                isCondeGanaPlaying = false;
                PlayVideo(atacaClip, false);
                break;
            case "Conde lastim":
                isCondePierdePlaying = false;
                isCondeGanaPlaying = false;
                PlayVideo(lastimClip, false);
                break;
            case "Conde gana":
                isCondePierdePlaying = false;
                isCondeGanaPlaying = true;
                PlayVideo(ganaClip, false);
                break;
            case "Conde pierde":
                isCondePierdePlaying = true;
                isCondeGanaPlaying = false;
                PlayVideo(pierdeClip, false); // Play the "Conde Pierde" clip without looping
                break;
            // Add more cases as needed
            default:
                isCondePierdePlaying = false;
                isCondeGanaPlaying = false;
                PlayDefaultClip();
                break;
        }
    }
}

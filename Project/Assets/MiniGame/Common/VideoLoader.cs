using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoLoader : MonoBehaviour
{
    [SerializeField]
    private string _relativePath;
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer player = GetComponent<VideoPlayer>();
        player.source = VideoSource.Url;
        player.url = Application.streamingAssetsPath + "/" + _relativePath;
        player.prepareCompleted += PrepareCompleted;
        player.Prepare();
    }
    void PrepareCompleted(VideoPlayer vp)
    {
        vp.prepareCompleted -= PrepareCompleted;
        vp.Play();
    }
}
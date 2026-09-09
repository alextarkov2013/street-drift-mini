using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance { get; private set; }

    [System.Serializable]
    public class TrackConfig
    {
        public string trackName = "Night City";
        public string trackDescription = "";
        public bool isUnlocked = true;
        public Terrain terrain;
    }

    [SerializeField] private TrackConfig[] availableTracks = new TrackConfig[1];
    private int currentTrackIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public TrackConfig GetCurrentTrack()
    {
        return availableTracks[currentTrackIndex];
    }

    public void SetTrack(int index)
    {
        if (index >= 0 && index < availableTracks.Length && availableTracks[index].isUnlocked)
        {
            currentTrackIndex = index;
        }
    }

    public TrackConfig[] GetAllTracks() => availableTracks;
}

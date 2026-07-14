using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {

            Destroy(gameObject); 
            return;
        }

        instance = this;
        
        
        DontDestroyOnLoad(gameObject);

        
        if (audioSource == null)
        {
               audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void Start()
    {
        
        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
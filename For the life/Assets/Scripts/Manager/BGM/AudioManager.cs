using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public static AudioManager instance;

    public AudioClip clickSound;
    public AudioSource audioSource;

    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance);
        }
*/
        //audioSource = GetComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();

    }

    public void onClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}

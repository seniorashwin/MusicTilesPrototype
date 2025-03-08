using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audiosource;
 
    void Start()
    {
        audiosource.Play();
    }

}

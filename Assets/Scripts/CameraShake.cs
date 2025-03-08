using UnityEngine;
using DG.Tweening;
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    void Awake()
    {
        instance = this; // make this script easily accessible 
    }

    public void Shake(float duration = 0.2f ,float strength = 0.2f)
    {
        Camera.main.transform.DOShakePosition(duration, strength);
    }
}

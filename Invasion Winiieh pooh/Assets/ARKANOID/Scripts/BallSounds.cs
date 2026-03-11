using UnityEngine;
using Assets.ARKANOID.Scripts;  
public class BallSounds : MonoBehaviour
{
    public AudioSource fuenteAudio;

    [Header("Sonidos")]
    public AudioClip sonidoBloque;
    public AudioClip sonidoPaddle;

    private void OnCollisionEnter(Collision collision)
    {
        // 1. Si choca con un BLOQUE
        if (collision.gameObject.CompareTag("Block"))
        {
            if (fuenteAudio != null && sonidoBloque != null)
            {
                fuenteAudio.PlayOneShot(sonidoBloque);
            }
        }

        // 2. Si choca con el PADDLE
   
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (fuenteAudio != null && sonidoPaddle != null)
            {
                fuenteAudio.PlayOneShot(sonidoPaddle);
            }
        }
    }
}
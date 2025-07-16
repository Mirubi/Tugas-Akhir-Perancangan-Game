using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject player; // drag object Player ke sini

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable kontrol player
            player.GetComponent<PlayerMovement>().enabled = false;

            // (Opsional) jalankan cutscene lalu aktifkan lagi
            StartCoroutine(EndCutsceneAfterDelay(3f)); // 3 detik
        }
    }

    private IEnumerator EndCutsceneAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Enable lagi
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}

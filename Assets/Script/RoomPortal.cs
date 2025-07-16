using UnityEngine;
using System.Collections;
using Cinemachine;

public class RoomPortal : MonoBehaviour
{
    public Transform targetPosition; // tempat player muncul
    public Transform cameraTarget;   // posisi baru kamera kalau pakai cinemachine
    public float fadeDuration = 1f;
    public float waitDuration = 1f; // delay loading palsu

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TransitionRoutine(other.gameObject));
        }
    }

    private IEnumerator TransitionRoutine(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        yield return RoomFadeManager.instance.FadeOut(fadeDuration);

        yield return new WaitForSeconds(waitDuration);

        player.transform.position = targetPosition.position;

        // Update kamera
        var vcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.OnTargetObjectWarped(player.transform, player.transform.position);
            vcam.ForceCameraPosition(player.transform.position, Quaternion.identity);
        }

        yield return RoomFadeManager.instance.FadeIn(fadeDuration);

        if (movement != null) movement.enabled = true;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBubbleUI : MonoBehaviour
{
  public Image iconImage;
  public TextMeshProUGUI nameText;
  public TextMeshProUGUI dialogText;

  private Coroutine typingCoroutine;

  public void Setup(string characterName, string dialogue, Sprite icon, float typeSpeed)
  {
    nameText.text = characterName;
    dialogText.text = "";

    if (iconImage != null)
    {
      if (icon != null)
      {
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(true);
      }
      else
      {
        iconImage.gameObject.SetActive(false);
      }
    }

    if (typingCoroutine != null)
    {
      StopCoroutine(typingCoroutine);
    }
    typingCoroutine = StartCoroutine(TypeText(dialogue, typeSpeed));
  }

  private IEnumerator TypeText(string fullString, float delay)
  {
    foreach (char letter in fullString.ToCharArray())
    {
      dialogText.text += letter;
      yield return new WaitForSeconds(delay);
    }
  }

  public void SkipTyping(string fullString)
  {
    if (typingCoroutine != null)
    {
      StopCoroutine(typingCoroutine);
      typingCoroutine = null;
    }
    dialogText.text = fullString;
  }
}
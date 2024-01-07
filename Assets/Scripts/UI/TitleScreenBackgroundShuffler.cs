using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenBackgroundShuffler : MonoBehaviour
{
    [SerializeField] List<Image> backgrounds;
    [SerializeField] float changeTime = 5f;
    [SerializeField] float transitionTime = 2f;

    Queue<Image> shuffleOrder = new Queue<Image>();
    Image currentImage;


    private void Awake()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].CrossFadeAlpha(0, 0, false);
        }
        while (backgrounds.Count > 0)
        {
            int index = Random.Range(0, backgrounds.Count - 1);
            Image image = backgrounds[index];
            backgrounds.Remove(image);
            shuffleOrder.Enqueue(image);
        }
        currentImage = shuffleOrder.Dequeue();
        currentImage.CrossFadeAlpha(1, transitionTime, false);
        StartCoroutine(SwapBackgrounds());
    }

    IEnumerator SwapBackgrounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeTime);
            Image next = shuffleOrder.Dequeue();
            shuffleOrder.Enqueue(currentImage);
            next.CrossFadeAlpha(1, transitionTime, false);
            currentImage.CrossFadeAlpha(0, transitionTime, false);
            currentImage = next;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterYell : MonoBehaviour
{

    public CanvasGroup yellBox;
    public TextMeshProUGUI yellSentence;
    public YellScript lines;
    public string lineToYell;

    public int fadeCount;


    void Start()
    {
        //grabbing components
        yellBox = GetComponentInChildren<CanvasGroup>();
        yellSentence = GetComponentInChildren<TextMeshProUGUI>();

        yellBox.alpha = 0;

        // retrieve random line from the list and set text to display it
        lineToYell = lines.yellSentences[Random.Range(0, lines.yellSentences.Count)];
        yellSentence.text = lineToYell;
    }

    public void monsterYell()
    {
        Debug.Log("yell!");
        //start coroutine for displaying the stuff

        StartCoroutine("Fade");

    }

    public IEnumerator Fade()
    {
        for (float i = 0; i < 3; i+= .05f)
        {
            yellBox.alpha = i;
            yield return new WaitForSeconds(0.05f);
            fadeCount++;
        }

        if(fadeCount == 61)
        {
            for (float i = 3; i > 0; i -= .05f)
            {
                yellBox.alpha = i;
                yield return new WaitForSeconds(0.05f);
                fadeCount--;
            }
        }
    }
}

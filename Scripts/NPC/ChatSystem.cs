using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> sentences;
    public string currentSentence;
    public GameObject quad;    // 말풍선 배경
    public TextMeshPro text;

    private float timeForOneSentence;

    public void Ondialogue(string[] _lines, Transform _chatPos, float _time)
    {
        // POSITION SETTING
        transform.position = _chatPos.transform.position;

        // TIME SETTING
        timeForOneSentence = _time;

        // SORTING ORDER SETTTING
        quad.GetComponent<MeshRenderer>().sortingLayerName = "GUI";
        text.GetComponent<MeshRenderer>().sortingLayerName = "GUI";
        text.GetComponent<MeshRenderer>().sortingOrder = 1;

        sentences = new Queue<string>();
        sentences.Clear();

        foreach (var line in _lines)
        {
            sentences.Enqueue(line);
        }

        StartCoroutine(DialogueFlow(_chatPos));
    }

    private IEnumerator DialogueFlow(Transform _chatPos)
    {
        yield return null;
        while (sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue();
            text.text = currentSentence;

            // Text 길이에 따라 배경 크기 바꾸기
            float x = text.preferredWidth;
            x = (x > 4) ? 6 : x + 0.7f;
            quad.transform.localScale = new Vector2(x, text.preferredHeight + 1.5f);

            transform.position = new Vector2(_chatPos.position.x, _chatPos.position.y + text.preferredHeight / 2);
            yield return new WaitForSeconds(timeForOneSentence);
        }
        Destroy(gameObject);
    }
}
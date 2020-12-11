using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DialogueSequence());
        }
        
        private IEnumerator DialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
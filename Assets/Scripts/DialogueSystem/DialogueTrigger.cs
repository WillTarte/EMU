using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DialogueSystem
{
    
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject dialogue;
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag("Player"))
            {
                dialogue.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleCharacterHandAnimator
{
    public class MessagesUI : MonoBehaviour
    {
        [SerializeField] private Text messagesUI;

        public string message { get { return messagesUI.text; } set { messagesUI.text = value; } }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

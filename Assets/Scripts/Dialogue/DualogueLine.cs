using System;
using UnityEngine;

namespace Dialogue
{
    [Serializable]
    public class DialogueLine
    {
        [SerializeField] private string speakerName;
        [SerializeField]
        [TextArea(2, 5)]
        private string text;
        
        [SerializeField] private Sprite portrait;
        
        public string SpeakerName => speakerName;
        public string Text => text;
        public Sprite Portrait => portrait;
    }
}

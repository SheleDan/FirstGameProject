using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Game/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private DialogueLine[] lines;

        public DialogueLine[] Lines => lines;
    }
}

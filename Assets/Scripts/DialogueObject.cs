using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]

public class DialogueObject : ScriptableObject {
    [SerializeField][TextArea] private string[] names;
    [SerializeField][TextArea] private string[] dialogue;

    public string[] Names => names;
    public string[] Dialogue => dialogue;
}

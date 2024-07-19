using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject
{
    public string[] textStart;
    public string[] textPositive;
    public string[] textNegative;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/YellScript", order = 1)]
public class YellScript : ScriptableObject
{
    public List<string> yellSentences;
}

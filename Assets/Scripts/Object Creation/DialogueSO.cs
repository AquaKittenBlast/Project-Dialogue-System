using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public string dialogueName;
    public List<Sentence> sentences;
}
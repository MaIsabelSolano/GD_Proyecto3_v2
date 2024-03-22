using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class DialogueMng : MonoBehaviour
{
  public List<Dialogue> Dialogues;
  [SerializeField] TextAsset dialoguesJSON;
  
  
  // Start is called before the first frame update
  void Start()
  {
    // obtener la data del JSON
    if (dialoguesJSON != null) {
      Debug.Log(dialoguesJSON);

      // Deserialize the JSON data into a DialogueList object
      DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(dialoguesJSON.text);

      // Assign the list of dialogues from the DialogueList object to Dialogues
      Dialogues = dialogueList.Dialogos;

      Debug.Log(Dialogues.Count);
      Debug.Log(Dialogues[0].emisor);
      Debug.Log(Dialogues[0].texto);
    }
  }

}

[System.Serializable]
public class DialogueList
{
    public List<Dialogue> Dialogos; // Use the same name as in the JSON

    // Constructor to initialize the list
    public DialogueList()
    {
        Dialogos = new List<Dialogue>();
    }
}

[System.Serializable]
public class Dialogue
{
  public int id;
  public int escena;
  public int ruta;
  public string texto;
  public string emisor;
  public List<Option> opciones;
  public List<Emotion> emociones;
}


// Define classes for nested objects
[System.Serializable]
public class Option
{
  public int num;
  public string texto;
  public int rutaA;
}

[System.Serializable]
public class Emotion
{
    public string personaje;
    public string emocion;
}
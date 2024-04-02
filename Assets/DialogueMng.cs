using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMng : MonoBehaviour
{
  public List<Dialogue> Dialogues;
  [SerializeField] TextAsset dialoguesJSON;

  [SerializeField] TextMeshProUGUI DialogoTexto;
  [SerializeField] TextMeshProUGUI DialogoEmisor;
  [SerializeField] Button OptionBtn1;
  [SerializeField] Button OptionBtn2;
  [SerializeField] Button OptionBtn3;
  [SerializeField] Button OptionBtn4;
  private List<Button> OptionBtns = new List<Button>();
  private bool OptionsActive;

  private Sprite[] KathySprites;

  int currentDialogue = 0;
  [SerializeField] RectTransform canvasRectTransform;

  private Dictionary<string, Sprite[]> characterSprites = new Dictionary<string, Sprite[]>();
  
  
  // Start is called before the first frame update
  void Start()
  {
    // obtener la data del JSON
    if (dialoguesJSON != null) {
      Debug.Log(dialoguesJSON);


      KathySprites = Resources.LoadAll<Sprite>("Sprites/Kathy/");
      loadCharacterSprites();

      // Deserialize the JSON data into a DialogueList object
      DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(dialoguesJSON.text);

      // Assign the list of dialogues from the DialogueList object to Dialogues
      Dialogues = dialogueList.Dialogos;


      OptionBtn1.onClick.AddListener(changeToOp1);
      OptionBtn2.onClick.AddListener(changeToOp2);
      OptionBtn3.onClick.AddListener(changeToOp3);
      OptionBtn4.onClick.AddListener(changeToOp4);

      // Agregarl los botones a la lista
      OptionBtns.Add(OptionBtn1);
      OptionBtns.Add(OptionBtn2);
      OptionBtns.Add(OptionBtn3);
      OptionBtns.Add(OptionBtn4);

      updateText();
      updateEmotions();

    }
  }

  // Update is called once per frame
  void Update()
  {
    // no aceptar input que no sea de los botones
    if (Dialogues[currentDialogue].opciones.Count > 0) {
      if (!OptionsActive) {
        // Reactivar los botones
        showButtons();
        // updateEmotions();
      }
    } 
    // cambiar de escena si se llegó al final  
    else if (Dialogues[currentDialogue].EOS) {
      // Es el final de la historia
      // TODO: Añadir lógica de cambio de escena
      // updateEmotions();
    }
    else {
      // Click normal si la historia no tiene opciones
      if (Input.GetMouseButtonDown(0)) {
        Debug.Log("click");
        if (currentDialogue < Dialogues.Count && currentDialogue >= 0) {
          currentDialogue = Dialogues[currentDialogue].next; // Cambiar el diálogo
          updateText();
          updateEmotions();
        }
      }
    }  
    
  }

  void loadCharacterSprites() {
    string[] characterNames = {"Kathy", "Dad"};
    foreach (string charName in characterNames) {
      Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + charName);
      characterSprites.Add(charName, sprites);
    }
  }

  void updateText() {
    Debug.Log("updateText");
    DialogoEmisor.text = Dialogues[currentDialogue].emisor;
    DialogoTexto.text = Dialogues[currentDialogue].texto;
    hideButtons();
  }

  void updateEmotions() {
    Debug.Log("updateEmotion");

    foreach (Emotion charEmotion in Dialogues[currentDialogue].emociones) {
      if (characterSprites.ContainsKey(charEmotion.personaje)) {
        Debug.Log("contains key");
        foreach (Sprite sp in characterSprites[charEmotion.personaje]) {
          Debug.Log("Sprites de " + characterSprites[charEmotion.personaje]);
          if (sp.name == charEmotion.emocion) {
            Transform currChar = canvasRectTransform.Find("Personajes").Find(charEmotion.personaje);
            if (currChar != null) {
              Image characterImg = currChar.GetComponent<Image>();
              // cambiar imagen
              characterImg.sprite  = sp;

              // cambiar posición
              updateCharacterPosition(charEmotion.posicion, characterImg);

              // TODO: Efectos
              
            }
          }
        }
      }
    }
  }

  void updateCharacterPosition(string pos, Image chara) {
    if (pos == "C") {
      chara.transform.position = canvasRectTransform.
        TransformPoint(new Vector2(0, (- canvasRectTransform.rect.width / 2f) - (- chara.rectTransform.rect.height/2f)));
    } else if (pos == "R") {
      chara.transform.position = canvasRectTransform.
        TransformPoint(new Vector2(canvasRectTransform.rect.width / 4f, (- canvasRectTransform.rect.height / 2f) - (- chara.rectTransform.rect.height/2f)));
    } else if (pos == "L") {
      chara.transform.position = canvasRectTransform.
        TransformPoint(new Vector2(- canvasRectTransform.rect.width / 4f, (- canvasRectTransform.rect.height / 2f) - (- chara.rectTransform.rect.height/2f)));
      // Vector2 vec = chara.transform.localScale;
      // vec.x *= -1; 
      // chara.transform.localScale = vec;
    } else if (pos == "Out") {
      // Afuera del mapa
      chara.transform.position = canvasRectTransform.
        TransformPoint(new Vector2(2 * canvasRectTransform.rect.width, 2 * canvasRectTransform.rect.height));
    }
  }

  void hideButtons() {
    for (int i = 0; i < OptionBtns.Count; i ++) {
      OptionBtns[i].gameObject.SetActive(false);
    }
    OptionsActive = false;
  }

  void showButtons() {
    OptionsActive = true;
    for (int i = 0; i < Dialogues[currentDialogue].opciones.Count; i++) {
        Button button = OptionBtns[i];
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(); // Use TMP_Text instead of Text
        if (buttonText != null) {
            buttonText.text = Dialogues[currentDialogue].opciones[i].texto;
        }
        button.gameObject.SetActive(true);
    }
  }

  void changeToOp1(){
    currentDialogue = Dialogues[currentDialogue].opciones[0].rutaA;
    updateText();
    updateEmotions();
  }
  void changeToOp2(){
    currentDialogue = Dialogues[currentDialogue].opciones[1].rutaA;
    updateText();
    updateEmotions();
  }
  void changeToOp3(){
    currentDialogue = Dialogues[currentDialogue].opciones[2].rutaA;
    updateText();
    updateEmotions();
  }
  void changeToOp4(){
    currentDialogue = Dialogues[currentDialogue].opciones[3].rutaA;
    updateText();
    updateEmotions();
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
  public int next;
  public bool EOS; //End of Story
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
    public string posicion;
}
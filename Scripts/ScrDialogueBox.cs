using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrDialogueBox : MonoBehaviour
{
    public ScrGameMaster gamemaster;
    public TextMeshProUGUI textComponent;
    public string[] lines = null;
    public float textSpeed = 0.01f;

    private int index;

    void Start()
    {
        gamemaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();
        textComponent.text = string.Empty;
        rf_StartDialogue();
    }

    void Update()
    {
        if (lines[index] == "")
        {
            gameObject.SetActive(false);
            gamemaster.rf_unlockPlayer();
        }


        // if text is finished, on input it goes to next line
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                rf_NextLine();
            }
            else // if text is not finished, it goes to finished state
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void rf_StartDialogue()
    {
        index = 0; // sets to start of text box
        StartCoroutine(rIE_TypeLine());
    }

    IEnumerator rIE_TypeLine()
    {
        // goes through characters in the lines and types them individually
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }

    void rf_NextLine()
    {
        // passa para a próxima linha de texto
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;

            // calls for next line to be written
            if (lines[index] != "")
            {
                StartCoroutine(rIE_TypeLine());
            }
            else
            {
                // raises index once more and calls for next line to be written
                index++;
                rf_NextLine();
            }
        }
        else // se for a última linha de texto, o objeto se desativa
        {
            gameObject.SetActive(false);
            gamemaster.rf_unlockPlayer();
        }
    }

}

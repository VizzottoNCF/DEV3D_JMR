using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrGameMaster : MonoBehaviour
{

    // GameMaster will be holding flags and most game functions

    public GameObject[] MuseumItems;
    public ScrMuseumItem tempScr;
    public ScrDialogueBox dialogScr;
    public GameObject WallKeyItem1 = null; 
    public GameObject WallKeyItem2 = null; 
    public GameObject PreviousItemInteracted = null;//var object interacted previously    
    public GameObject ItemHoldingNow; //var item holding now
    public FirstPersonController fpcScript; // movement script to be able to control/lock movement
    public ScrRayCasting RayCastScript;
    private float interactionDelayMax = 1.0f;
    private float interactionDelayNow = 1.0f;

    public string[] arr;
    public string[] arr2;
    public string[] arr3;


    #region unused
    // temp vars for data persistance class. Unused
    public bool redAcquired = false;
    public bool greenAcquired = false;
    public bool blueAcquired = false;
    public bool yellowAcquired = false;
    #endregion

    void Start()
    {
        #region Museum Items arrangements as keys

        arr = new string[] { "Diálogo 1 (parte 2)", "Diálogo 2 (parte 2)", "Diálogo 3 (parte 2)" };
        arr2 = new string[] { "Diálogo 1 (parte 3)", "Diálogo 2 (parte 3)", "Diálogo 3 (parte 3)", "Diálogo 4 (parte 3)" };
        arr3 = new string[] { "Diálogo 1 (parte 4)", "Diálogo 2 (parte 4)" };


        rf_WallKeys();

        print(WallKeyItem1);
        #endregion
        fpcScript = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        dialogScr = GameObject.FindWithTag("DialogBox").GetComponent<ScrDialogueBox>();
        RayCastScript = GameObject.FindWithTag("RayCastPlayer").GetComponent<ScrRayCasting>();
    }

    void Update()
    {

        if (fpcScript.playerCanMove)
        {
            if (interactionDelayNow > 0)
            {
                interactionDelayNow -= Time.deltaTime;
            }
        }
        else
        {
            interactionDelayNow = interactionDelayMax;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            rf_PassDialogueText(arr);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            rf_PassDialogueText(arr2);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            rf_PassDialogueText(arr3);
        }
        if (Input.GetKey(KeyCode.O))
        {
            rf_lockPlayer();
        }
        if (Input.GetKey(KeyCode.P))
        {
            rf_unlockPlayer();
        }
    }


    public void rf_MeshTurnOff(GameObject ItemToTurnOff)
    {
        // turns off mesh renderer of called item
        ItemToTurnOff.GetComponent<MeshRenderer>().enabled = false;

        // changes item held
        ItemHoldingNow = ItemToTurnOff;

        // turns on mesh renderer from previous held object
        if(PreviousItemInteracted!=null&& PreviousItemInteracted!= ItemHoldingNow) PreviousItemInteracted.GetComponent<MeshRenderer>().enabled = true;
        PreviousItemInteracted = ItemToTurnOff;

        Debug.Log(ItemHoldingNow);
    }

    private void rf_lockPlayer()
    {
        fpcScript.playerCanMove = false;
        fpcScript.cameraCanMove = false;
        //RayCastScript.enabled = false;
    }
    public void rf_unlockPlayer()
    {
        fpcScript.playerCanMove = true;
        fpcScript.cameraCanMove = true;
        //RayCastScript.enabled = true;
    }

    public void rf_DestroyWall(MeshRenderer _MeshRenderer, BoxCollider _BoxCollider, string _WallName)
    {
        if (ItemHoldingNow == WallKeyItem1 && _WallName == "Wall 1") 
        {
            _MeshRenderer.enabled = false;
            _BoxCollider.enabled = false;
            print("PAREDE 1 DESTRUÍDA");
        }
        if (ItemHoldingNow == WallKeyItem2 && _WallName == "Wall 2") 
        {
            _MeshRenderer.enabled = false;
            _BoxCollider.enabled = false;
            print("PAREDE 2 DESTRUÍDA");
        }
    }

    public void rf_WallKeys()
    {
        // grabs an array with all items tagged "MuseumItem"
        MuseumItems = GameObject.FindGameObjectsWithTag("MuseumItem");
        for (int i = 0; i < MuseumItems.Length; i++)
        {
            print(i + " == " +MuseumItems[i]);
            if (MuseumItems[i].name == "Key_Wall_1")
            {
                WallKeyItem1 = MuseumItems[i].gameObject;
            }
            else if (MuseumItems[i].name == "Key_Wall_2")
            {
                WallKeyItem2 = MuseumItems[i].gameObject;
            }
        }

        Debug.Log(WallKeyItem1);
        Debug.Log(WallKeyItem2);
    }

    public void rf_PassDialogueText(string[] _text_lines)
    {
        // turns gameobject back on
        dialogScr.gameObject.SetActive(true);
        dialogScr.textComponent.text = "";

        // adjusts lines lenght


        // clear previous dialogue from text array
        Array.Clear(dialogScr.lines,0, dialogScr.lines.Length);

        // make all of them blank characters so textbox can properly close after text ends
        for (int i = 0; i < dialogScr.lines.Length; i++)
        {
            dialogScr.lines[i] = "";
        }
        
        // fill out next dialogue with array received from interaction
        for (int i = 0; i < _text_lines.Length; i++)
        {
            dialogScr.lines[i] = _text_lines[i];
        }

        // Starts typing textbox
        dialogScr.rf_StartDialogue();
        rf_lockPlayer();

    }

    public void rf_GhostInteraction(GameObject _ghost)
    {
        // passes through public var Dialogue from NPC to Dialogue 
        if (fpcScript.playerCanMove && interactionDelayNow <= 0)
        {
            rf_PassDialogueText(_ghost.GetComponent<ScrGhostInteraction>().dialogue);
        }
    }


    #region commented // unused
    //public void rf_SearchAllAvailableItems()
    //{
    //    // grabs an array with all items tagged "MuseumItem"
    //    MuseumItems = GameObject.FindGameObjectsWithTag("MuseumItem");

    //    for (int i = 0; i < MuseumItems.Length; i++)
    //    {
    //        tempScr = MuseumItems[i].GetComponent<ScrMuseumItem>();
    //        if (!tempScr.in_range || tempScr.alreadyUsed)
    //        {

    //            return;
    //        }
    //    }
    //    print("a");
    //    // Sets WallKeyItem1 as a random item tagged  "MuseumItem"
    //    WallKeyItem1 = MuseumItems[Random.Range(0, MuseumItems.Length)];

    //    // Sets WallKeyItem2 as a different random item tagged "MuseumItem"
    //    WallKeyItem2 = MuseumItems[Random.Range(0, MuseumItems.Length)];
    //    while (WallKeyItem2 == WallKeyItem1)
    //    {
    //        WallKeyItem2 = MuseumItems[Random.Range(0, MuseumItems.Length)];
    //    }
    //}

    //public void rf_ItemArrayResize(ref T[], int index)
    //{
    //    for (int a = index; a < arr.Length - 1; a++)
    //    {
    //        // moving elements downwards, to fill the gap at [index]
    //        arr[a] = arr[a + 1];
    //    }
    //    // finally, let's decrement Array's size by one
    //    Array.Resize(ref arr, arr.Length - 1);
    //}
    #endregion
}

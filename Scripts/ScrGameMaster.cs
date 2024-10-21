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
    public GameObject ItemHoldingNowVisual;
    public FirstPersonController fpcScript; // movement script to be able to control/lock movement
    public ScrRayCasting RayCastScript;
    private float interactionDelayMax = 1.0f;
    private float interactionDelayNow = 1.0f;


    void Start()
    {
        #region Museum Items arrangements as keys

        rf_WallKeys();

        print(WallKeyItem1);
        #endregion

        // item holding visual startup
        rf_UpdateHoldingItemVisual()
;


        #region scripts
        fpcScript = GameObject.FindWithTag("Player").GetComponent<FirstPersonController>();
        dialogScr = GameObject.FindWithTag("DialogBox").GetComponent<ScrDialogueBox>();
        RayCastScript = GameObject.FindWithTag("RayCastPlayer").GetComponent<ScrRayCasting>();
        #endregion
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

        if (Input.GetKey(KeyCode.O))
        {
            rf_lockPlayer();
        }
        if (Input.GetKey(KeyCode.P))
        {
            rf_unlockPlayer();
        }
    }

    public void rf_UpdateHoldingItemVisual()
    {
        if (ItemHoldingNow == null)
        {
            ItemHoldingNowVisual.GetComponent<MeshFilter>().mesh = null;
        }
        else
        {
            ItemHoldingNowVisual.GetComponent<MeshFilter>().mesh = ItemHoldingNow.GetComponent<MeshFilter>().mesh;
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

        Debug.Log("Now Holding: " + ItemHoldingNow);

        rf_UpdateHoldingItemVisual();


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

    public void rf_DestroyWallNPC(GameObject _Wall)
    {
        _Wall.SetActive(false);
        print(_Wall.name + " desativado");
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

        var _temp_ghostScript = _ghost.GetComponent<ScrGhostInteraction>();

        // flips ghostkey1 as true
        if (_temp_ghostScript.GhostKey1 == ItemHoldingNow)
        {
            // destroys wall boundary if there is one
            if (_temp_ghostScript.Wall1 != null)
            {
                rf_DestroyWallNPC(_temp_ghostScript.Wall1);
                rf_MeshTurnOff(null);
            }
            _temp_ghostScript.GhostKey1Used = true;
            _temp_ghostScript.dialogue = _temp_ghostScript.dialogueKey2;
        }
        // flips ghostkey2 as true
        if (_temp_ghostScript.GhostKey2 == ItemHoldingNow)
        {
            // destroys wall boundary if there is one
            if (_temp_ghostScript.Wall2 != null)
            {
                rf_DestroyWallNPC(_temp_ghostScript.Wall2);
            }
            _temp_ghostScript.GhostKey2Used = true;
            _temp_ghostScript.dialogue = _temp_ghostScript.dialogueEnd;
        }

        // passes through public var Dialogue from NPC to Dialogue 
        if (fpcScript.playerCanMove && interactionDelayNow <= 0)
        {
            rf_PassDialogueText(_ghost.GetComponent<ScrGhostInteraction>().dialogue);
        }

        _temp_ghostScript.FirstInteraction = true;

        

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

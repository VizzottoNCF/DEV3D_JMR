using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrWindowPopUp : MonoBehaviour
{
    private GameObject PlayerCharacter;
    private Transform self;
    private bool IsExtended = false;
    private bool IsExtended110 = false;
    private bool CoroutinePlaying = false;
    private float scalePercent = 0;
    private float scale110p = 0.2f;
    private float scale100p = 0.13f;
    private float scale0p = 0.01f;
    private float speed = 1.5f;
    void Start()
    {
        self = this.GetComponent<Transform>();

        PlayerCharacter = GameObject.FindWithTag("Player");


        scalePercent = scale0p;

        self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);
    }

    void Update()
    {

        if (Vector3.Distance(self.position, PlayerCharacter.transform.position) < 2f && !CoroutinePlaying && !IsExtended)
        {
            StartCoroutine(rIE_Expand110());
        }
        else if (IsExtended110 && !CoroutinePlaying)
        {
            StartCoroutine(rIE_Decrease100());
        }
        else if (IsExtended && !IsExtended110 && !CoroutinePlaying && Vector3.Distance(self.position, PlayerCharacter.transform.position) > 2f)
        {
            StartCoroutine(rIE_ReduceToZero());
        }
    }


    private IEnumerator rIE_Expand110()
    {
        CoroutinePlaying = true;
        //scalePercent = scale0p / scale100p;
        scalePercent = scale0p;

        self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);

        while (scalePercent < scale110p)
        {
            scalePercent += speed * Time.deltaTime;

            if (scalePercent > scale110p)
            {
                scalePercent = scale110p;
                IsExtended = true;
                IsExtended110 = true;
            }

            self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);

            Debug.Log("Expand110");
            yield return new WaitForEndOfFrame(); 
            //yield return new WaitForFixedUpdate(); 
        }

        CoroutinePlaying = false;
    }

    private IEnumerator rIE_Decrease100()
    {
        CoroutinePlaying = true;
        while (scalePercent > scale100p)
        {
            scalePercent -= speed * Time.deltaTime;

            self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);

            Debug.Log("Decrease100");
            yield return new WaitForEndOfFrame();
            //yield return new WaitForFixedUpdate();
        }

        IsExtended110 = false;
        scalePercent = scale100p;
        CoroutinePlaying = false;
        self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);
    }

    private IEnumerator rIE_ReduceToZero()
    {
        CoroutinePlaying = true;

        while (scalePercent > scale0p)
        {
            scalePercent -= speed * Time.deltaTime;

            self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);

            Debug.Log("ReduceToZero");
            yield return new WaitForEndOfFrame();
        }

        IsExtended = false;
        scalePercent = scale0p;
        CoroutinePlaying = false;
        self.localScale = new Vector3(scalePercent, scalePercent, scalePercent);
    }
}

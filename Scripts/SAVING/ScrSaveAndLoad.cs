using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrSaveAndLoad : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerCamera;
    private ScrGameMaster GameMaster;
    public void Start()
    {
        GameMaster = GameObject.FindWithTag("GameMaster").GetComponent<ScrGameMaster>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DeleteSave();
        }
    }


    public void SaveGame()
    {
        print("File Saved");

        string filePath = Path.Combine(Application.streamingAssetsPath, "SaveFile.xml");

        // instanciates save state
        ClassSaveState SaveState = new ClassSaveState();

        // player rotation and position
        SaveState.xPos = Player.transform.position.x;
        SaveState.yPos = Player.transform.position.y;
        SaveState.zPos = Player.transform.position.z;

        SaveState.xRot = PlayerCamera.transform.rotation.eulerAngles.x;
        SaveState.yRot = Player.transform.rotation.eulerAngles.y;
        SaveState.zRot = PlayerCamera.transform.rotation.eulerAngles.z;

        // game flags
        SaveState.greenAcquired = GameMaster.greenAcquired;
        SaveState.redAcquired = GameMaster.redAcquired;
        SaveState.blueAcquired = GameMaster .blueAcquired;
        SaveState.yellowAcquired = GameMaster.yellowAcquired;


        // checks if file directory does not exists, if it doesn't creates it
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }


        // if filepath does not exist, serializes and writes in XML file
        if (!File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClassSaveState));
            StreamWriter streamWriter = new StreamWriter(filePath);
            serializer.Serialize(streamWriter.BaseStream, SaveState);
            streamWriter.Close();

        }
        // if filepath exists, deserializes and reads list
        else
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClassSaveState));
            StreamReader streamReader = new StreamReader(filePath);
            streamReader.Close();

            // serializes and closes stream writer
            StreamWriter streamWriter = new StreamWriter(filePath);
            xmlSerializer.Serialize(streamWriter.BaseStream, SaveState);
            streamWriter.Close();

        }
    }

    public void DeleteSave()
    {
        File.Delete(Path.Combine(Application.streamingAssetsPath, "SaveFile.xml"));
        print("Save File Deleted");

    }

    public void LoadGame()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "SaveFile.xml");

        if (!File.Exists(filePath))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            print("Save File does not exist");
        }
        else
        {
            print("File Loaded");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClassSaveState));
            StreamReader streamReader = new StreamReader(filePath);
            ClassSaveState SaveState = (ClassSaveState)xmlSerializer.Deserialize(streamReader.BaseStream);
            streamReader.Close();

            // player rotation and position
            // Setting the position
            Player.transform.position = new Vector3(SaveState.xPos, SaveState.yPos, SaveState.zPos);

            // Setting the rotation
            Player.transform.rotation = Quaternion.Euler(0, SaveState.yRot, 0);
            PlayerCamera.transform.rotation = Quaternion.Euler(SaveState.xRot, 0, SaveState.zRot);
            
            // game flags
            GameMaster.greenAcquired = SaveState.greenAcquired;
            GameMaster.redAcquired = SaveState.redAcquired;
            GameMaster.blueAcquired = SaveState.blueAcquired;
            GameMaster.yellowAcquired = SaveState.yellowAcquired;



        }
    }
}

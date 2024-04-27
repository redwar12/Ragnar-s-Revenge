using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private GameObject button1;
    private GameObject button2;
    private GameObject button3;
    private GameObject button4;
    private GameObject backButton;
    private GameObject highScoresButton;
    private GameObject creditsButton;
    private GameObject howToPlayButton;
    [SerializeField] private Color hoverColor = Color.green;
    private bool canSelect = true;
    public GameObject menu;
    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;
    private TextMeshProUGUI text3;
    private TextMeshProUGUI text4;
    private GameObject masterSlider;
    private GameObject musicSlider;
    private GameObject speechSlider;
    private GameObject sFXSlider;

    private PlayerMovement playerMovement;
    private int playerProgress;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // Find all GameObjects for Menu
        button1 = GameObject.Find("Button1");
        button2 = GameObject.Find("Button2");
        button3 = GameObject.Find("Button3");
        button4 = GameObject.Find("Button4");
        backButton = GameObject.Find("BackButton");
        highScoresButton = GameObject.Find("HighScoresButton");
        masterSlider = GameObject.Find("MasterSlider");
        musicSlider = GameObject.Find("MusicSlider");
        speechSlider = GameObject.Find("SpeechSlider");
        sFXSlider = GameObject.Find("SFXSlider");
        creditsButton = GameObject.Find("CreditsButton");
        howToPlayButton = GameObject.Find("HowToPlayButton");
        masterSlider.SetActive(false);
        musicSlider.SetActive(false);
        speechSlider.SetActive(false);
        sFXSlider.SetActive(false);


        // Find all TextMeshProUGUI for Menu
        TextMeshProUGUI text1 = button1.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text2 = button2.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text3 = button3.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI text4 = backButton.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI highScoreText = highScoresButton.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI backButtonText = backButton.GetComponent<TextMeshProUGUI>();
        playerMovement = player.GetComponent<PlayerMovement>();
        //playerMovement.canMove = false;

        // if player progress is 0, grey out and disable the colliders of Load Game and High Scores
        playerProgress = DataPersistenceManager.instance.GameData.playerProgress;
        Debug.Log("Player Progress: " + playerProgress);
        if (playerProgress == 0)
        {
            text2.color = Color.grey;
            button2.GetComponent<BoxCollider>().enabled = false;
            backButton.SetActive(false);
            highScoresButton.SetActive(false);
        }

        backButton.SetActive(false);
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        // Update from 'interactable' to 'interactableObject'
        TextMeshProUGUI text = args.interactableObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            ChangeTextColour(text, hoverColor); // Ensure this function is in the same script
        }
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        // Update from 'interactable' to 'interactableObject'
        TextMeshProUGUI text = args.interactableObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            ChangeTextColour(text, Color.white); // Reset to default color
        }
    }

    // Make sure this method is in the same script as the hover methods
    public void ChangeTextColour(TextMeshProUGUI text, Color colour)
    {
        text.color = colour;
    }

    public void OnActivated(ActivateEventArgs args)
    {
        {
            backButton.SetActive(true);
            string buttonPressed = args.interactable.gameObject.GetComponent<TextMeshProUGUI>().text;
            Debug.Log("Button Pressed: " + buttonPressed);

            switch (buttonPressed)
            {
                case "New Game":
                    Debug.Log("New Game");
                    playerMovement.canMove = true;
                    menu.SetActive(false);
                    break;

                case "Load Level":
                    button2.GetComponent<BoxCollider>().enabled = true;
                    button1.GetComponent<TextMeshProUGUI>().text = "Load Tutorial";
                    button2.GetComponent<TextMeshProUGUI>().text = "Load Level 1";
                    button3.GetComponent<TextMeshProUGUI>().text = "Load Level 2";
                    button4.GetComponent<TextMeshProUGUI>().text = "Load Level 3";
                    break;

                case "Settings":
                    button1.GetComponent<TextMeshProUGUI>().text = "Master Volume";
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<TextMeshProUGUI>().text = "SFX Volume";
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<TextMeshProUGUI>().text = "Music Volume";
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<TextMeshProUGUI>().text = "Speech Volume";
                    button4.GetComponent<BoxCollider>().enabled = false;
                    masterSlider.SetActive(true);
                    musicSlider.SetActive(true);
                    speechSlider.SetActive(true);
                    sFXSlider.SetActive(true);
                    break;

                case "Quit":
                    Application.Quit();
                    break;

                case "High\nScores":
                    button2.GetComponent<BoxCollider>().enabled = true;
                    button1.GetComponent<TextMeshProUGUI>().text = "Tutorial";
                    button2.GetComponent<TextMeshProUGUI>().text = "Level 1";
                    button3.GetComponent<TextMeshProUGUI>().text = "Level 2";
                    button4.GetComponent<TextMeshProUGUI>().text = "Level 3";
                    masterSlider.SetActive(false);
                    musicSlider.SetActive(false);
                    speechSlider.SetActive(false);
                    sFXSlider.SetActive(false);


                    //grey out and disable the colliders of levels the player has not completed
                    if (playerProgress < 1)
                    {
                        button2.GetComponent<TextMeshProUGUI>().color = Color.grey;
                        button2.GetComponent<BoxCollider>().enabled = false;
                    }
                    if (playerProgress < 2)
                    {
                        button3.GetComponent<TextMeshProUGUI>().color = Color.grey;
                        button3.GetComponent<BoxCollider>().enabled = false;
                    }
                    if (playerProgress < 3)
                    {
                        button4.GetComponent<TextMeshProUGUI>().color = Color.grey;
                        button4.GetComponent<BoxCollider>().enabled = false;
                    }



                    break;

                case "Back\n<-":
                    backButton.SetActive(false);
                    button1.GetComponent<BoxCollider>().enabled = true;
                    button2.GetComponent<BoxCollider>().enabled = true;
                    button3.GetComponent<BoxCollider>().enabled = true;
                    button4.GetComponent<BoxCollider>().enabled = true;
                    button1.GetComponent<TextMeshProUGUI>().text = "New Game";
                    button2.GetComponent<TextMeshProUGUI>().text = "Load Level";
                    button3.GetComponent<TextMeshProUGUI>().text = "Settings";
                    button4.GetComponent<TextMeshProUGUI>().text = "Quit";
                    masterSlider.SetActive(false);
                    musicSlider.SetActive(false);
                    speechSlider.SetActive(false);
                    sFXSlider.SetActive(false);
                    // set all text to white
                    button1.GetComponent<TextMeshProUGUI>().color = Color.white;
                    button2.GetComponent<TextMeshProUGUI>().color = Color.white;
                    button3.GetComponent<TextMeshProUGUI>().color = Color.white;
                    button4.GetComponent<TextMeshProUGUI>().color = Color.white;
                    highScoresButton.GetComponent<TextMeshProUGUI>().color = Color.white;

                    if (playerProgress == 0)
                    {
                        button2.GetComponent<TextMeshProUGUI>().color = Color.grey;
                        button2.GetComponent<BoxCollider>().enabled = false;
                        highScoresButton.GetComponent<TextMeshProUGUI>().color = Color.grey;
                        highScoresButton.GetComponent<BoxCollider>().enabled = false;
                    }
                    
                    break;

                case "Tutorial":
                    button1.GetComponent<TextMeshProUGUI>().text = "Tutorial Top 3 Times:";
                    button2.GetComponent<TextMeshProUGUI>().text = "1. " + DataPersistenceManager.instance.GameData.level1Score1Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level1Score1Seconds.ToString("D2");
                    button3.GetComponent<TextMeshProUGUI>().text = "2. " + DataPersistenceManager.instance.GameData.level1Score2Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level1Score2Seconds.ToString("D2");
                    button4.GetComponent<TextMeshProUGUI>().text = "3. " + DataPersistenceManager.instance.GameData.level1Score3Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level1Score3Seconds.ToString("D2");
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                case "Level 1":
                    button1.GetComponent<TextMeshProUGUI>().text = "Level 1 Top 3 Times:";
                    button2.GetComponent<TextMeshProUGUI>().text = "1. " + DataPersistenceManager.instance.GameData.level2Score1Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level2Score1Seconds.ToString("D2");
                    button3.GetComponent<TextMeshProUGUI>().text = "2. " + DataPersistenceManager.instance.GameData.level2Score2Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level2Score2Seconds.ToString("D2");
                    button4.GetComponent<TextMeshProUGUI>().text = "3. " + DataPersistenceManager.instance.GameData.level2Score3Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level2Score3Seconds.ToString("D2");
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                case "Level 2":
                    button1.GetComponent<TextMeshProUGUI>().text = "Level 2 Top 3 Times:";
                    button2.GetComponent<TextMeshProUGUI>().text = "1. " + DataPersistenceManager.instance.GameData.level3Score1Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level3Score1Seconds.ToString("D2");
                    button3.GetComponent<TextMeshProUGUI>().text = "2. " + DataPersistenceManager.instance.GameData.level3Score2Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level3Score2Seconds.ToString("D2");
                    button4.GetComponent<TextMeshProUGUI>().text = "3. " + DataPersistenceManager.instance.GameData.level3Score3Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level3Score3Seconds.ToString("D2");
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                case "Level 3":
                    button1.GetComponent<TextMeshProUGUI>().text = "Level 3 Top 3 Times:";
                    button2.GetComponent<TextMeshProUGUI>().text = "1. " + DataPersistenceManager.instance.GameData.level4Score1Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level4Score1Seconds.ToString("D2");
                    button3.GetComponent<TextMeshProUGUI>().text = "2. " + DataPersistenceManager.instance.GameData.level4Score2Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level4Score2Seconds.ToString("D2");
                    button4.GetComponent<TextMeshProUGUI>().text = "3. " + DataPersistenceManager.instance.GameData.level4Score3Minutes.ToString("D2") + ":" + DataPersistenceManager.instance.GameData.level4Score3Seconds.ToString("D2");
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                case "Credits":
                    button1.GetComponent<TextMeshProUGUI>().text = "Can be found in a readme file in the games folder";
                    button2.GetComponent<TextMeshProUGUI>().text = "";
                    button3.GetComponent<TextMeshProUGUI>().text = "";
                    button4.GetComponent<TextMeshProUGUI>().text = "";
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                case "How to Play":
                    button1.GetComponent<TextMeshProUGUI>().text = "Use the left stick to move";
                    button2.GetComponent<TextMeshProUGUI>().text = "Use the right stick to look around";
                    button3.GetComponent<TextMeshProUGUI>().text = "Grip to pick up objects";
                    button4.GetComponent<TextMeshProUGUI>().text = "Trigger to interact with the menu";
                    button1.GetComponent<BoxCollider>().enabled = false;
                    button2.GetComponent<BoxCollider>().enabled = false;
                    button3.GetComponent<BoxCollider>().enabled = false;
                    button4.GetComponent<BoxCollider>().enabled = false;
                    break;

                default:
                    Debug.LogError("Button pressed action not recognized.");
                    break;
            }
        }
    }
}
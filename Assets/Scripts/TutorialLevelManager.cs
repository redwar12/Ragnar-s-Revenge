﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
public class TutorialLevelManager : MonoBehaviour
{
    public GameObject fire;
    public Light[] lightSources;
    public AudioClip[] ragnarSpeech;
    public AudioSource ragnarAudioSource;
    public GameObject ragnarSpeechObject;
    public bool puzzle1Check = false;
    public bool puzzle2Check = false;
    public bool puzzle3Check = false;
    private bool puzzle1Complete = false;
    private bool puzzle2Complete = false;
    private bool puzzle3Complete = false;
    private bool roomComplete = false;

    [SerializeField] private GameObject puzzle1Trigger;
    [SerializeField] private GameObject puzzle2Trigger;
    [SerializeField] private GameObject puzzle3Trigger;

    public Timer timer;

    public GameObject VikingStatue;
    public GameObject VikingGhostStatue;
    public GameObject endOfLevelText;

    public float waitTime = 5.0f; // time to wait between events in seconds

    public AudioClip jarlSpeech;
    public AudioClip[] jarlVoiceLines;
    public AudioSource jarlAudioSource;
    public PlayerMovement playerMovement;
    public bool hasRing;
    public bool needsMetal;
    public bool usedWrongKey;
    public GameObject[] fireObjects;
    public Material torchMaterial;
    public bool defeatedSkeleton;
    public bool unlockedDoor;
    public bool openedDoor;

    private void Start()
    {
        // Subscribe to the events for each puzzle base item
        // When event is triggered i.e when the puzzle is completed
        // set the puzzle check to true (Done in puzzle scripts)

        //Add listener to speech trigger for when an object enters the trigger

        //set ghost to inactive
        VikingGhostStatue.SetActive(false);
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerMovement.canMove = true;

        //set light intensity to 0 for all light objects in array
        // Set light intensity to 0 for all light objects in array
        puzzle2Trigger.SetActive(false);

        fire.SetActive(true);
        endOfLevelText.SetActive(false);

    }

    void Update()
    {
        if (puzzle1Check && !puzzle1Complete)
        // if puzzle 1 is Solved , then run event , set puzzle complete to true
        // and set puzzle Check back to false so it doesn't run again
        {
            Debug.Log("Puzzle 1 Complete");
            Puzzle1Completed();
        }

        if (puzzle2Check && !puzzle2Complete)
        // if puzzle 2 is Solved , then run event , set puzzle complete to true
        {
            Puzzle2Completed();
        }

        if (puzzle3Check && !puzzle3Complete)
        // if puzzle 3 is Solved , then run event , set puzzle complete to true
        {
            Puzzle3Completed();
        }

        if (roomComplete)
        {
            // if all puzzles are solved, then run event
            RoomCompleted();
        }

        if (hasRing)
        {
            puzzle2Trigger.SetActive(true);

        }
    }

    void Puzzle1Completed()
    {
        // What to do when Puzzle 1 is complete
        // play you found me from the jarl which is element 8 in the array of jarl voice lines, if already playing then wait for it to finish
        Debug.Log("Puzzle 1 Complete, Attempting to play audio");
        if (jarlAudioSource.isPlaying)
        {
            Debug.Log("Jarl is already speaking, waiting for him to finish");
            StartCoroutine(WaitForJarlSpeech());
        }
        else
        {
            Debug.Log("Jarl is not speaking, playing audio");
            jarlAudioSource.clip = jarlVoiceLines[9];
            jarlAudioSource.Play();
            //when jarl is done speaking set variables
            puzzle1Complete = true;
            puzzle1Check = false;
        }
    }

    void Puzzle2Completed()
    {
        if (hasRing && puzzle2Check == true)
        {
            Debug.Log("Puzzle 2 Complete");
            //play jarl voice line 14 unless already playing then wait
            if (jarlAudioSource.isPlaying)
            {
                StartCoroutine(WaitForJarlSpeech());
            }
            jarlAudioSource.clip = jarlVoiceLines[14];
            jarlAudioSource.Play();
            // What to do when Puzzle 2 is complete
            puzzle2Complete = true;
            puzzle2Check = false;}
            if (jarlAudioSource.isPlaying)
            {
                StartCoroutine(WaitForJarlSpeech());
            }
            //GameObject.Find("Skeleton").SendMessage("ActivateSkeleton");
            jarlAudioSource.clip = jarlVoiceLines[15];
            jarlAudioSource.Play();
    }

    void Puzzle3Completed()
    {

        // What to do when Puzzle 3 is complete
        puzzle3Complete = true;
        puzzle3Check = false;
        //play jarl voice line 1 unless already playing then wait
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        jarlAudioSource.clip = jarlVoiceLines[1];
    }

    void RoomCompleted()
    {
        // What to do when Room is complete
        IncrementProgress(1);
        playerMovement.canMove = false;
        //play jarl voice line 2 unless already playing then wait
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        jarlAudioSource.clip = jarlVoiceLines[5];
        jarlAudioSource.Play();
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        //Display end of level text
        endOfLevelText.SetActive(true);
        //if player presses primary button, load MainMenu
        if (Input.GetButtonDown("PrimaryButton"))
        {
            SceneManager.LoadScene("MainMenu");
        // Load next scene
        }
    }

    private void IncrementProgress(int puzzlesCompleted)
    {
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("DataPersistenceManager instance is null!");
        }
        else if (DataPersistenceManager.instance.GameData == null)
        {
            Debug.LogError("GameData on DataPersistenceManager instance is null!");
        }
        else
        {
            if (DataPersistenceManager.instance.GameData.playerProgress < puzzlesCompleted)
            // if player has completed more puzzles than the current progress , save progress
            {
                DataPersistenceManager.instance.GameData.playerProgress++;
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Progress incremented to: " + DataPersistenceManager.instance.GameData.playerProgress);
            }
        }

    }

    // Handles trigger events from child colliders
    public void HandleTriggerEnter(int colliderID)
    {
        switch (colliderID)
        {
            case 1: // ID for puzzle 1 collider
                Debug.Log("Ragnars Speech Triggered");

                if (ragnarAudioSource != null)
                {
                    StartCoroutine(PlayRagnarsSpeechAndHandleStatue());

                }
                else
                {
                    Debug.LogWarning("AudioSource or Clip not properly assigned.");
                }
                // Handle Puzzle 1 trigger
                break;
            case 2: // ID for puzzle 2 collider
                Debug.Log("Puzzle 2 Triggered");
                // Handle Puzzle 2 trigger
                break;
            // Add more cases as needed
            default:
                Debug.LogWarning("Unknown collider triggered");
                break;
        }
    }

    private IEnumerator PlayRagnarsSpeechAndHandleStatue()
    {
        playerMovement.canMove = false; // Stop player movement
        VikingGhostStatue.SetActive(true);
        fire.SetActive(true);
        Destroy(ragnarSpeechObject); // Destroy the speech trigger object
        ragnarAudioSource.clip = ragnarSpeech[0]; // Assign the first clip in the array
        ragnarAudioSource.Play(); // Play the audio

        // Fade out lights
        foreach (Light lightSource in lightSources)
        {
            StartCoroutine(LightFade(lightSource, 0, 2)); // Fade to 0 intensity over 2 seconds
            SetFireObjectsInactive();
        }

        // Wait while the audio is playing
        while (ragnarAudioSource.isPlaying)
        {
            yield return null; // Wait until next frame
        }

        // Fade lights back in
        foreach (Light lightSource in lightSources)
        {
            StartCoroutine(LightFade(lightSource, 1, 2)); // Fade back to full intensity over 2 seconds
            SetFireObjectsActive();
        }
        VikingGhostStatue.SetActive(false);
        playerMovement.canMove = true; // Allow player movement again
                                       // Wait 10 seconds after Ragnar's speech before playing Jarl's speech
        yield return new WaitForSeconds(10);
        PlayJarlSpeech(); // Start playing Jarl's speech
    }

    private void PlayJarlSpeech()
    {
        // Assuming jarlAudioSource and jarlSpeech are already assigned
        jarlAudioSource.clip = jarlSpeech;
        jarlAudioSource.Play();
    }

    public void PlayJarlVoiceLine(int lineIndex)
    {
        if (jarlAudioSource.isPlaying)
        {
            StartCoroutine(WaitForJarlSpeech());
        }
        jarlAudioSource.clip = jarlVoiceLines[lineIndex];
        jarlAudioSource.Play();
    }

    private IEnumerator LightFade(Light light, float targetIntensity, float duration)
    {
        float startIntensity = light.intensity;
        float time = 0;

        while (time < duration)
        {
            Debug.Log("Fading light");
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity;
    }

    private IEnumerator WaitForJarlSpeech()
    {
        //stall until jarl speech is done, once finished , wait 3 seconds
        while (jarlAudioSource.isPlaying)
        {
            yield return null;
        }
        Debug.Log("Jarl has finished speaking, waiting 3 seconds");
        yield return new WaitForSeconds(1);
    }

    //set fire objects to active
    public void SetFireObjectsActive()
    {
        foreach (GameObject fireObject in fireObjects)
        {
            GameObject parent = fireObject.transform.parent.gameObject;
            // loop through all children of the parent object and set twinkle component on each to active if present and not already active
            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<twinkle>() != null && !child.GetComponent<twinkle>().enabled)
                {
                    torchMaterial.EnableKeyword("_EMISSION");
                    child.GetComponent<twinkle>().enabled = true;
                }
            }


            fireObject.SetActive(true);
        }
    }
    //set fire objects to inactive
    public void SetFireObjectsInactive()
    {
        foreach (GameObject fireObject in fireObjects)
        {
            GameObject parent = fireObject.transform.parent.gameObject;
            Debug.Log("Parent: " + parent.name);
            if (parent.GetComponent<twinkle>() != null)
            {
                parent.GetComponent<twinkle>().enabled = false;
            }
            foreach (Transform child in parent.transform)
            {
                if (child.GetComponent<Renderer>() != null)
                {
                    Debug.Log("Child: " + child.name);
                    //set emission to 0
                    child.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                }
                fireObject.SetActive(false);
            }
        }

    }
}
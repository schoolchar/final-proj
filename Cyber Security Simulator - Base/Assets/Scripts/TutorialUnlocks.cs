using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUnlockManager : MonoBehaviour
{
    //vars to store the unlocks 
    private bool originalCombatUnlocked;
    private bool originalParkourUnlocked;
    private bool originalEscapeRoomUnlocked;

    //says if we are in the tutorial scene or not, true is yes
    private bool isTutorialScene = false;



    //does this on start and destroy. gets the scene loaded. and then again on onDestroy 
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "tutorial")
        {
            EnterTutorial();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "tutorial")
        {
            ExitTutorial();
        }
    }

    private void EnterTutorial()
    {

        //takes old unlock states and saves
        originalCombatUnlocked = gameManager.instance.combatUnlocked;
        originalParkourUnlocked = gameManager.instance.parkourUnlocked;
        originalEscapeRoomUnlocked = gameManager.instance.escapeRoomUnlocked;

        // sets everything to unlocks
        gameManager.instance.combatUnlocked = true;
        gameManager.instance.parkourUnlocked = true;
        gameManager.instance.escapeRoomUnlocked = true;

        //sets bool to say we in tutorial
        isTutorialScene = true;
    }

    private void ExitTutorial()
    {
        // if we are not
        if (!isTutorialScene) return;

        // sets unlocks back to what they were saved as
        gameManager.instance.combatUnlocked = originalCombatUnlocked;
        gameManager.instance.parkourUnlocked = originalParkourUnlocked;
        gameManager.instance.escapeRoomUnlocked = originalEscapeRoomUnlocked;

        //says we are not in tutorial
        isTutorialScene = false;
    }
}
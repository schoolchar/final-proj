using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakerPanel : MonoBehaviour
{
    private int numOfSwitches = 10;
    private bool[] switches = new bool[10] {false, false, false, false, false, false, false, false, false, false }; //What the breaker panel starts at
    private bool[] solution = new bool[10] {true, false, false, false,true, true, false, true, false, true }; //Temp solution, can and should be changed around
    [SerializeField] private Button[] switchSprites;

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    [SerializeField] private TriggerBreakerPanel trigger;

    [Header("Controller")]
    public bool breakerActive;
    private int currentSwitch = 0;
    private bool canInput = true;


    private void Update()
    {
        ControllerChangeSwitch();
        ControllerClickSwitch();
    }

    /// <summary>
    /// Each switch has an assigned index corresponding to the array, attach this function to buttons on switchs, assign parameters
    /// </summary>
    public void ClickSwitch(int _index)
    {
        //Turns on and off
        switches[_index] = !switches[_index];
        ChangeSprite(_index);
        CheckSolution();
    } //END ClickSwitch()
    

    private void CheckSolution()
    {
        //Check if the arrays are the same
        for(int i = 0; i < numOfSwitches; i++)
        {
            //If they are not, return
            if (switches[i] != solution[i])
            {
                return;
            }
        }

        //If they are the same, solve puzzle
        Debug.Log("Solved puzzle");
        trigger.ExitCutsceneB();
    }

    ///<summary>
    ///Change sprite of button depending on on/off state of the switch
    ///</summary>
    void ChangeSprite(int _index)
    {
        if (switches[_index])
        {
            switchSprites[_index].image.sprite = onSprite;
        }
        else
        {
            switchSprites[_index].image.sprite = offSprite;
        }
    } //END ChangeSprite()

    void ControllerClickSwitch()
    {
        if(breakerActive)
        {
            if(Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ClickSwitch(currentSwitch);
            }

        }
    }

    void ControllerChangeSwitch()
    {
        if(Input.GetAxis("Dpad Horizontal") == 1 && canInput)
        {
            switchSprites[currentSwitch].image.transform.localScale = Vector3.one;
            if(currentSwitch + 1 <= 9)
            {
                currentSwitch++;
                switchSprites[currentSwitch].image.transform.localScale += (Vector3.one * 0.5f);

            }
                

            canInput = false;
            StartCoroutine(StopInput());
        }
        else if(Input.GetAxis("Dpad Horizontal") == -1 && canInput)
        {
            switchSprites[currentSwitch].image.transform.localScale = Vector3.one;
            if (currentSwitch - 1 >= 0)
            {
                currentSwitch--;
                switchSprites[currentSwitch].image.transform.localScale += (Vector3.one * 0.5f);
            }
                

            canInput = false;
            StartCoroutine(StopInput());
        }
        else if(Input.GetAxis("Dpad Vertical") == 1 && canInput)
        {
            switchSprites[currentSwitch].image.transform.localScale = Vector3.one;
            if (currentSwitch - 5 >= 0)
            {
                currentSwitch -= 5;
                switchSprites[currentSwitch].image.transform.localScale += (Vector3.one * 0.5f);
            }
               

            canInput = false;
            StartCoroutine(StopInput());
        }
        else if(Input.GetAxis("Dpad Vertical") == -1 && canInput)
        {
            switchSprites[currentSwitch].image.transform.localScale = Vector3.one;
            if (currentSwitch + 5 <= 9)
            {
                currentSwitch += 5;
                switchSprites[currentSwitch].image.transform.localScale += (Vector3.one * 0.5f);
            }
                

            canInput = false;
            StartCoroutine(StopInput());
        }

        Debug.Log(currentSwitch);
    }

    IEnumerator StopInput()
    {
        yield return new WaitForSeconds(0.3f);
        canInput = true;
    }

} //END BreakerPanel.cs

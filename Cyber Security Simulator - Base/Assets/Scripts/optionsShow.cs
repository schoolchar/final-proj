using UnityEngine;

public class GameModeToggleManager : MonoBehaviour
{
    [Header("Invincibility GameObjects")]
    public GameObject invincibleON;
    public GameObject invincibleOFF;

    [Header("Speedrun GameObjects")]
    public GameObject speedON;
    public GameObject speedOFF;

    private void Update()
    {

        if (gameManager.instance.invincibility)
        {
            invincibleON.SetActive(true);
            invincibleOFF.SetActive(false);
        }
        else
        {
            invincibleON.SetActive(false);
            invincibleOFF.SetActive(true);
        }
        if (gameManager.instance.speedrun)
        {
            speedON.SetActive(true);
            speedOFF.SetActive(false);
        }
        else
        {
            speedON.SetActive(false);
            speedOFF.SetActive(true);
        }
    }
}
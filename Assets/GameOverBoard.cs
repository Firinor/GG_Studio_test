using TMPro;
using UnityEngine;
using Zenject;

public class GameOverBoard : MonoBehaviour
{
    [Inject]
    private GameManager gameManager;

    [SerializeField]
    private TextMeshProUGUI text;

    void OnEnable()
    {
        string winer = gameManager.GetWinerName();

        if(string.IsNullOrEmpty(winer))
        {
            //throw new System.Exception("The game over screen should be shown only if there is a winner!");
            gameObject.SetActive(false);
            return;
        }

        text.text = $"{winer} becomes the winner!!!";
    }
}

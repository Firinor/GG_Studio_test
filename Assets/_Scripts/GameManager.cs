using Buffs;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BuffCore[] buffsPool;
    [SerializeField]
    private Unit leftUnit;
    [SerializeField]
    private Unit rightUnit;
    [SerializeField]
    private GameObject GameOverBoard;

    public BoolReactiveProperty IsGameOver;

    private Unit currentActiveUnit;
    private Unit firstActiveUnit;
    public IntReactiveProperty RoundCount;

    private void Awake()
    {
        FirstTurn();
    }

    private void FirstTurn()
    {
        RoundCount.Value = 1;

        firstActiveUnit = leftUnit;
        currentActiveUnit = firstActiveUnit;
        currentActiveUnit.OnStartTurn();
    }

    public BuffCore GetRandomBuffCore(BuffCore[] filter = null)
    {
        int index;

        if (filter == null)
        {
            index = CardGame.GetRandomCardFromTheDeck(buffsPool.Length);
            return buffsPool[index];

        }
        else
        {
            var filtedBuffsPool = buffsPool.Except(filter).ToArray();

            index = CardGame.GetRandomCardFromTheDeck(filtedBuffsPool.Length);
            return filtedBuffsPool[index];
        }
    }

    public void GameOver()
    {
        DeactivateAll();
        IsGameOver.Value = true;
    }
    private void DeactivateAll()
    {
        leftUnit.DeactivateAll();
        rightUnit.DeactivateAll();
    }
    public string GetWinerName()
    {
        if (!leftUnit.IsDead && rightUnit.IsDead)
            return leftUnit.gameObject.name;

        if (leftUnit.IsDead && !rightUnit.IsDead)
            return rightUnit.gameObject.name;

        return null;
    }

    public void NextTurn()
    {
        currentActiveUnit = currentActiveUnit == leftUnit ? rightUnit : leftUnit;

        if(currentActiveUnit == firstActiveUnit)
            RoundCount.Value++;

        currentActiveUnit.OnStartTurn();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

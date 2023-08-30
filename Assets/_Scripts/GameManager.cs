using Buffs;
using System.Linq;
using UnityEngine;
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
        GameOverBoard.SetActive(true);
    }

    public string GetWinerName()
    {
        if (!leftUnit.IsDead && rightUnit.IsDead)
            return leftUnit.gameObject.name;

        if (leftUnit.IsDead && !rightUnit.IsDead)
            return rightUnit.gameObject.name;

        return null;
    }

    private void DeactivateAll()
    {
        leftUnit.DeactivateAll();
        rightUnit.DeactivateAll();
    }
}

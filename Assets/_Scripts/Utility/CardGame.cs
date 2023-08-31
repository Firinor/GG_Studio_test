using Random = UnityEngine.Random;

namespace Utility
{
    public static class CardGame
    {
        public static int GetRandomCardFromTheDeck(int DeckLength)
        {
            return Random.Range(0, DeckLength);
        }
    }
}
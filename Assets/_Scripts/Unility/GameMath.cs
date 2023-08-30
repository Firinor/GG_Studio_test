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
    public static class Ratios
    {
        public static float PercentagesInRates(float percentage)
        {
            return percentage / 100f;
        }

        public static float RatesInPercentages(float rate)
        {
            return rate * 100f;
        }

        public static float ReduceByPercentage(float value, float percentage)
        {
            return value *= 1 - PercentagesInRates(percentage);
        }
    }
}
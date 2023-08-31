namespace Utility
{
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
            return value * (1 - PercentagesInRates(percentage));
        }
    }
}

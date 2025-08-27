namespace RideMobility.Api.Services.Helpers
{
    public static class FareCalculator
    {
        public static decimal CalculateFare(double distanceKm)
        {
            decimal baseFare = 50;
            decimal perKmRate = 10;
            return baseFare + (decimal)distanceKm * perKmRate;
        }
    }
}

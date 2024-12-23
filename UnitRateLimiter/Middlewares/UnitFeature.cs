namespace UnitRateLimiter.Middlewares
{
    sealed class UnitFeature(string? unit) : IUnitFeature
    {
        public string? Unit { get; } = unit;
    }
}

namespace UnitLimiter.Middlewares
{
    sealed class LimiterUnitFeature(string? unit) : ILimiterUnitFeature
    {
        public string? Unit { get; } = unit;
    }
}

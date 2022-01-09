namespace ArrangeContext.Moq.Tests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SystemUnderTestWithSealedDependency
    {
        public SystemUnderTestWithSealedDependency(
            // ReSharper disable once UnusedParameter.Local
            SealedDependency dependency)
        {
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SealedDependency
    {
        
    }
}
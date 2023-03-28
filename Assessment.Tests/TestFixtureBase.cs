namespace Assessment.Tests
{
    using AutoFixture;
    using Moq;

    public abstract class TestFixtureBase
    {
        protected MockRepository Mocks { get; }

        public Fixture Data { get; }

        protected TestFixtureBase()
        {
            this.Mocks = new MockRepository(MockBehavior.Strict);

            this.Data = new Fixture();
        }

        public void Verify()
        {
            this.Mocks.VerifyAll();
        }
    }
}

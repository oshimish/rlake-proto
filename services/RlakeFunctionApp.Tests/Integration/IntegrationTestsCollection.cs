﻿namespace RlakeFunctionApp.Tests.Integration
{
    [CollectionDefinition(Name)]
    public class IntegrationTestsCollection : ICollectionFixture<TestsInitializer>
    {
        public const string Name = nameof(IntegrationTestsCollection);
    }
}

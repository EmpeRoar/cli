﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Microsoft.DotNet.TestFramework;
using Microsoft.DotNet.Tools.Test.Utilities;
using Xunit;

namespace Microsoft.DotNet.Tools.Publish.Tests
{
    public class PublishAppWithDependencies : TestBase
    {
        [Fact]
        public void PublishTestAppWithContentPackage()
        {
            var testInstance = TestAssetsManager.CreateTestInstance("TestAppWithContentPackage")
                .WithLockFiles();

            var publishDir = Publish(testInstance);

            publishDir.Should().HaveFiles(new[]
            {
                "TestAppWithContentPackage.exe",
                "TestAppWithContentPackage.dll",
                "TestAppWithContentPackage.deps.json"
            });

            // these files come from the contentFiles of the SharedContentA dependency
            publishDir
                .Sub("scripts")
                .Should()
                .Exist()
                .And
                .HaveFile("run.cmd");

            publishDir
                .Should()
                .HaveFile("config.xml");
        }

        private DirectoryInfo Publish(TestInstance testInstance)
        {
            var publishCommand = new PublishCommand(testInstance.TestRoot);
            var publishResult = publishCommand.Execute();

            publishResult.Should().Pass();

            return publishCommand.GetOutputDirectory(portable: false);
        }
    }
}

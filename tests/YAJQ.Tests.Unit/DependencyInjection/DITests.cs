﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YAJQ.Core.JobHandler;
using YAJQ.Core.JobHandler.Interfaces;
using YAJQ.Core.JobQueue;
using YAJQ.Core.JobQueue.Interfaces;
using YAJQ.Core.Persistence;
using YAJQ.Core.Persistence.Interfaces;
using YAJQ.DependencyInjection;

namespace YAJQ.Tests.Unit.DependencyInjection;

public class DITests
{
    [Fact]
    public void AllRequiredServicesAreRegistered()
    {
        //Arrange
        var provider = new ServiceCollection().AddLogging().AddYAJQ().BuildServiceProvider();

        //Act
        var queue = provider.GetService<IJobQueue>();
        var handler = provider.GetService<IJobHandler>();
        var repo = provider.GetService<IJobRepository>();
        var persistence = provider.GetService<IJobPersistence>();

        //Assert
        queue.Should().NotBeNull();
        handler.Should().NotBeNull();
        repo.Should().NotBeNull();
        persistence.Should().NotBeNull();
    }
}
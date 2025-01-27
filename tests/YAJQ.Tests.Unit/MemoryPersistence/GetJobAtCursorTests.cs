﻿using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;
using YAJQ.Core.Archive.Interfaces;

namespace YAJQ.Tests.Unit.MemoryPersistence;

public class GetJobAtCursorTests : TestBase
{
    [Fact]
    public async Task ReturnsJobAtCursor()
    {
        //Arrange
        var job = PersistedSyncJob();
        var arch = Substitute.For<IJobArchive>();
        var pers = new Persistence.Memory.MemoryPersistence(arch);
        await pers.SaveJobAsync(job);

        //Act
        var result = await pers.GetJobAtCursorAsync();

        //Arrange
        result.WasSuccess.Should().BeTrue();
        result.Value.Id.Value.Should().Be(DefaultJobId);
    }

    [Fact]
    public async Task ReturnsBothJobsAfterIncrease()
    {
        //Arrange
        var job1 = PersistedSyncJob();
        var job2 = PersistedSyncJob("ASDF");
        var arch = Substitute.For<IJobArchive>();
        var pers = new Persistence.Memory.MemoryPersistence(arch);
        await pers.SaveJobAsync(job1);
        await pers.SaveJobAsync(job2);

        //Act
        var result1 = await pers.GetJobAtCursorAsync();
        var result2 = await pers.GetJobAtCursorAsync();

        //Arrange
        result1.WasSuccess.Should().BeTrue();
        result1.Value.Id.Value.Should().Be(DefaultJobId);

        result2.WasSuccess.Should().BeTrue();
        result2.Value.Id.Value.Should().Be("ASDF");
    }
}
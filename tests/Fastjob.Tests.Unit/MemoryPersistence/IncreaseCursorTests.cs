﻿using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fastjob.Tests.Unit.MemoryPersistence;

public class IncreaseCursorTests : TestBase
{
    [Fact]
    public async Task EmptyCollectionCursorStaysAtZero()
    {
        //Arrange
        var pers = new Persistence.Memory.MemoryPersistence();

        //Act
        var initialCursor = await pers.GetCursorAsync();
        var increasedCursor = await pers.IncreaseCursorAsync();

        //Assert
        initialCursor.WasSuccess.Should().BeTrue();
        increasedCursor.WasSuccess.Should().BeTrue();
        initialCursor.Value.CurrentCursor.Should().Be(initialCursor.Value.CurrentCursor);
    }

    [Fact]
    public async Task IncreasesCursorByOne()
    {
        //Arrange
        var pers = new Persistence.Memory.MemoryPersistence();
        await pers.SaveJobAsync(PersistedSyncJob());

        //Act
        var cursor = await pers.IncreaseCursorAsync();

        //Assert
        cursor.WasSuccess.Should().BeTrue();
        cursor.Value.CurrentCursor.Should().Be(1);
        cursor.Value.MaxCursor.Should().Be(1);
    }

    [Fact]
    public async Task ResetsMaxCursorAtOverflow()
    {
        //Arrange
        var pers = new Persistence.Memory.MemoryPersistence();
        await pers.SaveJobAsync(PersistedSyncJob());
        await pers.SaveJobAsync(PersistedSyncJob());

        //Act
        await pers.IncreaseCursorAsync();
        var cursor = await pers.IncreaseCursorAsync();

        //Assert
        cursor.WasSuccess.Should().BeTrue();
        cursor.Value.CurrentCursor.Should().Be(1);
        cursor.Value.MaxCursor.Should().Be(2);
    }
}
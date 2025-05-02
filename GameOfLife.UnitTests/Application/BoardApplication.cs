using GameOfLife.Common.Mapper;
using GameOfLife.Domain;
using GameOfLife.Domain;
using GameOfLife.Persistence;
using GameOfLife.Applications;
using Moq;
using Xunit;

namespace GameOfLife.UnitTests.Application;

public class BoardApplication
{
    private readonly BoolMatrixMapper _boolMatrixMapper = new();
    private readonly Mock<IBoardPersistence> _repositoryMock = new();
    private readonly BoardSimulator _boardSimulator = new();
    private readonly Applications.BoardApplication _service;

    public BoardApplication()
    {
        _service = new Applications.BoardApplication(_repositoryMock.Object, _boolMatrixMapper, _boardSimulator);
    }

    [Fact]
    public async Task NextGeneration_ShouldApplyRulesCorrectly()
    {
        var id = Guid.NewGuid();
        var board = new Board
        {
            Id = id,
            Step = 0,
            State = new bool[,]
            {
                { false, true, false },
                { false, true, false },
                { false, true, false }
            }
        };

        _repositoryMock.Setup(r => r.GetBoardAsync(id)).ReturnsAsync(board);

        bool[,] expected = new bool[,]
        {
            { false, false, false },
            { true,  true,  true },
            { false, false, false }
        };

        var result = await _service.NextGeneration(id);

        Assert.Equal(expected, result!.State);
        Assert.Equal(1, result.Step);
        _repositoryMock.Verify(r => r.UpdateBoardAsync(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public async Task Advance_ShouldAdvanceMultipleGenerations()
    {
        var id = Guid.NewGuid();
        var initial = new bool[,]
        {
            { false, true, false },
            { false, true, false },
            { false, true, false }
        };

        var board = new Board { Id = id, Step = 0, State = initial };
        _repositoryMock.Setup(r => r.GetBoardAsync(id)).ReturnsAsync(board);

        var result = await _service.Advance(id, 2);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Step);
        _repositoryMock.Verify(r => r.UpdateBoardAsync(It.IsAny<Board>()), Times.Once);
    }

    [Fact]
    public async Task FindFinalState_ShouldDetectStableState()
    {
        var id = Guid.NewGuid();
        bool[,] stable = new bool[,]
        {
            { false, false, false },
            { false, true,  true },
            { false, true,  true }
        };

        var board = new Board { Id = id, Step = 0, State = stable };
        _repositoryMock.Setup(r => r.GetBoardAsync(id)).ReturnsAsync(board);

        var (result, isStable) = await _service.FindFinalState(id, 10);

        Assert.True(isStable);
        Assert.NotNull(result);
        Assert.Equal(1, result!.Step);
    }

    [Fact]
    public async Task FindFinalState_ShouldDetectUnstableState()
    {
        var id = Guid.NewGuid();
        bool[,] unstable = new bool[,]
        {
            { false, true, false },
            { false, true, false },
            { false, true, false }
        };

        var board = new Board { Id = id, Step = 0, State = unstable };
        _repositoryMock.Setup(r => r.GetBoardAsync(id)).ReturnsAsync(board);

        var (result, isStable) = await _service.FindFinalState(id, 2);

        Assert.False(isStable);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task NextGeneration_ShouldReturnNull_IfBoardNotFound()
    {
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetBoardAsync(id)).ReturnsAsync((Board?)null);

        var result = await _service.NextGeneration(id);

        Assert.Null(result);
        _repositoryMock.Verify(r => r.UpdateBoardAsync(It.IsAny<Board>()), Times.Never);
    }

    [Fact]
    public async Task SaveBoardAsync_ShouldInitializeStepZero()
    {
        var state = new bool[3, 3];
        var saved = new Board(state);

        _repositoryMock.Setup(r => r.SaveBoardAsync(It.IsAny<Board>()))
            .ReturnsAsync(saved);

        var result = await _service.SaveBoardAsync(state);

        Assert.NotNull(result);
        Assert.Equal(0, result.Step);
        Assert.Equal(saved.State, result.State);
    }
}

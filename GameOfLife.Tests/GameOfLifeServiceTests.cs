using GameOfLife.Common.Mapper;
using GameOfLife.Repository;
using GameOfLife.Services;

namespace GameOfLife.Tests
{
    public class GameOfLifeServiceTests
    {
        private readonly BoolMatrixMapper _boolMatrixMapper = new();
        private readonly Mock<IBoardRepository> _repositoryMock = new Mock<IBoardRepository>();
        private readonly GameOfLifeService _service;

        public GameOfLifeServiceTests()
        {
            _service = new GameOfLifeService(_repositoryMock.Object, _boolMatrixMapper);
        }

        [Fact]
        public async Task NextGeneration_ShouldApplyRulesCorrectly()
        {
            var id = Guid.NewGuid();
            _repositoryMock.Setup(s => s.GetBoardAsync(id)).ReturnsAsync(new Model.Board
            {
                Id = id,
                Step = 0,
                State = new bool[,]
                    {
                        { false, true, false },
                        { false, true, false },
                        { false, true, false }
                    }
            });

            bool[,] expected = new bool[,]
            {
                { false, false, false },
                { true,  true,  true },
                { false, false, false }
            };

            var next = await _service.NextGeneration(id);
            Assert.Equal(expected, next!.State);
        }

        [Fact]
        public async Task Advance_ShouldAdvanceMultipleGenerations()
        {
            var id = Guid.NewGuid();
            var state = new bool[,]
            {
                { false, true, false },
                { false, true, false },
                { false, true, false }
            };
            var model = new Model.Board
            {
                Id = id,
                Step = 0,
                State = state
            };
            _repositoryMock.Setup(s => s.GetBoardAsync(id)).ReturnsAsync(model);

            var final = await _service.Advance(id, 2);
            Assert.Equal(model.State, state);
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
            var model = new Model.Board
            {
                Id = id,
                Step = 0,
                State = stable
            };
            _repositoryMock.Setup(s => s.GetBoardAsync(id)).ReturnsAsync(model);

            var (final, isStable) = await _service.FindFinalState(id, max: 10);

            Assert.True(isStable);
            Assert.Equal(stable, final!.State);
            Assert.Equal(1, final!.Step);
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
            var model = new Model.Board
            {
                Id = id,
                Step = 0,
                State = unstable
            };
            _repositoryMock.Setup(s => s.GetBoardAsync(id)).ReturnsAsync(model);

            var (final, isStable) = await _service.FindFinalState(id, max: 2);

            Assert.False(isStable);
        }
    }

}

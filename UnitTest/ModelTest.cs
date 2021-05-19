using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaciLaci4.Model;
using MaciLaci4.Persistence;
using System;

namespace UnitTest
{
    [TestClass]
    public class ModelTest
    {
        private BearGameModel _model;
        private BearGameBoard _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = new BearGameBoard();
            _model = new BearGameModel();
            _board = _model.LoadFields(Properties.Resources.mediumMap);
            
            
            _model.GameAdvanced += new EventHandler<BearGameEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<BearGameEventArgs>(Model_GameOver);
        }
        
        [TestMethod]
        public void NewGameMediumTest()
        {
            _model.NewGame();

            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(GameSize.Medium, _model.GameSize);

            int emptyFields = 0;
            int trees = 0;
            int guards = 0;
            int baskets = 0;
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                    if (_model.Board[i, j] == 0)
                        emptyFields++;
                    else if (_model.Board[i, j] == 1)
                        trees++;
                    else if (_model.Board[i, j] == 2)
                        guards++;
                    else if (_model.Board[i, j] == 3)
                        baskets++;

            Assert.AreEqual(203, emptyFields);
            Assert.AreEqual(13, trees);
            Assert.AreEqual(3, guards);
            Assert.AreEqual(5, baskets);
        }

        [TestMethod]
        public void NewGameSmallTest()
        {
            _model.GameSize = GameSize.Small;
            _model.NewGame();

            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(GameSize.Small, _model.GameSize);

            int emptyFields = 0;
            int trees = 0;
            int guards = 0;
            int baskets = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (_model.Board[i, j] == 0)
                        emptyFields++;
                    else if (_model.Board[i, j] == 1)
                        trees++;
                    else if (_model.Board[i, j] == 2)
                        guards++;
                    else if (_model.Board[i, j] == 3)
                        baskets++;

            Assert.AreEqual(85, emptyFields);
            Assert.AreEqual(9, trees);
            Assert.AreEqual(2, guards);
            Assert.AreEqual(3, baskets);
        }

        [TestMethod]
        public void NewGameLargeTest()
        {
            _model.GameSize = GameSize.Large;
            _model.NewGame();

            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(GameSize.Large, _model.GameSize);

            int emptyFields = 0;
            int trees = 0;
            int guards = 0;
            int baskets = 0;
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if (_model.Board[i, j] == 0)
                        emptyFields++;
                    else if (_model.Board[i, j] == 1)
                        trees++;
                    else if (_model.Board[i, j] == 2)
                        guards++;
                    else if (_model.Board[i, j] == 3)
                        baskets++;

            Assert.AreEqual(368, emptyFields);
            Assert.AreEqual(19, trees);
            Assert.AreEqual(5, guards);
            Assert.AreEqual(7, baskets);
        }

        [TestMethod]
        public void MoveTest()
        {
            _model.GameSize = GameSize.Medium;
            Assert.AreEqual(0, _model.BasketCount);
            _model.NewGame();

            _model.MoveBear(3);
          
            Assert.AreEqual(1, _model.BearPosition.X);
            Assert.AreEqual(0, _model.BearPosition.Y);
            Assert.AreEqual(0, _model.GameTime);

            _model.MoveBear(4);

            Assert.AreEqual(1, _model.BearPosition.X);
            Assert.AreEqual(1, _model.BearPosition.Y);
            Assert.AreEqual(0, _model.GameTime);

            _model.MoveBear(1);

            Assert.AreEqual(0, _model.BearPosition.X);
            Assert.AreEqual(1, _model.BearPosition.Y);
            Assert.AreEqual(0, _model.GameTime);

            _model.MoveBear(2);

            Assert.AreEqual(0, _model.BearPosition.X);
            Assert.AreEqual(0, _model.BearPosition.Y);
            Assert.AreEqual(0, _model.GameTime);
        }
        /*
        [TestMethod]
        public void AdvanceTimeTest()
        {
            _model.NewGame();

            Int32 time = _model.GameTime;
            while (time != 10)
            {
                time++;
                Assert.AreEqual(time, _model.GameTime);
                Assert.AreEqual(0, _model.BasketCount);
            }
            Assert.AreEqual(10, _model.GameTime);
        }*/
        [TestMethod]
        public void MoveGuardsTest()
        {
            _model.NewGame();

            foreach (var guard in _model.Guards)
            {
                Assert.AreEqual(2, _model.Board[guard.Position.X,guard.Position.Y]);
            }

            _model.MoveGuards();

            foreach (var guard in _model.Guards)
            {
                Assert.AreEqual(0, _model.Board[guard.Position.X, guard.Position.Y]);
            }
            Assert.AreEqual(0, _model.BasketCount);
            Assert.AreEqual(0, _model.GameTime);
        }

        private void Model_GameAdvanced(Object sender, BearGameEventArgs e)
        {
            Assert.IsTrue(_model.GameTime >= 0);
            Assert.AreEqual(e.BasketCount, _model.BasketCount);
            Assert.AreEqual(e.GameTime, _model.GameTime);
            Assert.IsFalse(e.IsWon);
        }

        private void Model_GameOver(Object sender, BearGameEventArgs e)
        {
            Assert.IsTrue(_model.IsGameWon);
        }
    }
}
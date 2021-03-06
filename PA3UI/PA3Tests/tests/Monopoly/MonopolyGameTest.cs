﻿using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA3BackEnd.src.Monopoly;

namespace PA3Tests.tests.Monopoly
{
    [TestClass]
    public class MonopolyGameTest
    {
        /// <summary>
        /// This test tests if GetDevelopmentValue returns the development value of the property specified,
        /// should be -2 if the tile specified is not a property
        /// </summary>
        [TestMethod]
        public void GetDevelopmentValueTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            var value = mg.GetDevelopmentValue(11);
            Assert.AreEqual(0, value);
            var dneValue = mg.GetDevelopmentValue(45);
            Assert.AreEqual(-2, dneValue);

        }

        [TestMethod]
        public void NextPlayersTurnTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            var roll = mg.CanRoleDice;
            Assert.AreEqual(false, roll);

            //round 1, player turn 1
            mg.NextPlayersTurn();
            roll = mg.CanRoleDice;
            Assert.AreEqual(true, roll);
            var currentPlayer = mg.currentPlayerID;
            Assert.AreEqual(1, currentPlayer);
            var round = mg.Round;
            Assert.AreEqual(1, round);
            //player 2
            mg.NextPlayersTurn();
            currentPlayer = mg.currentPlayerID;
            Assert.AreEqual(2, currentPlayer);
            //round 2 player 1
            mg.NextPlayersTurn();
            currentPlayer = mg.currentPlayerID;
            Assert.AreEqual(1, currentPlayer);
            round = mg.Round;
            Assert.AreEqual(2, round);
        }

        [TestMethod]
        public void GetUserTokenNameTest()
        {

            Assert.AreEqual("Shoe", MonopolyGame.GetUserTokenName(1));
            Assert.AreEqual("Thimble", MonopolyGame.GetUserTokenName(2));
            Assert.AreEqual("Car", MonopolyGame.GetUserTokenName(3));
            Assert.AreEqual("TopHat", MonopolyGame.GetUserTokenName(4));
            Assert.AreEqual("", MonopolyGame.GetUserTokenName(5));

        }

        [TestMethod]
        public void ApplyDevelopPropertyTest()
        {
            MonopolyGame mg = new MonopolyGame(4);
            mg.NextPlayersTurn();
            //no outstanding developments available
            var value = mg.ApplyDevelopProperty();
            Assert.AreEqual(-1, value);

            //not enough money
            for (var i = 1; i <= 40; i++)
            {
                if (i == 30)
                {
                    mg.DiceRoll(2, 2, out _);
                }
                else
                {
                    mg.DiceRoll(1, 0, out _);
                }

                try
                {
                    mg.BuyProperty();
                }
                catch
                {
                    // ignored
                }
            }
            mg.DevelopProperty(3);
            Assert.AreEqual(-2, mg.ApplyDevelopProperty());

            MonopolyGame newGame = new MonopolyGame(2);
            newGame.NextPlayersTurn();
            newGame.DiceRoll(1, 0, out _);
            newGame.BuyProperty();

            newGame.DiceRoll(1, 1, out _);
            newGame.BuyProperty();

            newGame.DevelopProperty(3);
            newGame.DevelopProperty(3);
            newGame.DevelopProperty(3);

            Assert.AreEqual(0, newGame.ApplyDevelopProperty());

            mg.CompleteTrade(new List<int>(), new List<int>(), -10000, 0, 1);

            mg.DevelopProperty(3);
            mg.DevelopProperty(3);
            mg.DevelopProperty(3);
            mg.DevelopProperty(3);
            mg.ApplyDevelopProperty();

            mg.DevelopProperty(6);
            mg.DevelopProperty(6);
            mg.DevelopProperty(6);
            mg.DevelopProperty(6);
            mg.ApplyDevelopProperty();

            mg.DevelopProperty(11);
            mg.DevelopProperty(11);
            mg.DevelopProperty(11);
            mg.DevelopProperty(11);
            mg.ApplyDevelopProperty();

            mg.DevelopProperty(16);
            mg.DevelopProperty(16);
            mg.DevelopProperty(16);
            mg.DevelopProperty(16);

            Assert.AreEqual(-3, mg.ApplyDevelopProperty());

        }

        /// <summary>
        /// Tests the CalculateMoneyAndHousesNeeded method
        /// </summary>
        [TestMethod]
        public void CalculateMoneyAndHousesNeededTest()
        {
            Street.InitializeHousesAndHotels();
            var mg = new MonopolyGame(2);
            int m;
            int h;
            int H;
            mg.CalculateMoneyAndHousesNeeded(out m, out h, out H);
            Assert.AreEqual(0, m);
            Assert.AreEqual(0, h);
            Assert.AreEqual(0, H);

            mg.NextPlayersTurn();
            mg.DiceRoll(1, 0, out _);
            mg.BuyProperty();
            mg.DiceRoll(1, 1, out _);
            mg.BuyProperty();

            mg.UnDevelopProperty(1);
            mg.CalculateMoneyAndHousesNeeded(out m, out h, out H);
            Assert.AreEqual(-30, m);
            Assert.AreEqual(0, h);
            Assert.AreEqual(0, H);
            mg.ApplyDevelopProperty();

            mg.DevelopProperty(1);
            mg.DevelopProperty(1);
            mg.DevelopProperty(1);
            mg.DevelopProperty(1);
            mg.DevelopProperty(1);
            mg.CalculateMoneyAndHousesNeeded(out m, out h, out H);
            Assert.AreEqual(380, m);
            Assert.AreEqual(7, h);
            Assert.AreEqual(0, H);
            mg.DevelopProperty(1);
            mg.ApplyDevelopProperty();

            mg.UnDevelopProperty(1);
            mg.CalculateMoneyAndHousesNeeded(out m, out h, out H);
            Assert.AreEqual(-50, m);
            Assert.AreEqual(4, h);
            Assert.AreEqual(-1, H);

            mg.UnDevelopProperty(1);
            mg.UnDevelopProperty(1);
            mg.UnDevelopProperty(1);
            mg.UnDevelopProperty(1);
            mg.UnDevelopProperty(1);
            mg.CalculateMoneyAndHousesNeeded(out m, out h, out H);
            Assert.AreEqual(-480, m);
            Assert.AreEqual(-4, h);
            Assert.AreEqual(-1, H);
        }

        [TestMethod]
        public void UnDevelopPropertyTest()
        {
            MonopolyGame monoGame = new MonopolyGame(2);
            monoGame.NextPlayersTurn();
            monoGame.UnDevelopProperty(3);
            monoGame.ApplyDevelopProperty();
            Assert.AreEqual(-1, monoGame.GetDevelopmentValue(3));

            monoGame.NextPlayersTurn();
            //build 'uneven' forcing all properties in group to have 1 house
            monoGame.DevelopProperty(6);
            monoGame.DevelopProperty(6);
            //apply development
            monoGame.ApplyDevelopProperty();
            //get rid of both testing if it has undeveloped evenly
            monoGame.UnDevelopProperty(6);
            monoGame.UnDevelopProperty(6);
            monoGame.ApplyDevelopProperty();

            //test if both in group are even
            Assert.AreEqual(0, monoGame.GetDevelopmentValue(8));


            monoGame.UnDevelopProperty(8);
            monoGame.ApplyDevelopProperty();
            monoGame.UnDevelopProperty(8);
            monoGame.ApplyDevelopProperty();

            Assert.AreEqual(-1, monoGame.GetDevelopmentValue(8));
        }

        [TestMethod]
        public void CalculateHighestPlayerScoreTest()
        {
            Street.InitializeHousesAndHotels();
            MonopolyGame polyGame = new MonopolyGame(2);
            polyGame.NextPlayersTurn();
            polyGame.NextPlayersTurn();

            //test if player 0 wins with score 1500 (No change in anyones score)
            int playerId;
            int score;
            polyGame.CalculateHighestPlayerScore(out playerId, out score);
            Assert.AreEqual(0, playerId);
            Assert.AreEqual(1500, score);

            polyGame.DiceRoll(1, 0, out _);
            polyGame.BuyProperty();
            polyGame.DiceRoll(1, 1, out _);
            polyGame.BuyProperty();

            polyGame.DevelopProperty(3);
            polyGame.DevelopProperty(3);
            polyGame.DevelopProperty(3);
            polyGame.DevelopProperty(3);
            polyGame.DevelopProperty(3);
            polyGame.ApplyDevelopProperty();

            polyGame.NextPlayersTurn();
            polyGame.DiceRoll(1, 2, out _);
            polyGame.PayRent(out _, out _, out _, out var a);
            a.Invoke(null, null);

            polyGame.CalculateHighestPlayerScore(out playerId, out score);
            Assert.AreEqual(1890, score);
            Assert.AreEqual(1, playerId);

        }

        [TestMethod]
        public void PayRentTest()
        {
            MonopolyGame mp = new MonopolyGame(2);
            mp.NextPlayersTurn();

            mp.DiceRoll(1, 0, out _);
            mp.BuyProperty();

            mp.NextPlayersTurn();

            mp.DiceRoll(1, 0, out _);
            mp.PayRent(out _, out _, out _, out var a);
            a.Invoke(null, null);

            Assert.AreEqual(1498, mp.GetBalanceOfPlayer(1));

            //Check Utility Rent
            mp.NextPlayersTurn();
            mp.DiceRoll(0, 0, out _);
            mp.DiceRoll(6, 5, out _);
            mp.BuyProperty();

            mp.NextPlayersTurn();
            mp.DiceRoll(6, 5, out _);

            mp.PayRent(out _, out _, out _, out var b);
            b.Invoke(null, null);

            Assert.AreEqual((1498 - 44), mp.GetBalanceOfPlayer(1));

        }
        [TestMethod]
        public void CanBuyTest()
        {
            MonopolyGame mg = new MonopolyGame(2);
            mg.NextPlayersTurn();

            for (int i = 1; i <= 40; i++)
            {
                if (i == 30)
                {
                    mg.DiceRoll(2, 2, out _);
                }
                else
                {
                    mg.DiceRoll(1, 0, out _);
                }

                try
                {
                    mg.BuyProperty();
                }
                catch
                {

                }
            }

            int l = mg.currentsPlayerLocation;

            Assert.AreEqual(false, mg.CanBuy(out _, out _));
        }

        /// <summary>
        /// Tests if BuyProperty correctly subtracts the cost of the property 
        /// from the players balance, as well as adding the property to the 
        /// list of properties owned by the player
        /// </summary>
        [TestMethod]
        public void BuyPropertyTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            //Player may buy unowned property, 
            //cost is subtracted from player balance,
            //property added to players list of owned properties
            mg.NextPlayersTurn();
            mg.DiceRoll(1, 2, out _);

            mg.BuyProperty();
            Assert.AreEqual(1440, mg.GetBalanceOfPlayer(0));

            var propertyList = mg.GetPropertiesOwnedByPlayer();
            Assert.AreEqual(1, propertyList.Count);
            Assert.AreEqual(3, propertyList[0]);

            //Non owner must pay rent
            mg.NextPlayersTurn();
            Assert.AreEqual(7, mg.DiceRoll(1, 2, out _));
        }


        /// <summary>
        /// Tests if GetPropertiesOwnedByPlayer returns the correct 
        /// list of properties owned by a player
        /// </summary>
        [TestMethod]
        public void GetPropertiesOwnedByPlayerTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            //Player 1 does not own property location = 3
            mg.NextPlayersTurn();
            mg.DiceRoll(1, 2, out _);
            var propertyList = mg.GetPropertiesOwnedByPlayer();
            Assert.AreEqual(0, propertyList.Count);

            //Player 2 owns 1 property location = 3
            mg.NextPlayersTurn();
            mg.DiceRoll(1, 2, out _);
            mg.BuyProperty();
            var propertyList1 = mg.GetPropertiesOwnedByPlayer();
            Assert.AreEqual(1, propertyList1.Count);
            Assert.AreEqual(3, propertyList1[0]);

            //Player 1 owns multiple properties location = 11
            mg.NextPlayersTurn();
            mg.DiceRoll(3, 3, out _);
            mg.BuyProperty();
            mg.DiceRoll(1, 1, out _);
            mg.BuyProperty();
            var propertyList2 = mg.GetPropertiesOwnedByPlayer();
            Assert.AreEqual(2, propertyList2.Count);
            Assert.AreEqual(9, propertyList2[0]);
            Assert.AreEqual(11, propertyList2[1]);

        }

        /// <summary>
        /// Tests if GetNameOfProperty returns the correct name for a piece of property, 
        /// should return an empty string if field is not property
        /// </summary>
        [TestMethod]
        public void GetNamesForPropertyTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            //valid properties
            var medAve = mg.GetNameOfProperty(1);
            var boardwalk = mg.GetNameOfProperty(39);
            var rr = mg.GetNameOfProperty(5);
            var ec = mg.GetNameOfProperty(12);
            //Non properties
            var go = mg.GetNameOfProperty(0);
            var tax = mg.GetNameOfProperty(4);
            var card = mg.GetNameOfProperty(7);

            Assert.AreEqual("Mediterranean Ave", medAve);
            Assert.AreEqual("Boardwalk", boardwalk);
            Assert.AreEqual("Reading Railroad", rr);
            Assert.AreEqual("Electric Company", ec);
            Assert.AreEqual("", go);
            Assert.AreEqual("", tax);
            Assert.AreEqual("", card);
        }

        /// <summary>
        /// Tests if GetPriceOfProperty returns the correct price for a piece of property, 
        /// should return -1 if current field is not property
        /// </summary>
        [TestMethod]
        public void GetPriceOfPropertyTest()
        {
            MonopolyGame mg = new MonopolyGame(2);


            //valid properties
            var medAve = mg.GetPriceOfProperty(1);
            var boardwalk = mg.GetPriceOfProperty(39);
            var rr = mg.GetPriceOfProperty(5);
            var ec = mg.GetPriceOfProperty(12);
            //non properties
            var go = mg.GetPriceOfProperty(0);
            var tax = mg.GetPriceOfProperty(4);
            var card = mg.GetPriceOfProperty(7);

            Assert.AreEqual(60, medAve);
            Assert.AreEqual(400, boardwalk);
            Assert.AreEqual(200, rr);
            Assert.AreEqual(150, ec);
            Assert.AreEqual(-1, go);
            Assert.AreEqual(-1, tax);
            Assert.AreEqual(-1, card);
        }

        /// <summary>
        /// Tests whether or not a piece of property has any buildings on it
        /// </summary>
        [TestMethod]
        public void HasAnyBuildingsOnItTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            //valid properties
            var medAve = mg.HasAnyBuildingsOnIt(1);
            var boardwalk = mg.HasAnyBuildingsOnIt(39);
            var rr = mg.HasAnyBuildingsOnIt(5);
            var ec = mg.HasAnyBuildingsOnIt(12);
            //non properties
            var go = mg.HasAnyBuildingsOnIt(0);
            var tax = mg.HasAnyBuildingsOnIt(4);
            var card = mg.HasAnyBuildingsOnIt(7);

            Assert.IsFalse(medAve);
            Assert.IsFalse(boardwalk);
            Assert.IsFalse(rr);
            Assert.IsFalse(ec);
            Assert.IsFalse(go);
            Assert.IsFalse(tax);
            Assert.IsFalse(card);
        }

        /// <summary>
        /// Tests if CompleteTrade correctly trades the amount of money 
        /// and properties traded between two players
        /// </summary>
        [TestMethod]
        public void CompleteTradeTest()
        {
            MonopolyGame mg = new MonopolyGame(2);

            mg.NextPlayersTurn();
            mg.DiceRoll(3, 2, out _);
            mg.BuyProperty();
            var propList1 = mg.GetPropertiesOwnedByPlayer(0);
            var balance1 = mg.GetBalanceOfPlayer(0);

            mg.NextPlayersTurn();
            mg.DiceRoll(3, 3, out _);
            mg.BuyProperty();
            var propList2 = mg.GetPropertiesOwnedByPlayer(1);
            var balance2 = mg.GetBalanceOfPlayer(1);

            mg.CompleteTrade(propList1, propList2, 100, 0, 1);


            Assert.AreEqual(propList2.Count, mg.GetPropertiesOwnedByPlayer(0).Count);
            Assert.AreEqual(propList1.Count, mg.GetPropertiesOwnedByPlayer(1).Count);
            foreach (var prop in mg.GetPropertiesOwnedByPlayer(0))
            {
                Assert.IsTrue(propList2.Contains(prop));
            }
            foreach (var prop in mg.GetPropertiesOwnedByPlayer(1))
            {
                Assert.IsTrue(propList1.Contains(prop));
            }

            Assert.AreEqual(balance1 - 100, mg.GetBalanceOfPlayer(0));
            Assert.AreEqual(balance2 + 100, mg.GetBalanceOfPlayer(1));
        }

        /// <summary>
        /// Tests if Complete Bid sets the right owner of the property 
        /// and subtracts the right amount of money from the player
        /// </summary>
        [TestMethod]
        public void CompleteBidTest()
        {
            // Setup
            MonopolyGame mg = new MonopolyGame(2);
            mg.NextPlayersTurn();
            mg.DiceRoll(2, 3, out _);

            // Test
            mg.CompleteBid(1, 100);

            // Assert
            var propList = mg.GetPropertiesOwnedByPlayer(1);
            var balance = mg.GetBalanceOfPlayer(1);

            Assert.AreEqual(1400, balance);
            var propertyList = mg.GetPropertiesOwnedByPlayer(1);
            Assert.AreEqual(1, propertyList.Count);
            Assert.AreEqual(5, propertyList[0]);

            mg.NextPlayersTurn();
            Assert.AreEqual(0, mg.DiceRoll(2, 3, out _));
        }

        [TestMethod]
        public void TestDiceRolle()
        {
            MonopolyGame mg = new MonopolyGame(2);
            mg.NextPlayersTurn();

            RoutedEventHandler action;
            // first double rolle
            Assert.AreEqual(0, mg.DiceRoll(1, 1, out action));
            Assert.IsNull(action);
            Assert.AreEqual(2, mg.currentsPlayerLocation);
            Assert.AreEqual(true, mg.CanRoleDice);
            Assert.AreEqual(false, mg.CanEndTurn);

            // seccond double rolle
            Assert.AreEqual(6, mg.DiceRoll(1, 1, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(4, mg.currentsPlayerLocation);
            action.Invoke(null, null);
            Assert.AreEqual(1300, mg.GetBalanceOfPlayer(0));
            Assert.AreEqual(true, mg.CanRoleDice);
            Assert.AreEqual(false, mg.CanEndTurn);

            // third double rolle player should go to prison
            Assert.AreEqual(4, mg.DiceRoll(1, 1, out action));
            Assert.IsNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            // player is in prison (first round), does not need to pay the fine yet, both player pays fine and does not pay fine is tested.
            Assert.AreEqual(3, mg.DiceRoll(1, 3, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(1300, mg.GetBalanceOfPlayer(0));
            action.Invoke(false, null);
            Assert.AreEqual(1300, mg.GetBalanceOfPlayer(0));
            action.Invoke(true, null);
            Assert.AreEqual(1250, mg.GetBalanceOfPlayer(0));
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            // player lands on free parking
            Assert.AreEqual(0, mg.DiceRoll(5, 5, out action));
            Assert.IsNull(action);
            Assert.AreEqual(20, mg.currentsPlayerLocation);
            Assert.AreEqual(true, mg.CanRoleDice);
            Assert.AreEqual(false, mg.CanEndTurn);

            // player lands on go to prison
            Assert.AreEqual(9, mg.DiceRoll(5, 5, out action));
            Assert.IsNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            // player gets out of prison by rolling doubles
            Assert.AreEqual(2, mg.DiceRoll(5, 5, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);
            action.Invoke(null, null);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            // player needs to pay tax 100
            Assert.AreEqual(5, mg.DiceRoll(14, 14, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(38, mg.currentsPlayerLocation);
            Assert.AreEqual(true, mg.CanRoleDice);
            Assert.AreEqual(false, mg.CanEndTurn);
            Assert.AreEqual(1250, mg.GetBalanceOfPlayer(0));
            action.Invoke(null, null);
            Assert.AreEqual(1150, mg.GetBalanceOfPlayer(0));

            // player lands on go to prison
            Assert.AreEqual(9, mg.DiceRoll(16, 16, out action));
            Assert.IsNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            //player does not rolle doubles and needs to pay the fine
            Assert.AreEqual(3, mg.DiceRoll(1, 2, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);
            action.Invoke(false, null);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            Assert.AreEqual(3, mg.DiceRoll(1, 2, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);
            action.Invoke(false, null);

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();

            Assert.AreEqual(1, mg.DiceRoll(1, 2, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(10, mg.currentsPlayerLocation);
            Assert.AreEqual(false, mg.CanRoleDice);
            Assert.AreEqual(true, mg.CanEndTurn);
            action.Invoke(null, null);
            Assert.AreEqual(1300, mg.GetBalanceOfPlayer(0));

            mg.NextPlayersTurn();
            mg.NextPlayersTurn();
        }
    }
}
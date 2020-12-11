﻿using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA3BackEnd.src.Monopoly;

namespace PA3Tests.tests.Monopoly
{
    [TestClass]
    public class MonopolyGameTest
    {
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
            Assert.AreEqual(true,roll);
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
           /* MonopolyGame mg = new MonopolyGame(4);
            //no outstanding developments available
            var value = mg.ApplyDevelopProperty();
            Assert.Equals(-1, value);
            //player does not have enough money to develop
            mg.currentPlayerBalance.*/
        }

        [TestMethod]
        public void CalculateMoneyAndHousesNeededTest()
        {
            
        }

        [TestMethod]
        public void DevelopPropertyTest()
        {
            
        }

        [TestMethod]
        public void BuyPropertyTest() 
        {
            MonopolyGame mg = new MonopolyGame(2);
            
            //Player may buy unowned property, 
            //cost is subtracted from player balance,
            //property added to players list of owned properties
            mg.NextPlayersTurn();
            mg.DiceRole(1, 2, out _);

            mg.BuyProperty();
            Assert.AreEqual(1440, mg.GetBalanceOfPlayer(0));

            var propertyList = mg.GetPropertiesOwnedByPlayer();
            Assert.AreEqual(1, propertyList.Count);
            Assert.AreEqual(3, propertyList[0]);

            //Non owner must pay rent
            mg.NextPlayersTurn();
            Assert.AreEqual(7, mg.DiceRole(1, 2, out _));
        }
        
        [TestMethod]
        public void GetPropertiesOwnedByPlayerTest() 
        {

        }
        
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
        
        [TestMethod]
        public void CompleteTradeTest() 
        {
            MonopolyGame mg = new MonopolyGame(2);
        }

        [TestMethod]
        public void CompleteBidTest()
        {
            // Setup
            MonopolyGame mg = new MonopolyGame(2);
            mg.NextPlayersTurn();
            mg.DiceRole(2, 3, out _);

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
            Assert.AreEqual(0, mg.DiceRole(2, 3, out _));
        }

        [TestMethod]
        public void TestDiceRole()
        {
            MonopolyGame mg = new MonopolyGame(2);
            mg.NextPlayersTurn();

            RoutedEventHandler action;
            Assert.AreEqual(0, mg.DiceRole(1, 1, out action));
            Assert.IsNull(action);
            Assert.AreEqual(2, mg.currentsPlayerLocation);

            Assert.AreEqual(6, mg.DiceRole(1, 1, out action));
            Assert.IsNotNull(action);
            Assert.AreEqual(4, mg.currentsPlayerLocation);
            action.Invoke(null, null);
            Assert.AreEqual(1300, mg.GetBalanceOfPlayer(0));

        }
    }
}
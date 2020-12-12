﻿using System;
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
          /*  MonopolyGame mg = new MonopolyGame(4);
            //no outstanding developments available
            var value = mg.ApplyDevelopProperty();
            Assert.Equals(-1, value);
            //player does not have enough money to develop
            mg.currentPlayer.subtractBalance(1000);
            //player has enough money and
        */
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
        public void UnDevelopPropertyTest()
        {
            MonopolyGame monoGame = new MonopolyGame(2);
            monoGame.NextPlayersTurn();
            monoGame.UnDevelopProperty(3);
            monoGame.ApplyDevelopProperty();
            Assert.AreEqual(-1,monoGame.GetDevelopmentValue(3));
            
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
            Assert.AreEqual(0,monoGame.GetDevelopmentValue(8));

            
            monoGame.UnDevelopProperty(8);
            monoGame.ApplyDevelopProperty();
            monoGame.UnDevelopProperty(8);
            monoGame.ApplyDevelopProperty();
            
            Assert.AreEqual(-1,monoGame.GetDevelopmentValue(8));
        }

        [TestMethod]
        public void BuyPropertyTest() 
        {
             
        }
        
        [TestMethod]
        public void GetPropertiesOwnedByPlayerTest() 
        {

        }
        
        [TestMethod]
        public void GetNamesForPropertyTest() 
        {
            MonopolyGame mg = new MonopolyGame(2);

            var medAve = mg.GetNameOfProperty(1);
            var boardwalk = mg.GetNameOfProperty(39);
            var rr = mg.GetNameOfProperty(5);
            var ec = mg.GetNameOfProperty(12);
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

            var medAve = mg.GetPriceOfProperty(1);
            var boardwalk = mg.GetPriceOfProperty(39);
            var rr = mg.GetPriceOfProperty(5);
            var ec = mg.GetPriceOfProperty(12);
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

            var medAve = mg.HasAnyBuildingsOnIt(1);
            var boardwalk = mg.HasAnyBuildingsOnIt(39);
            var rr = mg.HasAnyBuildingsOnIt(5);
            var ec = mg.HasAnyBuildingsOnIt(12);
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

    }
}
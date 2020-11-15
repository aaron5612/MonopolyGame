﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PA3UI.ui
{
    /// <summary>
    /// Interaction logic for MonopolyGame.xaml
    /// </summary>
    public partial class MonopolyGame : UserControl
    {
        private Board board;
        private Rectangle cover;
        private RoutedEventHandler routedEventHandler;

        public MonopolyGame(int players, int timerTime)
        {
            InitializeComponent();
            board = new Board(players);
            board.SetValue(Grid.ColumnProperty, 1);
            board.SetValue(Grid.RowProperty, 2);
            mainGrid.Children.Add(board);

            Ch.Add_EventHandler(viewDeed_OnClick);

            cover = new Rectangle();
            cover.SetValue(Grid.RowSpanProperty, 3);
            cover.SetValue(Grid.ColumnSpanProperty, 3);
            cover.Fill = Brushes.White;
            cover.Opacity = 0.5;


            timeElapsed = 0;
            this.timerTime = timerTime;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0,1,0);
            timer.Tick += Timer_Elapsed;
            SetRemainingTime(timerTime);
            timer.Start();

            rnd = new Random();
            RoleDices();

            InitilizeData(players);

        }

        private void SetRemainingTime(int timeLeft) 
        {
            if (timeLeft < 0)
            {
                timeLeft = 0;
            }

            if (timeLeft > 60)
            {
                textBlockTimer.Text = $"Time Left: {timeLeft / 60}h {timeLeft % 60}min";
            }
            else
            {
                textBlockTimer.Text = $"Time Left: {timeLeft}min";
            }
        }

        private void LoadPlayerDataTopBar() 
        {
            var playersToken = GetUserTokenName(currentsPlayerTurn);

            textBlockPlayersTurn.Text = $"Player: {currentsPlayerTurn + 1 } ({playersToken})";
            textBlockRound.Content = $"Round: {Round}";
            textBlockMoney.Content = $"${players[currentsPlayerTurn].balance}";
        }

        private void EndGame()
        {
            timer.Stop();
            SetRemainingTime(0);
        }

        private void Button_Resign_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EndGame();
        }

        private void viewDeed_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ShowDialogBox() 
        {
            try
            {
                mainGrid.Children.Add(cover);
            }
            catch 
            {
                Debug.Fail("Adding the cover failed!");    
            }
        }

        private void ShowDialogBoxBidding(string msg, int min, int max, RoutedEventHandler routedEvent) 
        {
            ShowDialogBox();
            routedEventHandler = routedEvent;
            var dialogBox = Dialog.ShowBidingDialog(msg, min, max, RemoveDialogBox);
            dialogBox.SetValue(Grid.RowProperty, 2);
            dialogBox.SetValue(Grid.ColumnProperty, 1);
            mainGrid.Children.Add(dialogBox);
        }

        private void ShowDialogBoxOK(string msg, RoutedEventHandler routedEvent) 
        {
            ShowDialogBox();
            routedEventHandler = routedEvent;
            var dialogBox = Dialog.ShowOKDialog(msg, RemoveDialogBox);
            dialogBox.SetValue(Grid.RowProperty, 2);
            dialogBox.SetValue(Grid.ColumnProperty, 1);
            mainGrid.Children.Add(dialogBox);
        }

        private void ShowDialogBoxYesNo(string msg, RoutedEventHandler routedEvent)
        {
            ShowDialogBox();
            routedEventHandler = routedEvent;
            var dialogBox = Dialog.ShowYesNoDialog(msg, RemoveDialogBox);
            dialogBox.SetValue(Grid.RowProperty, 2);
            dialogBox.SetValue(Grid.ColumnProperty, 1);
            mainGrid.Children.Add(dialogBox);
        }

        private void RemoveDialogBox(object sender, RoutedEventArgs args)
        {
            mainGrid.Children.Remove(cover);
            mainGrid.Children.Remove((UIElement)sender);
            if (routedEventHandler != null)
            {
                var tempEvent = routedEventHandler;
                routedEventHandler = null;
                tempEvent.Invoke(sender, args);
            }
        }

        private void RoleDices(object sender, RoutedEventArgs e)
        {
            cover.Opacity = 0;
            ShowDialogBox();
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            int i = 0;
            timer.Tick += (object s, EventArgs args) => 
            {
                i++;
                if (i < 20)
                {
                    RoleDices();
                }
                else 
                {
                    mainGrid.Children.Remove(cover);
                    cover.Opacity = 0.5;
                    int x = RandomRole();
                    int y = RandomRole();
                    DiceOne.Source = new BitmapImage(new Uri(IntRoleToTexture(x), UriKind.RelativeOrAbsolute));
                    DiceTwo.Source = new BitmapImage(new Uri(IntRoleToTexture(y), UriKind.RelativeOrAbsolute));
                    DiceRole(x, y);
                    timer.Stop();
                }
            };
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RoleDices() 
        {
            DiceOne.Source = new BitmapImage(new Uri(IntRoleToTexture(RandomRole()), UriKind.RelativeOrAbsolute));
            DiceTwo.Source = new BitmapImage(new Uri(IntRoleToTexture(RandomRole()), UriKind.RelativeOrAbsolute));
        }

        private int RandomRole() 
        {
            return rnd.Next(1, 7);
        }

        private string IntRoleToTexture(int x) 
        {
            switch (x) 
            {
                case 1:
                    return "/Assets/Dice/dice_one.png";
                case 2:
                    return "/Assets/Dice/dice_two.png";
                case 3:
                    return "/Assets/Dice/dice_three.png";
                case 4:
                    return "/Assets/Dice/dice_four.png";
                case 5:
                    return "/Assets/Dice/dice_five.png";
                case 6:
                    return "/Assets/Dice/dice_six.png";
            }
            return "";
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DiceButtonGrid.Height = DiceButton.ActualHeight;
            DiceButtonGrid.Width = DiceButton.ActualWidth;
        }

        private void EndTurnButton_Click(object sender, RoutedEventArgs e)
        {
            HideEndTurnButton();
            NextPlayersTurn();
        }

        private void HideEndTurnButton()
        { 
            EndTurnButton.IsEnabled = false;
            EndTurnButton.Visibility = Visibility.Hidden;
        }

        private void ShowEndTurnButton()
        {
            EndTurnButton.IsEnabled = true;
            EndTurnButton.Visibility = Visibility.Visible;
        }
    }
}

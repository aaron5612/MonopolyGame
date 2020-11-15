﻿using System.Windows;
using System.Windows.Controls;

namespace PA3UI.ui
{
    public partial class CardHolder : UserControl
    {
        private RoutedEventHandler eHandler;
        
        public CardHolder()
        {
            InitializeComponent();
            
        }

        public void Add_Deed(int id)
        {
            var deed = new Deed(id);
            deed.AddEvent_Handler(eHandler);
            
            mainGrid.Children.Add(deed);
        }

        public void Add_EventHandler(RoutedEventHandler eventHandler)
        {
            eHandler = eventHandler;
        }
        
        
    }
}
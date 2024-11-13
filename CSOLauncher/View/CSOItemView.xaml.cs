using CSODataCore;
using CSOLauncher.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace CSOLauncher.View
{
    public sealed partial class CSOItemView : UserControl
    {
        public CSOItemViewModel ViewModel
        {
            get { return (CSOItemViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(CSOItemViewModel),
            typeof(CSOItemView),
            new PropertyMetadata(null)
        );

        public CSOItemView()
        {
            this.InitializeComponent();
            var data = new ItemData(ItemManager.Search(148)[0], 1, ItemManager.Search(744)[0], ItemManager.EmptyItem, ItemManager.EmptyItem)
            {
                ReinforceData = new()
                {
                    Damage = 0,
                    Accuracy = 0,
                    Weight = 0,
                    Rebound = 0,
                    Repeatedly = 0,
                    Ammo = 0,
                    OverDmg = 0,
                }
            };
            ViewModel = new(data);
        }
    }
}

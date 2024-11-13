using CommunityToolkit.Mvvm.ComponentModel;
using CSODataCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using System.Text;
using static CSOLauncher.Launcher;

namespace CSOLauncher.ViewModels
{
    public partial class CSOItemViewModel : ObservableObject
    {
        private const string EmtpyPaint = "WeaponPaintRemove";
        private const string Paint = "paint";
        private const string WeaponPaint = "weaponpaint";
        private const string CSOItemName = "CSO_Item_Name_";
        private const string CSO_WpnTitle = "CSO_WpnTitle_";
        private static readonly string[] ReinfocreTitle5 = ["D5", "A5", "K5", "W5", "S5", "B5"];
        private static readonly string[] ReinfocreTitle3 = ["D3", "A3", "K3", "W3", "S3", "B3"];
        private const char Space = ' ';
        public static readonly WriteableBitmap PartIcon = Launcher.Assets["partsweapon_icon"];
        private static readonly string[] Colors = ["White", "#" ];

        private const int LeftOffset = 192;
        private const int RightOffset = 213;

        public ItemData ItemData;

        /// <summary>
        /// 附加属性
        /// </summary>
        [ObservableProperty]
        private CSOPartViewModel? _CSOPartViewModel1;
        [ObservableProperty]
        private CSOPartViewModel? _CSOPartViewModel2;
        [ObservableProperty]
        private CSOPaintViewModel? _CSOPaintViewModel;
        [ObservableProperty]
        private CSOReinforceViewModel? _CSOReinforceViewModel;

        /// <summary>
        /// 名称
        /// </summary>

        public string CSOName => GetName();
        [ObservableProperty]
        private string _CSONameColor;

        public CSOItemViewModel(ItemData data)
        {
            Changed = new(OnChanged);
            ChangedUI = new(OnChangedUI);
            ItemData = data;
            if(data.Item.Infomation != null)
            {
                if((int)data.Item.SortingIndex < 10)
                {
                    if (data.Item.Infomation.Part != ItemPart.Disable)
                    {
                        CSOPartViewModel1 = new(data, Changed, CSOPartViewModel.CSOPartSlot.Part1);
                        CSOPartViewModel2 = new(data, Changed, CSOPartViewModel.CSOPartSlot.Part2);
                        PartVisiblity = Visibility.Visible;
                    }
                    if (data.Item.Infomation.Paints != null)
                    {
                        CSOPaintViewModel = new(data, ChangedUI);
                        PaintVisiblity = Visibility.Visible;
                    }
                    if (data.Item.Infomation.Reinforce != null && data.ReinforceData != null)
                    {
                        CSOReinforceViewModel = new(data, ChangedUI);
                        ReinforceVisiblity = Visibility.Visible;
                    }
                }
                if (PaintVisiblity == Visibility.Visible && ReinforceVisiblity == Visibility.Collapsed)
                {
                    PaintOffset = RightOffset;
                }
                else if (PaintVisiblity == Visibility.Collapsed && ReinforceVisiblity == Visibility.Visible)
                {
                    ReinforceOffset = RightOffset;
                }
                else if (PaintVisiblity == Visibility.Visible && ReinforceVisiblity == Visibility.Visible)
                {
                    PaintOffset = LeftOffset;
                    ReinforceOffset = RightOffset;
                }
            }
        }

        [ObservableProperty]
        private Visibility _PartVisiblity = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _PaintVisiblity = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _ReinforceVisiblity = Visibility.Collapsed;
         
        [ObservableProperty]
        private int _PaintOffset;
        [ObservableProperty]
        private int _ReinforceOffset;

        [ObservableProperty]
        private ViewModelPropertyChanged _Changed;
        [ObservableProperty]
        private ViewModelPropertyChanged _ChangedUI;  

        private void OnChangedUI()
        {
            OnPropertyChanged(nameof(CSOName));
        }

        private void OnChanged()
        {

        }

        private string GetName()
        {
            StringBuilder sb = new();
            sb.Append(CSOItemName);
            if (ItemData.Item.TransName != null)
            {
                if ((int)ItemData.Item.SortingIndex < 10)
                {
                    // 涂装判定
                    if (ItemData.Paint.ResourceName != EmtpyPaint)
                    {
                        sb.Append(ItemData.Item.ResourceName);
                        sb.Append(ItemData.Paint.ResourceName.ToLower());
                        sb.Replace(WeaponPaint, Paint);
                        return ItemManager.LanguageDictionary[sb.ToString()];
                    }
                    // 强化命名判定
                    else if (ItemData.Item.Infomation != null)
                    {
                        // 存在可强化
                        if (ItemData.Item.Infomation.Reinforce != null)
                        {
                            sb.Clear();
                            var data = ItemData.ReinforceData;
                            if (data != null)
                            {
                                List<byte> bytes = [data.Damage, data.Accuracy, data.Rebound, data.Weight, data.Repeatedly, data.Ammo];

                                int count = 0;
                                int index = 0;
                                List<int> ints = [];
                                sb.Append(CSO_WpnTitle);
                                foreach (byte b in bytes)
                                {
                                    if (b == 5)
                                    {
                                        sb.Append(ReinfocreTitle5[index]);
                                        return ItemData.Item.TransName + Space + ItemManager.LanguageDictionary[sb.ToString()];
                                    }
                                    index++;
                                }
                                sb.Clear();
                                sb.Append(CSO_WpnTitle);
                                index = 0;
                                foreach (byte b in bytes)
                                {
                                    if (b == 3 && count != 2)
                                    {
                                        ints.Add(index);
                                        count++;
                                    }
                                    index++;
                                }
                                if (ints.Count == 2)
                                {
                                    foreach (int i in ints)
                                    {
                                        sb.Append(ReinfocreTitle3[i]);
                                    }
                                    return ItemData.Item.TransName + Space + ItemManager.LanguageDictionary[sb.ToString()];
                                }
                            }
                            return ItemData.Item.TransName;
                        }
                        else return ItemData.Item.TransName;
                    }
                    else return ItemData.Item.TransName;
                }
                else return ItemData.Item.TransName;
            }
            else return ItemData.Item.Name;
        }
    }
}

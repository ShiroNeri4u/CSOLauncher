using CommunityToolkit.Mvvm.ComponentModel;
using CSODataCore;
using CSOLauncher.DrawMethod;
using CsvHelper.Configuration.Attributes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static CSOLauncher.Launcher;

namespace CSOLauncher.ViewModels
{
    public partial class CSOItemViewModel : ObservableObject
    {
        private const string EmtpyPaint = "WeaponPaintRemove";
        private const string Paint = "paint";
        private const string WeaponPaint = "weaponpaint";
        private const string ItemDir = @"\Item\";
        private const string PNGPostName = ".png";
        private const string CSO_Item_Name = "CSO_Item_Name_";
        private const string CSO_WpnTitle = "CSO_WpnTitle_";
        private static readonly string[] ReinforceTitle5 = ["D5", "A5", "K5", "W5", "S5", "B5"];
        private static readonly string[] ReinforceTitle3 = ["D3", "A3", "K3", "W3", "S3", "B3"];
        private const char Space = ' ';
        private const char UnderLine = '_';
        public static readonly WriteableBitmap PartIcon = Launcher.Assets["partsweapon_icon"];
        private static readonly WriteableBitmap[] ItemBorder = [Launcher.Assets["itembox_grey"], Launcher.Assets["itembox_green"], Launcher.Assets["itembox_blue"], Launcher.Assets["itembox_red"], Launcher.Assets["itembox_gold"], Launcher.Assets["itembox_purple"], Launcher.Assets["itembox_redbrown"]];
        private static readonly string[] ItemBottom = ["#2B2B2B", "#37342D", "#353A2B", "#2B2B3A", "#382B3A", "#1A2529", "#2B2B2B"];
        private static readonly string[] NameColors = ["#000000", "#B39769",  "#B9E469", "#5151CD", "#A919AD", "#00B4FF", "#000000"];

        private const int LeftOffset = 193;
        private const int RightOffset = 212;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CSOItemBorder))]
        [NotifyPropertyChangedFor(nameof(CSOItemBottom))]
        [NotifyPropertyChangedFor(nameof(CSOItemNameColor))]
        private ItemData _CSOItemData;

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

        public string CSOItemName => GetName();
        public string CSOItemNameColor => CSOItemData.Item.ItemGrade == null ? NameColors[0] : NameColors[(int)CSOItemData.Item.ItemGrade - 1];

        /// <summary>
        /// 图片
        /// </summary>
        public WriteableBitmap CSOItemBorder => CSOItemData.Item.ItemGrade == null ? ItemBorder[0] : ItemBorder[(int)CSOItemData.Item.ItemGrade - 1];
        public string CSOItemBottom => CSOItemData.Item.ItemGrade == null ? ItemBottom[0] : ItemBottom[(int)CSOItemData.Item.ItemGrade - 1];
        public BitmapImage CSOItemBitmap => GetItemBitmap();
        public CSOItemViewModel(ItemData data)
        {
            Changed = new(OnChanged);
            ChangedUI = new(OnChangedUI);
            CSOItemData = data;
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
            OnPropertyChanged(nameof(CSOItemName));
            OnPropertyChanged(nameof(CSOItemBitmap));
        }

        private void OnChanged()
        {

        }

        private string GetName()
        {
            string name = GetNamePre();
            if (CSOItemData.ReinforceData != null)
            {
                int count = CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo + CSOItemData.ReinforceData.OverDmg;
                if (count > 0)
                {
                    return $"{name}(+{count})";
                }
            }
            return name;
        }

        private string GetNamePre()
        {
            StringBuilder sb = new();
            if (CSOItemData.Item.TransName != null)
            {
                if ((int)CSOItemData.Item.SortingIndex < 10)
                {
                    // 涂装判定
                    if (CSOItemData.Paint.ResourceName != EmtpyPaint)
                    {
                        sb.Append(CSO_Item_Name);
                        sb.Append(CSOItemData.Item.ResourceName);
                        sb.Append(CSOItemData.Paint.ResourceName.ToLower());
                        sb.Replace(WeaponPaint, Paint);
                        return ItemManager.LanguageDictionary[sb.ToString()];
                    }
                    // 强化命名判定
                    else if (CSOItemData.Item.Infomation != null)
                    {
                        // 存在可强化
                        if (CSOItemData.Item.Infomation.Reinforce != null)
                        {
                            sb.Clear();
                            var data = CSOItemData.ReinforceData;
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
                                        sb.Append(ReinforceTitle5[index]);
                                        return CSOItemData.Item.TransName + Space + ItemManager.LanguageDictionary[sb.ToString()];
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
                                        sb.Append(ReinforceTitle3[i]);
                                    }
                                    return CSOItemData.Item.TransName + Space + ItemManager.LanguageDictionary[sb.ToString()];
                                }
                            }
                            return CSOItemData.Item.TransName;
                        }
                        else return CSOItemData.Item.TransName;
                    }
                    else return CSOItemData.Item.TransName;
                }
                else return CSOItemData.Item.TransName;
            }
            else return CSOItemData.Item.Name;
        }

        private BitmapImage GetItemBitmap()
        {
            BitmapImage bitmap;
            StringBuilder sb = new ();
            if (CSOItemData.Paint.ResourceName != EmtpyPaint)
            {
                sb.Append(Launcher.ResourceDirectory);
                sb.Append(ItemDir);
                sb.Append(CSOItemData.Item.ResourceName);
                sb.Append(CSOItemData.Paint.ResourceName.ToLower());
                sb.Replace(WeaponPaint, Paint);
                sb.Append(PNGPostName);
                if(Launcher.ImageResources.TryGetValue(sb.ToString(), out BitmapImage? bm1))
                {
                    if(bm1 != null)
                    {
                        return bm1;
                    }
                }
                bitmap = new BitmapImage(new Uri(sb.ToString()));
                Launcher.ImageResources.Add(sb.ToString(), bitmap);
                return bitmap;
            }
            else if (CSOItemData.ReinforceData != null)
            {
                if (CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo >= 8)
                {
                    sb.Clear();
                    sb.Append(Launcher.ResourceDirectory);
                    sb.Append(ItemDir);
                    sb.Append(CSOItemData.Item.ResourceName.ToLower());
                    sb.Append(UnderLine);
                    sb.Append(8);
                    sb.Append(PNGPostName);
                    if (File.Exists(sb.ToString()))
                    {
                        if (Launcher.ImageResources.TryGetValue(sb.ToString(), out BitmapImage? bm2))
                        {
                            if (bm2 != null)
                            {
                                return bm2;
                            }
                        }
                        bitmap = new BitmapImage(new Uri(sb.ToString()));
                        Launcher.ImageResources.Add(sb.ToString(), bitmap);
                        return bitmap;
                    }
                    sb.Replace('8', '6');
                    if (File.Exists(sb.ToString()))
                    {
                        if (Launcher.ImageResources.TryGetValue(sb.ToString(), out BitmapImage? bm3))
                        {
                            if (bm3 != null)
                            {
                                return bm3;
                            }
                        }
                        bitmap = new BitmapImage(new Uri(sb.ToString()));
                        Launcher.ImageResources.Add(sb.ToString(), bitmap);
                        return bitmap;
                    }
                }
                else if (CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo >= 6)
                {
                    sb.Clear();
                    sb.Append(Launcher.ResourceDirectory);
                    sb.Append(ItemDir);
                    sb.Append(CSOItemData.Item.ResourceName.ToLower());
                    sb.Append(UnderLine);
                    sb.Append(6);
                    sb.Append(PNGPostName);
                    if (File.Exists(sb.ToString()))
                    {
                        if (Launcher.ImageResources.TryGetValue(sb.ToString(), out BitmapImage? bm4))
                        {
                            if (bm4 != null)
                            {
                                return bm4;
                            }
                        }
                        bitmap = new BitmapImage(new Uri(sb.ToString()));
                        Launcher.ImageResources.Add(sb.ToString(), bitmap);
                        return bitmap;
                    }
                }
            }
            sb.Clear();
            sb.Append(Launcher.ResourceDirectory);
            sb.Append(ItemDir);
            sb.Append(CSOItemData.Item.ResourceName.ToLower());
            sb.Append(PNGPostName);
            if (Launcher.ImageResources.TryGetValue(sb.ToString(), out BitmapImage? bm5))
            {
                if (bm5 != null)
                {
                    return bm5;
                }
            }
            bitmap = new BitmapImage(new Uri(sb.ToString()));
            Launcher.ImageResources.Add(sb.ToString(), bitmap);
            return bitmap;
        }
    }
}

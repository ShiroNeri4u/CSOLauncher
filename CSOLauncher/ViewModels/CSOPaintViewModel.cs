using CommunityToolkit.Mvvm.ComponentModel;
using CSODataCore;
using CSOLauncher.DrawMethod;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using static CSOLauncher.Launcher;

namespace CSOLauncher.ViewModels
{
    public partial class CSOPaintViewModel : ObservableObject
    {
        private const string PreDescName = "CSO_Item_Desc_";
        private const string Paint = "paint";
        private static readonly Dictionary<Item, List<CSOPaintData>> CSOPaintGroupDictionary = [];
        private const string OldNewLine = @"\n";
        private const string NewLine = "\n";
        private const int CSOPaintFlyoutWidth = 245;
        private const CSOFlyout.Color Color = CSOFlyout.Color.Grey;
        public static readonly WriteableBitmap CSOPainttEditorBorder = Launcher.Assets[CSOFlyout.GetAssets(150, 230, Color)];
        public static readonly WriteableBitmap CSOPaintBackground = Launcher.Assets[Paint];
        private static readonly FontFamily CSOFont = (FontFamily)Microsoft.UI.Xaml.Application.Current.Resources["CSOFont"];

        /// <summary>
        /// 涂装属性
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CSOPaintItemGroup))]
        private ItemData _CSOItemData;
        public List<CSOPaintData>? CSOPaintItemGroup => GetPaintData();

        public ViewModelPropertyChanged? Changed;

        public string CSOPaintName => GetName();
        public string CSOPaintDescription => GetDescription();

        /// <summary>
        /// 浮窗属性
        /// </summary>
        [ObservableProperty]
        private bool _CSOPaintFlyoutIsOpen;
        public int CSOPaintFlyoutHeight => GetFlyoutHeight();
        public WriteableBitmap CSOPaintFlyoutBorder => Launcher.Assets[CSOFlyout.GetAssets(CSOPaintFlyoutWidth, CSOPaintFlyoutHeight, Color)];

        /// <summary>
        /// 编辑器属性
        /// </summary>
        [ObservableProperty]
        private bool _CSOPaintEditorIsOpen;

        public CSOPaintViewModel(ItemData data, ViewModelPropertyChanged changed)
        {
            CSOItemData = data;
            Changed = changed;
            Changed += OnChanged;
            CSOPaintFlyoutIsOpen = false;
            CSOPaintEditorIsOpen = false;
        }
        
        public void OnChanged()
        {
            OnPropertyChanged(nameof(CSOPaintName));
            OnPropertyChanged(nameof(CSOPaintDescription));
            OnPropertyChanged(nameof(CSOPaintDescription));
            OnPropertyChanged(nameof(CSOPaintFlyoutHeight));
            OnPropertyChanged(nameof(CSOPaintFlyoutBorder));
        }

        private string GetName()
        {
            return CSOItemData.Paint.TransName ?? CSOItemData.Paint.Name;
        }

        private string GetDescription()
        {
            StringBuilder desc = new();
            desc.Append(ItemManager.LanguageDictionary[PreDescName + CSOItemData.Paint!.ResourceName]);
            desc.Replace(OldNewLine, NewLine);
            return desc.ToString();
        }

        private int GetFlyoutHeight()
        {
            TextBlock textBlock = new()
            {
                Text = CSOPaintDescription,
                Width = CSOPaintFlyoutWidth,
                FontSize = 12,
                CharacterSpacing = 50,
                TextWrapping = TextWrapping.WrapWholeWords,
                FontFamily = CSOFont,
            };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (CSOItemData.Paint != null)
            {
                return (int)textBlock.DesiredSize.Height + 57;
            }
            else
            {
                return 0;
            }
        }

        private List<CSOPaintData> GetPaintData()
        {
            if (CSOItemData.Item.Infomation != null)
            {
                if (CSOItemData.Item.Infomation.Paints != null)
                {
                    if (!CSOPaintGroupDictionary.TryGetValue(CSOItemData.Item, out List<CSOPaintData>? list))
                    {
                        List<CSOPaintData> newList = [];
                        foreach (Item paint in CSOItemData.Item.Infomation.Paints)
                        {
                            CSOPaintData data = new(CSOItemData.Item, paint);
                            newList.Add(data);
                        }
                        CSOPaintGroupDictionary[CSOItemData.Item] = newList;
                        return CSOPaintGroupDictionary[CSOItemData.Item];
                    }
                    else
                    {
                        return list;
                    }
                }
                else return [];
            }
            else return [];
        }
        
    }

    public sealed class CSOPaintData
    {
        private static readonly string[] ColorString = ["c", "b", "a", "s", "ss", "sss", "rb"];
        private const string PreName = "itembox_random";
        private const string Extend = ".png";
        private const string ItemDirectory = "Item";
        private const string Paint = "paint";
        private const string WeaponPaint = "weaponpaint";
        private const char BackSlash = '\\';
        private const char UnderLine = '_';
        private const string CSOItemBoxBorderName = "line";
        private const string CSOItemBoxBottomName = "bg";
        private const string WeaponPaintRemove = "WeaponPaintRemove";

        public Item CSOPaintItem;
        public string CSOPaintName;
        public object? CSOPaintUri;
        public WriteableBitmap CSOItemBoxBorder;
        public WriteableBitmap CSOItemBoxBottom;

        public CSOPaintData(Item item, Item paintitem)
        {
            CSOPaintItem = paintitem;
            CSOPaintName = paintitem.TransName ?? paintitem.Name;
            StringBuilder sb = new();
            sb.Append(PreName);
            sb.Append(UnderLine);
            sb.Append(ColorString[(int)(paintitem.ItemGrade != null ? paintitem.ItemGrade : ItemGrade.None)]);
            sb.Append(UnderLine);
            sb.Append(CSOItemBoxBorderName);
            CSOItemBoxBorder = Launcher.Assets[sb.ToString()];
            sb.Replace(CSOItemBoxBorderName, CSOItemBoxBottomName);
            CSOItemBoxBottom = Launcher.Assets[sb.ToString()];

            if (paintitem.ResourceName != WeaponPaintRemove)
            {
                sb.Clear();
                sb.Append(item.ResourceName.ToLower());
                sb.Append(paintitem.ResourceName.ToLower());
                sb.Replace(WeaponPaint, Paint);
                string uriName = sb.ToString();
                if (Launcher.ImageResources.TryGetValue(uriName, out var bitmap))
                {
                    CSOPaintUri = bitmap;
                }
                else
                {
                    sb.Clear();
                    sb.Append(Launcher.ResourceDirectory);
                    sb.Append(BackSlash);
                    sb.Append(ItemDirectory);
                    sb.Append(BackSlash);
                    sb.Append(uriName);
                    sb.Append(Extend);
                    BitmapImage newbitmap = new(new Uri(sb.ToString()));
                    Launcher.ImageResources.Add(paintitem.ResourceName, newbitmap);
                    CSOPaintUri = newbitmap;
                }
            }
            else
            {
                if (Launcher.ImageResources.TryGetValue(item.ResourceName.ToLower(), out var bitmap))
                {
                    CSOPaintUri = bitmap;
                }
                else
                {
                    sb.Clear();
                    sb.Append(Launcher.ResourceDirectory);
                    sb.Append(BackSlash);
                    sb.Append(ItemDirectory);
                    sb.Append(BackSlash);
                    sb.Append(item.ResourceName.ToLower());
                    sb.Append(Extend);
                    BitmapImage newbitmap = new(new Uri(sb.ToString()));
                    Launcher.ImageResources.Add(paintitem.ResourceName, newbitmap);
                    CSOPaintUri = newbitmap;
                }
            }
        }
    }
}

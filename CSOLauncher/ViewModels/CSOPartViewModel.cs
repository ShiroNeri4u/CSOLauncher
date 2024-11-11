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

namespace CSOLauncher.ViewModels
{
    public partial class CSOPartViewModel : ObservableObject
    {
        private const string PreDescName = "CSO_Item_Desc_";
        private const string EmptyPartDescName = "CSO_WeaponParts_Tooltip_Empty";
        private const string PartBackgroundPostName = "_s";
        private const string EmptySlot = "empty_slot";
        private const string OldNewLine = @"\n";
        private const string NewLine = "\n";
        private const int CSOPartFlyoutWidth = 245;
        private static readonly FontFamily CSOFont = (FontFamily)Microsoft.UI.Xaml.Application.Current.Resources["CSOFont"];
        private const CSOFlyout.Color Color = CSOFlyout.Color.Grey;
        public static readonly WriteableBitmap CSOPartEditorBorder = Launcher.Assets[CSOFlyout.GetAssets(150, 230, Color)];
        public static readonly Dictionary<ItemPart, List<CSOPartData>> CSOPartgroupDictionary = new()
        {
            {ItemPart.Disable, new () },
            {ItemPart.WeaponType, GetData(ItemPart.WeaponType) },
            {ItemPart.KnifeType, GetData(ItemPart.KnifeType) },
            {ItemPart.SpecialType, GetData(ItemPart.SpecialType) },
        };

        /// <summary>
        /// 配件属性
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CSOPartItemGroup))]
        private Item? _CSOItem;
        public List<CSOPartData> CSOPartItemGroup => GetPartType();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CSOPartBackground))]
        [NotifyPropertyChangedFor(nameof(CSOPartNameVisibility))]
        [NotifyPropertyChangedFor(nameof(CSOPartName))]
        [NotifyPropertyChangedFor(nameof(CSOPartDescription))]
        [NotifyPropertyChangedFor(nameof(CSOPartFlyoutHeight))]
        [NotifyPropertyChangedFor(nameof(CSOPartFlyoutBorder))]
        private Item? _CSOPartItem;
        public WriteableBitmap CSOPartBackground => (CSOPartItem ?? ItemManager.EmptyItem).IsEmpty ? Launcher.Assets[EmptySlot] : Launcher.Assets[(CSOPartItem ?? ItemManager.EmptyItem).ResourceName.ToLower() + PartBackgroundPostName];
        public Visibility CSOPartNameVisibility => (CSOPartItem ?? ItemManager.EmptyItem).IsEmpty ? Visibility.Collapsed : Visibility.Visible;
        public string CSOPartName => GetName();
        public string CSOPartDescription => GetDescription();

        /// <summary>
        /// 浮窗属性
        /// </summary>
        [ObservableProperty]
        private bool _CSOPartFlyoutIsOpen;
        public int CSOPartFlyoutHeight => GetFlyoutHeight();
        public WriteableBitmap CSOPartFlyoutBorder => Launcher.Assets[CSOFlyout.GetAssets(CSOPartFlyoutWidth, CSOPartFlyoutHeight, Color)];

        private static List<CSOPartData> GetData(ItemPart partType)
        {
            List<CSOPartData> data = [];
            foreach (Item partItem in ItemManager.PartDictionary[partType])
            {
                data.Add(new CSOPartData(partItem));
            }
            return data;
        }

        /// <summary>
        /// 编辑器属性
        /// </summary>

        [ObservableProperty]
        private bool _CSOPartEditorIsOpen;

        private List<CSOPartData> GetPartType()
        {
            ItemPart part;
            if (CSOItem != null)
            {
                if (CSOItem.Infomation != null)
                {
                    part = CSOItem.Infomation.Part;
                }
                else part = ItemPart.Disable;
            }
            else part = ItemPart.Disable;
            if (CSOPartgroupDictionary.TryGetValue(part, out var list))
            {
                return list ?? [];
            }
            else
            {
                return [];
            }
        }

        private string GetName()
        {
            if (CSOPartItem != null)
            {
                if (!CSOPartItem.IsEmpty)
                {
                    return CSOPartItem.TransName ?? CSOPartItem.Name;
                }
                else
                {
                    return ItemManager.EmptyItem.TransName ?? ItemManager.EmptyItem.Name;
                }
            }
            else
            {
                return ItemManager.EmptyItem.TransName ?? ItemManager.EmptyItem.Name;
            }
        }

        private string GetDescription()
        {
            if (CSOPartItem != null)
            {
                if (CSOPartItem.IsEmpty)
                {
                    return ItemManager.LanguageDictionary[EmptyPartDescName];
                }
                else
                {
                    StringBuilder desc = new();
                    desc.Append(ItemManager.LanguageDictionary[PreDescName + CSOPartItem!.ResourceName]);
                    desc.Replace(OldNewLine, NewLine);
                    return desc.ToString();
                }
            }
            else
            {
                return ItemManager.EmptyItem.TransName ?? ItemManager.EmptyItem.Name;
            }
        }

        private int GetFlyoutHeight()
        {
            TextBlock textBlock = new()
            {
                Text = CSOPartDescription,
                Width = CSOPartFlyoutWidth,
                FontSize = 12,
                CharacterSpacing = 50,
                TextWrapping = TextWrapping.WrapWholeWords,
                FontFamily = CSOFont,
            };
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (CSOPartItem != null)
            {
                if (CSOPartItem.IsEmpty)
                {
                    return (int)textBlock.DesiredSize.Height + 27;
                }
                else
                {
                    return (int)textBlock.DesiredSize.Height + 57;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public sealed class CSOPartData
    {
        private static readonly string[] ColorString = ["c", "b", "a", "s", "ss", "sss", "rb"];
        private const string PreName = "itembox_random";
        private const string Extend = ".png";
        private const string ItemDirectory = "Item";
        private const string NonePart = "partsrandom";
        private const char BackSlash = '\\';
        private const char UnderLine = '_';
        private const string CSOItemBoxBorderName = "line";
        private const string CSOItemBoxBottomName = "bg";

        public Item CSOPartItem;
        public string CSOPartName;
        public object CSOPartUri;
        public WriteableBitmap CSOItemBoxBorder;
        public WriteableBitmap CSOItemBoxBottom;

        public CSOPartData(Item item)
        {
            CSOPartItem = item;
            CSOPartName = item.TransName ?? item.Name;
            StringBuilder sb = new();
            sb.Append(PreName);
            sb.Append(UnderLine);
            sb.Append(ColorString[(int)(item.ItemGrade != null ? item.ItemGrade : ItemGrade.None)]);
            sb.Append(UnderLine);
            sb.Append(CSOItemBoxBorderName);
            CSOItemBoxBorder = Launcher.Assets[sb.ToString()];
            sb.Replace(CSOItemBoxBorderName, CSOItemBoxBottomName);
            CSOItemBoxBottom = Launcher.Assets[sb.ToString()];
            if (!item.IsEmpty)
            {
                if (Launcher.ImageResources.TryGetValue(item.ResourceName, out var bitmap))
                {
                    CSOPartUri = bitmap;
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
                    Launcher.ImageResources.Add(item.ResourceName, newbitmap);
                    CSOPartUri = newbitmap;
                }
            }
            else
            {
                CSOPartUri = Launcher.Assets[NonePart];
            }
        }
    }
}
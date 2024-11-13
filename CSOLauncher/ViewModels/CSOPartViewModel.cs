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
        public enum CSOPartSlot : byte
        {
            Part1 = 0,
            Part2 = 1,
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CSOPartItemGroup))]
        private ItemData _CSOItemData;

        [ObservableProperty]
        private CSOPartSlot _Slot;

        public List<CSOPartData> CSOPartItemGroup => GetPartType();

        public WriteableBitmap CSOPartBackground => GetBackground();
        public Visibility CSOPartNameVisibility => GetNameVisibility();
        public string CSOPartName => GetName();
        public string CSOPartDescription => GetDescription();

        /// <summary>
        /// 浮窗属性
        /// </summary>
        [ObservableProperty]
        private bool _CSOPartFlyoutIsOpen;
        public int CSOPartFlyoutHeight => GetFlyoutHeight();
        public WriteableBitmap CSOPartFlyoutBorder => Launcher.Assets[CSOFlyout.GetAssets(CSOPartFlyoutWidth, CSOPartFlyoutHeight, Color)];

        /// <summary>
        /// 编辑器属性
        /// </summary>

        [ObservableProperty]
        private bool _CSOPartEditorIsOpen;

        public CSOPartViewModel(ItemData data, ViewModelPropertyChanged changed, CSOPartSlot slot)
        {
            CSOItemData = data;
            Changed = changed;
            Slot = slot;
            Changed += OnChanged;
            CSOPartFlyoutIsOpen = false;
            CSOPartEditorIsOpen = false;
        }

        [ObservableProperty]
        private ViewModelPropertyChanged _Changed;

        private void OnChanged()
        {
            OnPropertyChanged(nameof(CSOPartBackground));
            OnPropertyChanged(nameof(CSOPartNameVisibility));
            OnPropertyChanged(nameof(CSOPartName));
            OnPropertyChanged(nameof(CSOPartDescription));
            OnPropertyChanged(nameof(CSOPartFlyoutHeight));
            OnPropertyChanged(nameof(CSOPartFlyoutBorder));
        }

        private WriteableBitmap GetBackground()
        {
            if (Slot == CSOPartSlot.Part1)
            {
                if (CSOItemData.Part1.IsEmpty)
                {
                    return Launcher.Assets[EmptySlot];
                }
                else
                {
                    return Launcher.Assets[CSOItemData.Part1.ResourceName.ToLower() + PartBackgroundPostName];
                }
            }
            else
            {
                if (CSOItemData.Part2.IsEmpty)
                {
                    return Launcher.Assets[EmptySlot];
                }
                else
                {
                    return Launcher.Assets[CSOItemData.Part2.ResourceName.ToLower() + PartBackgroundPostName];
                }
            }
        }

        private Visibility GetNameVisibility()
        {
            if (Slot == CSOPartSlot.Part1)
            {
                return CSOItemData.Part1.IsEmpty ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return CSOItemData.Part2.IsEmpty? Visibility.Collapsed: Visibility.Visible;
            }
        }

        private static List<CSOPartData> GetData(ItemPart partType)
        {
            List<CSOPartData> data = [];
            foreach (Item partItem in ItemManager.PartDictionary[partType])
            {
                data.Add(new CSOPartData(partItem));
            }
            return data;
        }

        private List<CSOPartData> GetPartType()
        {
            ItemPart part;
            if (CSOItemData.Item.Infomation != null)
            {
                part = CSOItemData.Item.Infomation.Part;
            }
            else
            {
                part = ItemPart.Disable;
            }
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
            if (Slot == CSOPartSlot.Part1)
            {
                if (CSOItemData.Part1.IsEmpty)
                {
                    return ItemManager.EmptyItem.TransName ?? ItemManager.EmptyItem.Name;
                }
                else
                {
                    return CSOItemData.Part1.TransName ?? CSOItemData.Part1.Name;
                }
            }
            else
            {
                if (CSOItemData.Part2.IsEmpty)
                {
                    return ItemManager.EmptyItem.TransName ?? ItemManager.EmptyItem.Name;
                }
                else
                {
                    return CSOItemData.Part1.TransName ?? CSOItemData.Part1.Name;
                }
            }
        }

        private string GetDescription()
        {
            if (Slot == CSOPartSlot.Part1)
            {
                if (CSOItemData.Part1.IsEmpty)
                {
                    return ItemManager.LanguageDictionary[EmptyPartDescName];
                }
                else
                {
                    StringBuilder desc = new();
                    desc.Append(ItemManager.LanguageDictionary[PreDescName + CSOItemData.Part1.ResourceName]);
                    desc.Replace(OldNewLine, NewLine);
                    return desc.ToString();
                }
            }
            else
            {
                if (CSOItemData.Part2.IsEmpty)
                {
                    return ItemManager.LanguageDictionary[EmptyPartDescName];
                }
                else
                {
                    StringBuilder desc = new();
                    desc.Append(ItemManager.LanguageDictionary[PreDescName + CSOItemData.Part2.ResourceName]);
                    desc.Replace(OldNewLine, NewLine);
                    return desc.ToString();
                }
            }
        }

        private int GetFlyoutHeight()
        {
            Item slot;
            if(Slot == CSOPartSlot.Part1)
            {
                slot = CSOItemData.Part1;
            }
            else
            {
                slot = CSOItemData.Part2;
            }
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


            if (slot.IsEmpty)
            {
                return (int)textBlock.DesiredSize.Height + 27;
            }
            else
            {
                return (int)textBlock.DesiredSize.Height + 57;
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
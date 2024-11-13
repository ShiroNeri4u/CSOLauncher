using CommunityToolkit.Mvvm.ComponentModel;
using CSODataCore;
using CSOLauncher.DrawMethod;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static CSOLauncher.Launcher;
using static CSOLauncher.ViewModels.ReinforceDataView;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSOLauncher.ViewModels
{
    public partial class CSOReinforceViewModel : ObservableObject
    {
        private const string OverDmgName = "CSO_PlayRoom_WeaponPropDlg_MasterEnhance";
        private const string CSO = "CSO_";
        private const string Label = "_Label";
        private const string ReinforceName = "enchant";
        private const string TipMax = "CSO_ToolTip_WpnEnhancebleMaxLv";
        private const string TipRest = "CSO_ToolTip_RestWpnEnhMaster";
        private const string TipRemain = "CSO_PlayRoom_WeaponPropDlg_EnhanceRemain";
        private const string TipEx = "CSO_ToolTip_WpnEnhPropEx";
        private const string Tip = "CSO_ToolTip_WpnEnhPropEx_Desc";
        private const string Unlimited = "CSO_Match_Roomlist_NoLimit";
        private const string ReplaceName = "%d";
        private const string OldNewLine = "\\n";
        private const string NewLine = "\n";
        public static readonly WriteableBitmap CSOReinforceground = Launcher.Assets[ReinforceName];
        public static string ReinforceTip => GetTip();
        private const CSOFlyout.Color Color = CSOFlyout.Color.Grey;
        private static readonly WriteableBitmap _CSOReinforceFlyoutBorder = Launcher.Assets[CSOFlyout.GetAssets(230, 310, Color)];
        public static readonly WriteableBitmap CSOReinforceFlyoutBorder = _CSOReinforceFlyoutBorder;
        private static readonly ReinforceType[] Types = [ReinforceType.Damage, ReinforceType.Accuracy, ReinforceType.Rebound, ReinforceType.Weight, ReinforceType.Repeatedly, ReinforceType.Ammo, ReinforceType.OverDmg];

        /// <summary>
        /// 数据
        /// </summary>
        [ObservableProperty]
        private ItemData _CSOItemData;

        public List<ReinforceDataView> ReinforceDataView = [];
        public string MaxReinforceTip => GetTipMax();
        public string RestReinforceTip => GetTipRest();
        public string RemainReinforceTip => GetTipRemain();
        public string ExReinforceTip => GetTipEx();

        [ObservableProperty]
        private bool _CSOReinforceFlyoutIsOpen;
        [ObservableProperty]
        private bool _CSOReinforceEditorMode;

        public CSOReinforceViewModel(ItemData data , ViewModelPropertyChanged changed)
        {
            CSOItemData = data;
            CSOReinforceFlyoutIsOpen = false;
            CSOReinforceEditorMode = false;
            Changed = changed;
            Changed += OnChanged;
            if (data.ReinforceData != null)
            {
                CreateDataView(data.ReinforceData);
            }
        }

        private void OnChanged()
        {
            OnPropertyChanged(nameof(MaxReinforceTip));
            OnPropertyChanged(nameof(RestReinforceTip));
            OnPropertyChanged(nameof(RemainReinforceTip));
        }

        private string GetTipMax()
        {
            if (CSOItemData.ReinforceData != null && CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null)
            {
                StringBuilder sb = new();
                sb.Append(ItemManager.LanguageDictionary[TipMax]);
                if(!Launcher.AllowReinforceIgnoreMaxLv)
                {
                    sb.Replace(ReplaceName, CSOItemData.Item.Infomation.Reinforce.TotalMaxLv.ToString());
                }
                else sb.Replace(ReplaceName, ItemManager.LanguageDictionary[Unlimited]);
                return sb.ToString();
            }
            else return string.Empty;
        }

        private string GetTipRest()
        {
            if (CSOItemData.ReinforceData != null)
            {
                StringBuilder sb = new();
                sb.Append(ItemManager.LanguageDictionary[TipRest]);
                byte count;
                if ((CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null) && (CSOItemData.Item.Infomation.Reinforce.OverDmg == null || CSOItemData.Item.Infomation.Reinforce.OverDmg == 0))
                {
                    count = 0;
                }
                else
                {
                    if(CSOItemData.ReinforceData.OverDmg == 0)
                    {
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                sb.Replace(ReplaceName, count.ToString());
                return sb.ToString();
            }
            else return string.Empty;
        }

        private string GetTipRemain()
        {
            StringBuilder sb = new();
            sb.Append(ItemManager.LanguageDictionary[TipRemain]);
            int count;
            if(CSOItemData.ReinforceData != null && CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null)
            {
                count = CSOItemData.Item.Infomation.Reinforce.TotalMaxLv - (CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo + CSOItemData.ReinforceData.OverDmg);
            }
            else
            {
                count = 0;
            }
            if (!Launcher.AllowReinforceIgnoreMaxLv)
            {
                sb.Replace(ReplaceName, count.ToString());
            }
            else
            {
                int colonIndex = sb.ToString().IndexOf(':');
                if (colonIndex < 0)
                {
                    colonIndex = sb.ToString().IndexOf('：');
                }
                if (colonIndex != -1)
                {
                    sb.Remove(colonIndex + 1, sb.Length - (colonIndex + 1));
                    sb.Append(ItemManager.LanguageDictionary[Unlimited]);
                }
            }
            return sb.ToString();
        }

        private string GetTipEx()
        {
            StringBuilder sb = new();
            sb.Append(ItemManager.LanguageDictionary[TipEx]);
            int count;
            if (CSOItemData.ReinforceData != null)
            {
                if(CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null)
                {
                    byte? overdmg = CSOItemData.Item.Infomation.Reinforce.OverDmg;
                    if (overdmg != null)
                    {
                        count = (int)overdmg;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            else
            {
                count = 0;
            }
            sb.Replace(ReplaceName + "%", count.ToString());
            return sb.ToString();
        }

        private static string GetTip()
        {
            StringBuilder sb = new();
            sb.Append(ItemManager.LanguageDictionary[Tip]);
            sb.Replace(OldNewLine, NewLine);
            return sb.ToString();
        }

        private void CreateDataView(ReinforceData data)
        {
            if (data != null)
            {
                foreach (ReinforceType type in Types)
                {
                    ReinforceDataView.Add(new(CSOItemData, type, GetTag(type)));
                }
                for (int i = 0; i < ReinforceDataView.Count; i++)
                {
                    Changed += ReinforceDataView[i].OnChanged;
                }
                for (int i = 0; i < ReinforceDataView.Count; i++)
                {
                    ReinforceDataView[i].Changed = Changed;
                }
            }
        }

        private ViewModelPropertyChanged Changed;

        private static string GetTag(ReinforceType dataType)
        {
            StringBuilder sb = new();
            sb.Append(CSO);
            switch (dataType)
            {
                case ReinforceType.Damage:
                    sb.Append(nameof(ReinforceType.Damage));
                    break;
                case ReinforceType.Accuracy:
                    sb.Append(nameof(ReinforceType.Accuracy));
                    break;
                case ReinforceType.Rebound:
                    sb.Append(nameof(ReinforceType.Rebound));
                    break;
                case ReinforceType.Weight:
                    sb.Append(nameof(ReinforceType.Weight));
                    break;
                case ReinforceType.Repeatedly:
                    sb.Append(nameof(ReinforceType.Repeatedly));
                    break;
                case ReinforceType.Ammo:
                    sb.Append(nameof(ReinforceType.Ammo));
                    break;
                case ReinforceType.OverDmg:
                    break;
            }
            if (dataType != ReinforceType.OverDmg)
            {
                sb.Append(Label);
                return ItemManager.LanguageDictionary[sb.ToString()];
            }
            else
            {
                return ItemManager.LanguageDictionary[OverDmgName];
            }
        }
    }

    public partial class ReinforceDataView : ObservableObject
    {
        public enum ReinforceType : byte
        {
            Damage = 0,
            Accuracy = 1,
            Rebound = 2,
            Weight = 3,
            Repeatedly = 4,
            Ammo = 5,
            OverDmg = 6,
        }
        private const char Slash = '/';
        private const string EmptyBoxName = "emptybox";
        private const string FillBoxName = "fillbox";
        private static readonly string[] ButtonLeftName = ["btn_small_left@c", "btn_small_left@n", "btn_small_left@o"];
        private static readonly string[] ButtonRightName = ["btn_small_right@c", "btn_small_right@n", "btn_small_right@o"];
        private static readonly WriteableBitmap EmptyBox = Launcher.Assets[EmptyBoxName];
        private static readonly WriteableBitmap FillBox = Launcher.Assets[FillBoxName];
        private static readonly WriteableBitmap[] _ButtonLeft = [Launcher.Assets[ButtonLeftName[0]], Launcher.Assets[ButtonLeftName[1]], Launcher.Assets[ButtonLeftName[2]]];
        public static  readonly WriteableBitmap[] ButtonLeft = _ButtonLeft;
        private static readonly WriteableBitmap[] _ButtonRight = [Launcher.Assets[ButtonRightName[0]], Launcher.Assets[ButtonRightName[1]], Launcher.Assets[ButtonRightName[2]]];
        public static readonly WriteableBitmap[] ButtonRight = _ButtonRight;

        [ObservableProperty]
        public WriteableBitmap _CurrentButtonLeft;
        [ObservableProperty]
        public WriteableBitmap _CurrentButtonRight;

        public ViewModelPropertyChanged? Changed;

        public ItemData CSOItemData;

        private readonly ReinforceType CurrentType;
        public string TagName;
        public string VauleTag => GetTag();
        public byte MaximumValue;
        
        private bool _Init = false;
        private byte _CurrentValue;
        public byte CurrentValue
        {
            get => _CurrentValue;
            set
            {
                _CurrentValue = value;
                ChangeData();
                GetBox();
                OnPropertyChanged();
                if(!_Init)
                {
                    _Init = true;
                    OnChanged();
                }
                else
                {
                    Changed?.Invoke();
                }
                OnPropertyChanged(nameof(Reinforcebox));
                OnPropertyChanged(nameof(VauleTag));
            }
        }

        public Visibility LeftVisibility => GetLeftVisibility();
        public Visibility RightVisibility => GetRightVisibility();
        public ObservableCollection<WriteableBitmap> Reinforcebox = [];

        private string GetTag()
        {
            StringBuilder sb = new();
            sb.Append(CurrentValue);
            sb.Append(Slash);
            sb.Append(MaximumValue);
            return sb.ToString();
        }
        private void GetBox()
        {
            Reinforcebox.Clear();
            for(int i = 0; i < CurrentValue; i++)
            {
                Reinforcebox.Add(FillBox);
            }
            for(int i = CurrentValue; i < MaximumValue; i++)
            {
                Reinforcebox.Add(EmptyBox);
            }
        }
        private void ChangeData()
        {
            if (CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null && CSOItemData.ReinforceData != null)
            {
                switch (CurrentType)
                {
                    case ReinforceType.Damage:
                        CSOItemData.ReinforceData.Damage = CurrentValue;
                        break;
                    case ReinforceType.Accuracy:
                        CSOItemData.ReinforceData.Accuracy = CurrentValue;
                        break;
                    case ReinforceType.Rebound:
                        CSOItemData.ReinforceData.Rebound = CurrentValue;
                        break;
                    case ReinforceType.Weight:
                        CSOItemData.ReinforceData.Weight = CurrentValue;
                        break;
                    case ReinforceType.Repeatedly:
                        CSOItemData.ReinforceData.Repeatedly = CurrentValue;
                        break;
                    case ReinforceType.Ammo:
                        CSOItemData.ReinforceData.Ammo = CurrentValue;
                        break;
                    case ReinforceType.OverDmg:
                        CSOItemData.ReinforceData.OverDmg = CurrentValue;
                        break;
                }
            }
        }

        public void OnChanged()
        {
            OnPropertyChanged(nameof(LeftVisibility));
            OnPropertyChanged(nameof(RightVisibility));
        }

        public Visibility GetLeftVisibility()
        {
            if (CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null && CSOItemData.ReinforceData != null)
            {
                if (CSOItemData.Item.Infomation.Reinforce.OverDmg == 0 || CSOItemData.Item.Infomation.Reinforce.OverDmg == null)
                {
                    if (MaximumValue == 0)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        if (CurrentValue == 0)
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            return Visibility.Visible;
                        }
                    }
                }
                else
                {
                    if (CurrentType == ReinforceType.OverDmg)
                    {
                        if (CurrentValue == 0)
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            return Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (CSOItemData.ReinforceData.OverDmg == 0)
                        {
                            if (CurrentValue == 0)
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                }
            }
            else return Visibility.Collapsed;
        }

        public Visibility GetRightVisibility()
        {
            if (CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null && CSOItemData.ReinforceData != null)
            {
                if (CSOItemData.Item.Infomation.Reinforce.OverDmg == 0 || CSOItemData.Item.Infomation.Reinforce.OverDmg == null)
                {
                    if (MaximumValue == 0)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        if ((CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo + CSOItemData.ReinforceData.OverDmg) >= CSOItemData.Item.Infomation.Reinforce.TotalMaxLv && !Launcher.AllowReinforceIgnoreMaxLv)
                        {
                            return Visibility.Collapsed;
                        }
                        else if (CurrentValue < MaximumValue)
                        {
                            return Visibility.Visible;
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    if (CurrentType == ReinforceType.OverDmg)
                    {
                        if ((CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo) + 1 == CSOItemData.Item.Infomation.Reinforce.TotalMaxLv)
                        {
                            if (CSOItemData.ReinforceData.OverDmg == 1)
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                        else
                        {
                            return Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (CurrentValue >= MaximumValue)
                        {
                            return Visibility.Collapsed;
                        }
                        else
                        {
                            if ((CSOItemData.ReinforceData.Damage + CSOItemData.ReinforceData.Accuracy + CSOItemData.ReinforceData.Rebound + CSOItemData.ReinforceData.Weight + CSOItemData.ReinforceData.Repeatedly + CSOItemData.ReinforceData.Ammo) + 1 >= CSOItemData.Item.Infomation.Reinforce.TotalMaxLv && !Launcher.AllowReinforceIgnoreMaxLv)
                            {
                                return Visibility.Collapsed;
                            }
                            else
                            {
                                return Visibility.Visible;
                            }
                        }
                    }
                }
            }
            else return Visibility.Collapsed;
        }

        public ReinforceDataView(ItemData data, ReinforceType type, string tag)
        {
            CSOItemData = data;
            CurrentType = type;
            if(CSOItemData.Item.Infomation != null && CSOItemData.Item.Infomation.Reinforce != null && CSOItemData.ReinforceData != null)
            {
                switch (CurrentType)
                {
                    case ReinforceType.Damage:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Damage;
                        CurrentValue = CSOItemData.ReinforceData.Damage;
                        break;
                    case ReinforceType.Accuracy:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Accuracy;
                        CurrentValue = CSOItemData.ReinforceData.Accuracy;
                        break;
                    case ReinforceType.Rebound:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Rebound;
                        CurrentValue = CSOItemData.ReinforceData.Rebound;
                        break;
                    case ReinforceType.Weight:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Weight;
                        CurrentValue = CSOItemData.ReinforceData.Weight;
                        break;
                    case ReinforceType.Repeatedly:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Repeatedly;
                        CurrentValue = CSOItemData.ReinforceData.Repeatedly;
                        break;
                    case ReinforceType.Ammo:
                        MaximumValue = CSOItemData.Item.Infomation.Reinforce.Ammo;
                        CurrentValue = CSOItemData.ReinforceData.Ammo;
                        break;
                    case ReinforceType.OverDmg:
                        MaximumValue = (byte)((CSOItemData.Item.Infomation.Reinforce.OverDmg == null || CSOItemData.Item.Infomation.Reinforce.OverDmg == 0) ? 0 : 1);
                        CurrentValue = CSOItemData.ReinforceData.OverDmg;
                        break;
                }
            }
            TagName = tag;
            CurrentButtonLeft = ButtonLeft[1];
            CurrentButtonRight = ButtonRight[1];
        }
    }
}

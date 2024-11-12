using CommunityToolkit.Mvvm.ComponentModel;
using CSODataCore;
using CSOLauncher.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static CSOLauncher.ViewModels.ReinforceDataView;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using CSOLauncher.DrawMethod;
using Windows.Security.Authentication.Web.Provider;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Store;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using static CSOLauncher.CSOButtonBase;
using Microsoft.UI.Input;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using static CSOLauncher.ViewModels.CSOReinforceViewModel;

namespace CSOLauncher.ViewModels
{
    public partial class CSOReinforceViewModel : ObservableObject
    {
        private const string OverDmgName = "CSO_PlayRoom_WeaponPropDlg_MasterEnhance";
        private const string CSO = "CSO_";
        private const string Label = "_Label";
        private const string Reinforce = "enchant";
        private const string TipMax = "CSO_ToolTip_WpnEnhancebleMaxLv";
        private const string TipRest = "CSO_ToolTip_RestWpnEnhMaster";
        private const string TipRemain = "CSO_PlayRoom_WeaponPropDlg_EnhanceRemain";
        private const string TipEx = "CSO_ToolTip_WpnEnhPropEx";
        private const string Tip = "CSO_ToolTip_WpnEnhPropEx_Desc";
        public static string ReinforceTip => GetTip();
        private const string ReplaceName = "%d";
        private const string OldNewLine = "\\n";
        private const string NewLine = "\n";
        public static readonly WriteableBitmap CSOReinforceground = Launcher.Assets[Reinforce];
        private static readonly FontFamily CSOFont = (FontFamily)Microsoft.UI.Xaml.Application.Current.Resources["CSOFont"];
        private const CSOFlyout.Color Color = CSOFlyout.Color.Grey;
        public static WriteableBitmap CSOReinforceFlyoutBorder = Launcher.Assets[CSOFlyout.GetAssets(230, 310, Color)];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RestReinforceTip))]
        [NotifyPropertyChangedFor(nameof(RemainReinforceTip))]
        private ReinforceData? _ReinforceData;
        public string MaxReinforceTip => GetTipMax();
        public string RestReinforceTip => GetTipRest();
        public string RemainReinforceTip => GetTipRemain();
        public string ExReinforceTip => GetTipEx();

        public List<ReinforceDataView> ReinforceDataView = [];
        [ObservableProperty]
        private bool _CSOReinforceFlyoutIsOpen;
        [ObservableProperty]
        private bool _IsEditorMode;


        public CSOReinforceViewModel(ReinforceData data)
        {
            ReinforceData = data;
            Changed = new OnChangedChild(ChangeText);
            CreateDataView(data);
        }

        private string GetTipMax ()
        {
            if (ReinforceData != null)
            {
                StringBuilder sb = new();
                sb.Append(ItemManager.LanguageDictionary[TipMax]);
                sb.Replace(ReplaceName, ReinforceData.Reinforce.TotalMaxLv.ToString());
                return sb.ToString();
            }
            else return string.Empty;
        }

        private void ChangeText()
        {
            OnPropertyChanged(nameof(RestReinforceTip));
            OnPropertyChanged(nameof(RemainReinforceTip));
        }

        private string GetTipRest()
        {
            if (ReinforceData != null)
            {
                StringBuilder sb = new();
                sb.Append(ItemManager.LanguageDictionary[TipRest]);
                byte count;
                if (ReinforceData.Reinforce.OverDmg == null || ReinforceData.Reinforce.OverDmg == 0)
                {
                    count = 0;
                }
                else
                {
                    if(ReinforceData.OverDmg == 0)
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
            if(ReinforceData != null)
            {
                count = ReinforceData.Reinforce.TotalMaxLv - (ReinforceData.Damage + ReinforceData.Accuracy + ReinforceData.Rebound + ReinforceData.Weight + ReinforceData.Repeatedly + ReinforceData.Ammo + ReinforceData.OverDmg);
            }
            else
            {
                count = 0;
            }
            sb.Replace(ReplaceName, count.ToString());
            return sb.ToString();
        }

        private string GetTipEx()
        {
            StringBuilder sb = new();
            sb.Append(ItemManager.LanguageDictionary[TipEx]);
            int count;
            if (ReinforceData != null)
            {
                if(ReinforceData.Reinforce.OverDmg != null)
                {
                    count = (int)ReinforceData.Reinforce.OverDmg;
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
            string damage = GetTag(ReinforceType.Damage);
            ReinforceDataView.Add(new(data, ReinforceType.Damage, damage));
            string accuracy = GetTag(ReinforceType.Accuracy);
            ReinforceDataView.Add(new(data, ReinforceType.Accuracy, accuracy));
            string rebound = GetTag(ReinforceType.Rebound);
            ReinforceDataView.Add(new(data, ReinforceType.Rebound, rebound));
            string Weight = GetTag(ReinforceType.Weight);
            ReinforceDataView.Add(new(data, ReinforceType.Weight, Weight));
            string repeatedly = GetTag(ReinforceType.Repeatedly);
            ReinforceDataView.Add(new(data, ReinforceType.Repeatedly, repeatedly));
            string ammo = GetTag(ReinforceType.Ammo);
            ReinforceDataView.Add(new(data, ReinforceType.Ammo, ammo));
            string overdmg = GetTag(ReinforceType.OverDmg);
            ReinforceDataView.Add(new(data, ReinforceType.OverDmg, overdmg));
            for(int i = 0; i < ReinforceDataView.Count; i++)
            {
                Changed += ReinforceDataView[i].OnChangedOther;
            }
            for (int i = 0; i < ReinforceDataView.Count; i++)
            {
                ReinforceDataView[i].Changed = Changed;
            }
        }

        public delegate void OnChangedChild();

        public OnChangedChild Changed;

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
        private static string[] ButtonLeftName = ["btn_small_left@c", "btn_small_left@n", "btn_small_left@o"];
        private static string[] ButtonRightName = ["btn_small_right@c", "btn_small_right@n", "btn_small_right@o"];
        private static WriteableBitmap EmptyBox = Launcher.Assets[EmptyBoxName];
        private static WriteableBitmap FillBox = Launcher.Assets[FillBoxName];
        public static WriteableBitmap[] ButtonLeft = [Launcher.Assets[ButtonLeftName[0]], Launcher.Assets[ButtonLeftName[1]], Launcher.Assets[ButtonLeftName[2]]];
        public static WriteableBitmap[] ButtonRight = [Launcher.Assets[ButtonRightName[0]], Launcher.Assets[ButtonRightName[1]], Launcher.Assets[ButtonRightName[2]]];
        [ObservableProperty]
        public WriteableBitmap _CurrentButtonLeft;
        [ObservableProperty]
        public WriteableBitmap _CurrentButtonRight;

        public OnChangedChild? Changed;

        public ReinforceData Data;
        private ReinforceType CurrentType;
        public string TagName;
        public string VauleTag => GetTag();
        public byte MaximumValue;
        

        private bool init = false;
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
                if(!init)
                {
                    init = true;
                    OnChangedOther();
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
            sb.Append(CurrentValue.ToString());
            sb.Append(Slash);
            sb.Append(MaximumValue.ToString());
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
            switch (CurrentType)
            {
                case ReinforceType.Damage:
                    Data.Damage = CurrentValue;
                    break;
                case ReinforceType.Accuracy:
                    Data.Accuracy = CurrentValue;
                    break;
                case ReinforceType.Rebound:
                    Data.Rebound = CurrentValue;
                    break;
                case ReinforceType.Weight:
                    Data.Weight = CurrentValue;
                    break;
                case ReinforceType.Repeatedly:
                    Data.Repeatedly = CurrentValue;
                    break;
                case ReinforceType.Ammo:
                    Data.Ammo = CurrentValue;
                    break;
                case ReinforceType.OverDmg:
                    Data.OverDmg = CurrentValue;
                    break;
            }
        }

        public void OnChangedOther()
        {
            OnPropertyChanged(nameof(LeftVisibility));
            OnPropertyChanged(nameof(RightVisibility));
        }

        public Visibility GetLeftVisibility()
        {
            if(Data.Reinforce.OverDmg == 0 || Data.Reinforce.OverDmg == null)
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
                if(CurrentType == ReinforceType.OverDmg)
                {
                    if(CurrentValue == 0)
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
                    if(Data.OverDmg == 0)
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

        public Visibility GetRightVisibility()
        {
            if (Data.Reinforce.OverDmg == 0 || Data.Reinforce.OverDmg == null)
            {
                if (MaximumValue == 0)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    if ((Data.Damage + Data.Accuracy + Data.Rebound + Data.Weight + Data.Repeatedly + Data.Ammo + Data.OverDmg) >= Data.Reinforce.TotalMaxLv)
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
                    if ((Data.Damage + Data.Accuracy + Data.Rebound + Data.Weight + Data.Repeatedly + Data.Ammo) + 1 == Data.Reinforce.TotalMaxLv)
                    {
                        if(Data.OverDmg == 1)
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
                        if ((Data.Damage + Data.Accuracy + Data.Rebound + Data.Weight + Data.Repeatedly + Data.Ammo) + 1 >= Data.Reinforce.TotalMaxLv)
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

        public ReinforceDataView(ReinforceData data, ReinforceType type, string tag)
        {
            Data = data;
            CurrentType = type;
            switch (CurrentType)
            {
                case ReinforceType.Damage:
                    MaximumValue = Data.Reinforce.Damage;
                    CurrentValue = Data.Damage;
                    break;
                case ReinforceType.Accuracy:
                    MaximumValue = Data.Reinforce.Accuracy;
                    CurrentValue = Data.Accuracy;
                    break;
                case ReinforceType.Rebound:
                    MaximumValue = Data.Reinforce.Rebound;
                    CurrentValue = Data.Rebound;
                    break;
                case ReinforceType.Weight:
                    MaximumValue = Data.Reinforce.Weight;
                    CurrentValue = Data.Weight;
                    break;
                case ReinforceType.Repeatedly:
                    MaximumValue = Data.Reinforce.Repeatedly;
                    CurrentValue = Data.Repeatedly;
                    break;
                case ReinforceType.Ammo:
                    MaximumValue = Data.Reinforce.Ammo;
                    CurrentValue = Data.Ammo;
                    break;
                case ReinforceType.OverDmg:
                    MaximumValue = (byte)((Data.Reinforce.OverDmg == null) ? 0 : 1);
                    CurrentValue = Data.OverDmg;
                    break;
            }
            TagName = tag;
            CurrentButtonLeft = ButtonLeft[1];
            CurrentButtonRight = ButtonRight[1];
        }
    }
}

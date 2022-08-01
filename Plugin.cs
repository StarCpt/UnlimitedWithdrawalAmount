using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Plugins;
using HarmonyLib;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Controls;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Unlimited_ATM_Withdrawal_Amount
{
    public class Plugin : IPlugin
    {
        public void Dispose()
        {

        }

        public void Init(object gameInstance)
        {
            new Harmony("UnlimitedWithdrawlAmount").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Update()
        {

        }
    }

    [HarmonyPatch(typeof(AtmBlockView), "InitializeComponent")]
    public class PatchAtmWithdrawalLimit
    {
        public static void Postfix(NumericTextBox ___e_25)
        {
            ___e_25.MaxLength = int.MaxValue;
            ___e_25.Maximum = float.MaxValue;
        }
    }

    [HarmonyPatch(typeof(StoreBlockView), "Get_e_6_Items")]
    public class PatchStoreWithdrawalLimit
    {
        public static void Postfix(ref ObservableCollection<object> __result)
        {   //                                                                                         grid1        grid2        grid3        grid5           numericTextBox1
            NumericTextBox box = (NumericTextBox)((Grid)((Grid)((Grid)((Grid)((TabItem)__result[0]).Content).Children[1]).Children[0]).Children[4]).Children[4];
            box.MaxLength = int.MaxValue;
            box.Maximum = float.MaxValue;
        }
    }
}

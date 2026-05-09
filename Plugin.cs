using System.Collections.Generic;
using VRage.Plugins;
using HarmonyLib;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Controls;
using System.Reflection;
using System.Collections.ObjectModel;
using EmptyKeys.UserInterface;
using System;
using EmptyKeys.UserInterface.Data;
using System.Linq;

namespace UnlimitedWithdrawalAmount;

public class Plugin : IPlugin
{
    public void Init(object gameInstance)
    {
        new Harmony("UnlimitedWithdrawlAmount").PatchAll(Assembly.GetExecutingAssembly());
    }

    public void Update()
    {
    }

    public void Dispose()
    {
    }
}

[HarmonyPatch(typeof(AtmBlockView), nameof(AtmBlockView.InitializeComponent))]
public class PatchAtmWithdrawalLimit
{
    [HarmonyPostfix]
    public static void Postfix(AtmBlockView __instance)
    {
        __instance.e_25.MaxLength = 10;
        __instance.e_25.Maximum = 2147483000f;
    }
}

[HarmonyPatch(typeof(StoreBlockView), nameof(StoreBlockView.Get_e_6_Items))]
public class PatchStoreWithdrawalLimit
{
    [HarmonyPostfix]
    public static void Postfix(ObservableCollection<object> __result)
    {
        NumericTextBox numericTextBox = (NumericTextBox)((TabItem)__result[0]).FindElement(static i => i.Name == "e_64" && (BindingOperations.GetAllBindings(i)?.Any(static i => i?.ParentBinding?.PropertyName == "BalanceChangeValue") ?? false))!;
        numericTextBox.MaxLength = 10;
        numericTextBox.Maximum = 2147483000f;
    }
}

public static class Extensions
{
    public static UIElement? FindElement(this UIElement rootElement, Func<UIElement, bool> predicate)
    {
        Stack<UIElement> stack = [];
        stack.Push(rootElement);
        while (stack.Count > 0)
        {
            var element = stack.Pop();
            if (predicate.Invoke(element))
                return element;
            if (element is ContentControl contentControlElement && contentControlElement.Content is UIElement contentElement)
            {
                stack.Push(contentElement);
            }
            else if (element is Panel panelElement)
            {
                for (int i = panelElement.Children.Count - 1; i >= 0; i--)
                {
                    var child = panelElement.Children[i];
                    if (child is not null)
                    {
                        stack.Push(child);
                    }
                }
            }
        }
        return null;
    }
}

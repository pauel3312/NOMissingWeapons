using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;

namespace NOMW.Patches;

[HarmonyPatch]
internal static class WeaponAmmoPatch
{
    [HarmonyPrefix]
    [UsedImplicitly]
    internal static bool AmmoGonePrefix(Weapon __instance, ref int __result)
    {
        if (!(__instance.hardpoint?.part?.IsDetached() ?? false)) return true;
        __result = 0;
        return false;
    }
    
    [UsedImplicitly]
    private static IEnumerable<MethodBase> TargetMethods()
    {
        var weaponTypes = typeof(Weapon).Assembly.GetTypes()
            .Where(t => typeof(Weapon).IsAssignableFrom(t));

        foreach (var type in weaponTypes)
        {
            var ammoTotal = AccessTools.DeclaredMethod(type, nameof(Weapon.GetAmmoTotal));
            if (ammoTotal != null)
                yield return ammoTotal;

            var ammoLoaded = AccessTools.DeclaredMethod(type, nameof(Weapon.GetAmmoLoaded));
            if (ammoLoaded != null)
                yield return ammoLoaded;
        }
    }
}
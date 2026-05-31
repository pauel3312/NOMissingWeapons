using HarmonyLib;

namespace NOMW.Patches;

[HarmonyPatch(typeof(WeaponStation))]
internal class WeaponStationPatch
{
    [HarmonyPatch(nameof(WeaponStation.LaunchMount))]
    [HarmonyPrefix]
    internal static void LaunchMountPrefix(WeaponStation __instance, Unit owner, Unit target, GlobalPosition aimpoint)
    {
        while (__instance.weaponIndex < __instance.Weapons.Count &&
               __instance.Weapons[__instance.weaponIndex].hardpoint.part.IsDetached())
        {
            __instance.weaponIndex++;
        }
    }
}
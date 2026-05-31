using HarmonyLib;

namespace NOMW.Patches;

[HarmonyPatch]
internal static class HardpointSpawnPatch
{
    [HarmonyPatch(typeof(Hardpoint), nameof(Hardpoint.SpawnMount))]
    [HarmonyPostfix]
    internal static void SpawnMountPostfix(this Hardpoint __instance, Aircraft aircraft, WeaponMount weaponMount)
    {
        __instance.part.onParentDetached += part =>
        {
            if (aircraft == null || !(aircraft.Player?.IsLocalPlayer ?? false)) return;
            var weapons = part.GetComponentsInChildren<Weapon>();
            NOMW.Logger.LogDebug($"Detached {part.name}; {weapons.Length}");
            foreach (var weapon in weapons)
            {
                weapon.weaponStation.AccountAmmo();
                weapon.weaponStation.Updated();
            }
        };
    }
}
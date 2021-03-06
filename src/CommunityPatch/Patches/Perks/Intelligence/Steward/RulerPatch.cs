using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using static System.Reflection.BindingFlags;
using static CommunityPatch.HarmonyHelpers;

namespace CommunityPatch.Patches.Perks.Intelligence.Steward {

  public sealed class RulerPatch : PerkPatchBase<RulerPatch> {

    public override bool Applied { get; protected set; }

    private static readonly MethodInfo TargetMethodInfo = typeof(Clan).GetMethod("get_CompanionLimit", Public | Instance | DeclaredOnly);

    private static readonly MethodInfo PatchMethodInfo = typeof(RulerPatch).GetMethod(nameof(CompanionLimitPatched), NonPublic | Static | DeclaredOnly);

    public RulerPatch() : base("IcgVKFxZ") {
    }

    public override IEnumerable<MethodBase> GetMethodsChecked() {
      yield return TargetMethodInfo;
    }

    public static readonly byte[][] Hashes = {
      new byte[] {
        // e1.1.0.224785
        0x18, 0xDB, 0x6B, 0x5B, 0xF9, 0x74, 0xDC, 0xA3,
        0xF4, 0x82, 0x7E, 0x6A, 0x21, 0xEF, 0x15, 0x44,
        0x62, 0x28, 0x42, 0xB4, 0xB1, 0x9C, 0xD5, 0x99,
        0xE6, 0xC7, 0xF3, 0x16, 0x71, 0xC3, 0xF6, 0x22
      }
    };

    public override void Apply(Game game) {
      if (Applied) return;

      CommunityPatchSubModule.Harmony.Patch(TargetMethodInfo,
        null,
        new HarmonyMethod(PatchMethodInfo));
      Applied = true;
    }

    public override bool? IsApplicable(Game game) {
      var patchInfo = Harmony.GetPatchInfo(TargetMethodInfo);
      if (AlreadyPatchedByOthers(patchInfo))
        return false;

      var hash = TargetMethodInfo.MakeCilSignatureSha256();
      return hash.MatchesAnySha256(Hashes);
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static void CompanionLimitPatched(Clan __instance, ref int __result) {
      if (__instance.Leader.GetPerkValue(ActivePatch.Perk))
        __result += Town.All.Count(t => t.Owner.Owner == __instance.Leader);
    }

  }

}
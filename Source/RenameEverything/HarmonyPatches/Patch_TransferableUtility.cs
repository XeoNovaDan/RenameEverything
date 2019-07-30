using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace RenameEverything
{

    public static class Patch_TransferableUtility   
    {

        [HarmonyPatch(typeof(TransferableUtility))]
        [HarmonyPatch(nameof(TransferableUtility.TransferAsOne))]
        public static class Patch_TransferAsOne
        {

            public static void Postfix(Thing a, Thing b, ref bool __result)
            {
                // If Thing a and Thing b both have CompRenamable and they either have different names or different label colours, count them as separate items for tradeable UIs
                var renamableCompA = a.TryGetComp<CompRenamable>();
                var renamableCompB = b.TryGetComp<CompRenamable>();
                if (renamableCompA != null && !renamableCompA.Equals(renamableCompB))
                {
                    __result = false;
                }
            }

        }

    }

}

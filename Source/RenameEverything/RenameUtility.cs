using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public static class RenameUtility
    {

        public static bool IsClassOrSubclassOf(this Type type, Type other) => type == other || type.IsSubclassOf(other);

        public static IEnumerable<CompRenamable> GetRenamableEquipmentComps(Pawn pawn)
        {
            // Equipment
            if (pawn.equipment != null)
                foreach (var eq in pawn.equipment.AllEquipmentListForReading)
                    if (eq.GetComp<CompRenamable>() is CompRenamable renamableComp)
                        yield return renamableComp;

            // Apparel
            if (pawn.apparel != null)
                foreach (var ap in pawn.apparel.WornApparel)
                    if (ap.GetComp<CompRenamable>() is CompRenamable renamableComp)
                        yield return renamableComp;
        }

    }

}

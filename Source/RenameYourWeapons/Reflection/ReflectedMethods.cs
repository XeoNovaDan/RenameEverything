using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    [StaticConstructorOnStartup]
    public static class ReflectedMethods
    {

        static ReflectedMethods()
        {
            // Convert DualWield.Ext_Pawn_EquipmentTracker.TryGetOffHandEquipment to a delegate
            if (ModCompatibilityCheck.DualWield)
            {
                var extPawnEquipmentTrackerType = GenTypes.GetTypeInAnyAssemblyNew("DualWield.Ext_Pawn_EquipmentTracker", null);
                TryGetOffHandEquipmentInfo = extPawnEquipmentTrackerType.GetMethod("TryGetOffHandEquipment", BindingFlags.Public | BindingFlags.Static);
            }
        }

        public static MethodInfo TryGetOffHandEquipmentInfo;

    }

}

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
                var tryGetOffHandEquipmentInfo = extPawnEquipmentTrackerType.GetMethod("TryGetOffHandEquipment", BindingFlags.Public | BindingFlags.Static);
                TryGetOffHandEquipment = (FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>)Delegate.CreateDelegate(typeof(FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>), tryGetOffHandEquipmentInfo);
            }
        }

        public static FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool> TryGetOffHandEquipment;

        public delegate V FuncOut<T, U, V>(T input, out U output);

    }

}

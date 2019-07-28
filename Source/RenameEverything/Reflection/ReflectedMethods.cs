using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace RenameEverything
{

    [StaticConstructorOnStartup]
    public static class ReflectedMethods
    {

        static ReflectedMethods()
        {
            // Convert DualWield.Ext_Pawn_EquipmentTracker.TryGetOffHandEquipment to a delegate
            if (ModCompatibilityCheck.DualWield)
            {
                TryGetOffHandEquipment = (FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>)Delegate.CreateDelegate(typeof(FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool>),
                    AccessTools.Method(GenTypes.GetTypeInAnyAssemblyNew("DualWield.Ext_Pawn_EquipmentTracker", "DualWield"), "TryGetOffHandEquipment"));
            }
        }

        public static FuncOut<Pawn_EquipmentTracker, ThingWithComps, bool> TryGetOffHandEquipment;

        public delegate V FuncOut<T, U, V>(T input, out U output);

    }

}

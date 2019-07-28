using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public static class ModCompatibilityCheck
    {

        public static bool DualWield => ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Dual Wield");

    }

}

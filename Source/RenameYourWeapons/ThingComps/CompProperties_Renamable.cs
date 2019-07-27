using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class CompProperties_Renamable : CompProperties
    {

        public CompProperties_Renamable()
        {
            compClass = typeof(CompRenamable);
        }

        public string renameTranslationKey;
        public string inspectStringTranlationKey;

    }

}

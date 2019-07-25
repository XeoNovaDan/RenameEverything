using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameYourWeapons
{

    public class CompRenamable : ThingComp
    {

        private string cachedLabel;
        public string name;

        public bool Named
        {
            get => !name.NullOrEmpty() && name.ToLower() != cachedLabel.ToLower();
            set
            {
                if (!value)
                    name = String.Empty;
            }
        }

        public override string TransformLabel(string label)
        {
            cachedLabel = label;
            if (Named)
            {
                bool shouldAppendCachedLabel = RenameYourWeaponsSettings.appendCachedLabel && (!RenameYourWeaponsSettings.onlyAppendInThingHolder || ThingOwnerUtility.GetFirstSpawnedParentThing(parent) != parent);
                return name + (shouldAppendCachedLabel ? $" ({cachedLabel.CapitalizeFirst()})" : String.Empty);
            }
            return label;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            return RenameWeaponsUtility.RenameWeaponGizmos(this);
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref cachedLabel, "cachedLabel");
            Scribe_Values.Look(ref name, "name");
            base.PostExposeData();
        }

        public override string CompInspectStringExtra()
        {
            return Named ? $"{"ShootReportWeapon".Translate()}: {cachedLabel.CapitalizeFirst()}" : null;
        }

    }

}

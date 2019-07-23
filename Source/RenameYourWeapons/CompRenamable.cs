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

        public bool Named => !name.NullOrEmpty();

        public override string TransformLabel(string label)
        {
            cachedLabel = label;
            return Named ? name : label;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_RenameWeapon()
            {
                renamable = this,
                defaultLabel = "Rename".Translate(),
                icon = TexButton.RenameTex,
            };
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

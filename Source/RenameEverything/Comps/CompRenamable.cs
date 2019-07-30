using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class CompRenamable : ThingComp
    {

        private string cachedLabel;
        public string name;

        private float labelColourR = 1;
        private float labelColourG = 1;
        private float labelColourB = 1;
        private float labelColourA = 1;

        [Unsaved]
        private Color cachedColour;

        public CompProperties_Renamable Props => (CompProperties_Renamable)props;

        public bool Named
        {
            get => !name.NullOrEmpty() && name.ToLower() != cachedLabel.ToLower();
            set
            {
                if (!value)
                    name = String.Empty;
            }
        }

        public Color Colour
        {
            get
            {
                if (cachedColour == default)
                    cachedColour = new Color(labelColourR, labelColourG, labelColourB, labelColourA);
                return cachedColour;
            }
            set
            {
                labelColourR = value.r;
                labelColourG = value.g;
                labelColourB = value.b;
                labelColourA = value.a;
                cachedColour = default;
            }
        }

        public override string TransformLabel(string label)
        {
            cachedLabel = label;
            if (Named)
            {
                bool shouldAppendCachedLabel = RenameEverythingSettings.appendCachedLabel && (!RenameEverythingSettings.onlyAppendInThingHolder || ThingOwnerUtility.GetFirstSpawnedParentThing(parent) != parent);
                return name + (shouldAppendCachedLabel ? $" ({cachedLabel.CapitalizeFirst()})" : String.Empty);
            }
            return label;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            string filler = Props.inspectStringTranslationKey.Translate().UncapitalizeFirst();
            yield return new Command_Rename()
            {
                renamable = this,
                defaultLabel = Props.renameTranslationKey.Translate(),
                defaultDesc = "RenameEverything.RenameGizmo_Description".Translate(filler),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1,
            };

            yield return new Command_RecolourLabel()
            {
                renamable = this,
                defaultLabel = "RenameEverything.RecolourLabel".Translate(),
                defaultDesc = "RenameEverything.RecolourLabel_Description".Translate(filler),
                icon = TexButton.RenameTex
            };

            if (Named)
            {
                yield return new Command_Action()
                {
                    defaultLabel = "RenameEverything.RemoveName".Translate(),
                    defaultDesc = "RenameEverything.RemoveName_Description".Translate(filler),
                    icon = TexButton.DeleteX,
                    action = () => Named = false,
                };
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref cachedLabel, "cachedLabel");
            Scribe_Values.Look(ref name, "name");

            Scribe_Values.Look(ref labelColourR, "labelColourR", 1);
            Scribe_Values.Look(ref labelColourG, "labelColourG", 1);
            Scribe_Values.Look(ref labelColourB, "labelColourB", 1);
            Scribe_Values.Look(ref labelColourA, "labelColourA", 1);

            base.PostExposeData();
        }

        public override string CompInspectStringExtra()
        {
            return Named ? $"{Props.inspectStringTranslationKey.Translate()}: {cachedLabel.CapitalizeFirst()}" : null;
        }

    }

}

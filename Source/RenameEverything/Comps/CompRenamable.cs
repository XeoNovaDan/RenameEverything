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

        private string cachedLabel = String.Empty;
        private string _name = String.Empty;
        public Color labelColour = Color.white;
        private bool allowMerge;

        public CompProperties_Renamable Props => (CompProperties_Renamable)props;

        public bool Named
        {
            get => !_name.NullOrEmpty() && _name.ToLower() != cachedLabel.ToLower();
            set
            {
                if (!value)
                    _name = String.Empty;
            }
        }

        public string Name
        {
            get => Named ? _name : String.Empty;
            set => _name = value;
        }

        public bool Coloured
        {
            get => !labelColour.IndistinguishableFrom(Color.white);
            set
            {
                if (!value)
                    labelColour = Color.white;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CompRenamable otherRenamable)
                return Name.Equals(otherRenamable.Name) && labelColour.IndistinguishableFrom(otherRenamable.labelColour);
            return false;
        }

        public override int GetHashCode() => Name.GetHashCode();

        public override bool AllowStackWith(Thing other)
        {
            if (!base.AllowStackWith(other))
                return false;
            return allowMerge || Equals(other.TryGetComp<CompRenamable>());
        }

        public override string TransformLabel(string label)
        {
            cachedLabel = label;
            if (Named)
            {
                bool shouldAppendCachedLabel = RenameEverythingSettings.appendCachedLabel && (!RenameEverythingSettings.onlyAppendInThingHolder || ThingOwnerUtility.GetFirstSpawnedParentThing(parent) != parent);
                return Name + (shouldAppendCachedLabel ? $" ({cachedLabel.CapitalizeFirst()})" : String.Empty);
            }
            return label;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            string filler = Props.inspectStringTranslationKey.Translate().UncapitalizeFirst();
            // Rename
            yield return new Command_Rename()
            {
                renamable = this,
                defaultLabel = Props.renameTranslationKey.Translate(),
                defaultDesc = "RenameEverything.RenameGizmo_Description".Translate(filler),
                icon = TexButton.RenameTex,
                hotKey = KeyBindingDefOf.Misc1,
            };
            
            // Recolour label
            yield return new Command_RecolourLabel()
            {
                renamable = this,
                defaultLabel = "RenameEverything.RecolourLabel".Translate(),
                defaultDesc = "RenameEverything.RecolourLabel_Description".Translate(filler),
                icon = TexButton.RecolourTex
            };

            if (Named || Coloured)
            {
                // Allow merging
                if (parent.def.stackLimit > 1)
                    yield return new Command_Toggle()
                    {
                        defaultLabel = "RenameEverything.AllowMerging".Translate(),
                        defaultDesc = "RenameEverything.AllowMerging_Description".Translate(),
                        icon = TexButton.AllowMergingTex,
                        isActive = () => allowMerge,
                        toggleAction = () => allowMerge = !allowMerge
                    };

                // Remove name
                if (Named)
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
            Scribe_Values.Look(ref cachedLabel, "cachedLabel", String.Empty);
            Scribe_Values.Look(ref _name, "name", String.Empty);
            Scribe_Values.Look(ref labelColour, "labelColour", Color.white);
            Scribe_Values.Look(ref allowMerge, "allowMerge");

            base.PostExposeData();
        }

        public override string CompInspectStringExtra()
        {
            return Named ? $"{Props.inspectStringTranslationKey.Translate()}: {cachedLabel.CapitalizeFirst()}" : null;
        }

    }

}

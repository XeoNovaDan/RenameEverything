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
        public bool allowMerge;

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
            return RenameUtility.GetRenamableCompGizmos(this);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    [StaticConstructorOnStartup]
    static class TexButton
    {

        public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete");
        public static readonly Texture2D RenameTex = ContentFinder<Texture2D>.Get("UI/Buttons/Rename");
        public static readonly Texture2D RecolourTex = ContentFinder<Texture2D>.Get("UI/Buttons/Recolour");
        public static readonly Texture2D AllowMergingTex = ContentFinder<Texture2D>.Get("UI/Buttons/AllowMerging");

    }

}

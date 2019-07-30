using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RenameEverything
{

    public class Dialog_RecolourLabel : Window
    {

        private const int SliderHeight = 25;
        private const int SliderGap = 15;
        private const int SliderWidth = 255;
        private const int PreviewSquareSize = 135;

        public Dialog_RecolourLabel(List<CompRenamable> renamableComps)
        {
            this.renamableComps = renamableComps;
            if (renamableComps.Count == 1)
                SetColour(renamableComps[0].Colour);
            else
                SetColour(Color.white);
        }

        public Dialog_RecolourLabel(CompRenamable renamableComp)
        {
            renamableComps = new List<CompRenamable>() { renamableComp };
            SetColour(renamableComp.Colour);
        }

        private void SetColour(Color colour)
        {
            Log.Message(colour.ToStringSafe());
            r = colour.r;
            g = colour.g;
            b = colour.b;
            a = colour.a;
        }

        public override Vector2 InitialSize => new Vector2(480, 280);

        public override void DoWindowContents(Rect inRect)
        {
            bool enterPressed = false;
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                enterPressed = true;
                Event.current.Use();
            }

            // Sliders
            GUI.SetNextControlName("RecolourSliders");
            r = Widgets.HorizontalSlider(new Rect(15, 15, SliderWidth, SliderHeight), r, 0, 1, leftAlignedLabel: "R", rightAlignedLabel: (r * 255).ToString("0"));
            g = Widgets.HorizontalSlider(new Rect(15, 15 + SliderHeight + SliderGap, SliderWidth, SliderHeight), g, 0, 1, leftAlignedLabel: "G", rightAlignedLabel: (g * 255).ToString("0")); ;
            b = Widgets.HorizontalSlider(new Rect(15, 15 + (SliderHeight + SliderGap) * 2, SliderWidth, SliderHeight), b, 0, 1, leftAlignedLabel: "B", rightAlignedLabel: (b * 255).ToString("0"));
            a = Widgets.HorizontalSlider(new Rect(15, 15 + (SliderHeight + SliderGap) * 3, SliderWidth, SliderHeight), a, 0, 1, leftAlignedLabel: "A", rightAlignedLabel: (a * 100).ToString("0"));

            // Preview
            GUI.color = CurColour;
            Widgets.DrawTextureFitted(new Rect(15 + SliderWidth + 30, SliderGap, PreviewSquareSize, PreviewSquareSize), TexButton.ColourPreviewTex, 1);
            GUI.color = Color.white;

            // 'OK' button
            var okButtonRect = new Rect(15, inRect.height - 35 - 15, 110, 35);
            if (Widgets.ButtonText(okButtonRect, "OK") || enterPressed)
            {
                foreach (var renamableComp in renamableComps)
                    renamableComp.Colour = CurColour;
                Find.WindowStack.TryRemove(this);
            }

            // 'Reset' button
            var resetButtonRect = new Rect(okButtonRect.x + okButtonRect.width + 20, okButtonRect.y, okButtonRect.width, okButtonRect.height);
            if (Widgets.ButtonText(resetButtonRect, "ResetButton".Translate()))
                SetColour(Color.white);
        }

        private Color CurColour => new Color(r, g, b, a);

        private float r;
        private float g;
        private float b;
        private float a;

        private List<CompRenamable> renamableComps = new List<CompRenamable>();

    }

}

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RebootVan.Content.Rarities
{
	public class RebootRarity : ModRarity
	{
        public override Color RarityColor => ColorSwap(new Color(109, 217, 253), new Color(69, 187, 253), 4f);

        public static Color ColorSwap(Color firstColor, Color secondColor, float seconds)
        {
            double timeMult = (double)(MathHelper.TwoPi / seconds);
            float lerpValue = (float)((Math.Sin(timeMult*Main.GlobalTimeWrappedHourly) + 1) * 0.5f);
            return Color.Lerp(firstColor, secondColor, lerpValue);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RebootVan.Content.Items;
using ReLogic.Content;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RebootVan.Content.Tiles
{
    public class RebootVanTile : ModTile
    {
        private Asset<Texture2D> glowTexture;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileSpelunker[Type] = true;

            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16, 16, 16, 16];
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            // Additional edits here, such as lava immunity, alternate placements, and subtiles
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(128, 128, 158), Language.GetText("TileText.RebootVanTile"));
            TileID.Sets.DisableSmartCursor[Type] = true;
            MinPick = 300;
            AnimationFrameHeight = 108;
            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
			if (++frameCounter >= 6) {
				frameCounter = 0;
				frame = ++frame % 24;
			}
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            // If you are using ModTile.SpecialDraw or PostDraw or PreDraw, use this snippet and add zero to all calls to spriteBatch.Draw
            // The reason for this is to accommodate the shift in drawing coordinates that occurs when using the different Lighting mode
            // Press Shift+F9 to change lighting modes quickly to verify your code works for all lighting modes
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

            // Because height of third tile is different we change it
            int height = tile.TileFrameY % AnimationFrameHeight == 36 ? 18 : 16;

            // Offset along the Y axis depending on the current frame
            int frameYOffset = Main.tileFrame[Type] * AnimationFrameHeight;

            // Firstly we draw the original texture and then glow mask texture
            spriteBatch.Draw(
                TextureAssets.Tile[Type].Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 0f);
            // Make sure to draw with Color.White or at least a color that is fully opaque
            // Achieve opaqueness by increasing the alpha channel closer to 255. (lowering closer to 0 will achieve transparency)
            spriteBatch.Draw(
                glowTexture.Value,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.TileFrameX, tile.TileFrameY + frameYOffset, 16, height),
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            // Return false to stop vanilla draw
            return false;
        }
        public override bool CanExplode(int i, int j) => false;

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (!Main.LocalPlayer.HasItem(ModContent.ItemType<BluGlo>()))
                return true;

            int topX = i - tile.TileFrameX % 144 / 18;
            int topY = j - tile.TileFrameY % 108 / 18;
            for (int k = 0; k < 8; k++)
            {
                for (int l = 0; l < 6; l++)
                {
                    Main.tile[topX+k, topY+l].TileType = (ushort)ModContent.TileType<ActiveRebootVanTile>();
                }
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, topX, topY, 8, 6);
            }
            Main.LocalPlayer.ConsumeItem(ModContent.ItemType<BluGlo>(), true);
            SoundStyle ChugJug = new SoundStyle($"{nameof(RebootVan)}/Sounds/ChugJug")
            {
                Volume = 0.75f,
                Pitch = 0f
            };
            SoundEngine.PlaySound(ChugJug, new(i * 16, j * 16));
            return true;
        }
        

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<BluGlo>();
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<BluGlo>();
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }
    }
}
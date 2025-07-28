using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RebootVan.Content.Items;
using RebootVan.Content.Tiles;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RebootVan
{
    public class PrismaUtils
    {
        public static Condition CreateNewCondition(string localisationEntry, Func<bool> boolean)
        {
            return new Condition(Language.GetText($"Mods.PrismaAlchemy.Condition.{localisationEntry}"), boolean);
        }

        public static void DisplayText(string text)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(text, Color.White);
            else if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.White);
        }

        public static void DisplayColorText(string text, Color textColor)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(Language.GetTextValue(text), textColor);
            else if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(text), textColor);
        }

        public static void RefillVan()
        {
            SoundStyle ChugJug = new SoundStyle($"{nameof(RebootVan)}/Sounds/ChugJug")
            {
                Volume = 0.75f,
                Pitch = 0f
            };
            SoundEngine.PlaySound(ChugJug);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                bool foundVan = false;
                bool foundActiveVan = false;
                int foundI = 0;
                int foundJ = 0;
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].TileType == ModContent.TileType<RebootVanTile>())
                        {
                            foundI = i;
                            foundJ = j;
                            foundVan = true;
                            break;
                        }
                        else if (Main.tile[i, j].TileType == ModContent.TileType<ActiveRebootVanTile>())
                        {
                            foundI = i;
                            foundJ = j;
                            foundActiveVan = true;
                            break;
                        }
                    }
                }
                if (foundVan)
                {
                    Tile tile = Main.tile[foundI, foundJ];

                    int topX = foundI - tile.TileFrameX % 144 / 18;
                    int topY = foundJ - tile.TileFrameY % 108 / 18;
                    for (int k = 0; k < 8; k++)
                    {
                        for (int l = 0; l < 6; l++)
                        {
                            Main.tile[topX + k, topY + l].TileType = (ushort)ModContent.TileType<ActiveRebootVanTile>();
                        }
                    }
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        NetMessage.SendTileSquare(-1, topX, topY, 8, 6);
                    }
                    SoundEngine.PlaySound(SoundID.Item3);
                    PrismaUtils.DisplayColorText("The Reboot Van is online!", Color.Aqua);
                }
                else if (foundActiveVan)
                {
                    Tile tile = Main.tile[foundI, foundJ];

                    int topX = foundI - tile.TileFrameX % 144 / 18;
                    int topY = foundJ - tile.TileFrameY % 108 / 18;
                    Item.NewItem(Entity.GetSource_None(), new Vector2(topX * 16 + 16, topY * 16 + 50), ModContent.ItemType<BluGlo>());
                    PrismaUtils.DisplayColorText("The Reboot Van dropped a BluGlo!", Color.Aqua);
                }
            });
        }

        public static void DoubleRefillVan()
        {
            SoundStyle ChugJug = new SoundStyle($"{nameof(RebootVan)}/Sounds/ChugJug")
            {
                Volume = 0.75f,
                Pitch = 0f
            };
            SoundEngine.PlaySound(ChugJug);
            ThreadPool.QueueUserWorkItem(_ =>
            {
                int foundI = 0;
                int foundJ = 0;
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].TileType == ModContent.TileType<RebootVanTile>())
                        {
                            foundI = i;
                            foundJ = j;
                            break;
                        }
                        else if (Main.tile[i, j].TileType == ModContent.TileType<ActiveRebootVanTile>())
                        {
                            foundI = i;
                            foundJ = j;
                            break;
                        }
                    }
                }
                Tile tile = Main.tile[foundI, foundJ];

                int topX = foundI - tile.TileFrameX % 144 / 18;
                int topY = foundJ - tile.TileFrameY % 108 / 18;
                Item.NewItem(Entity.GetSource_None(), new Vector2(topX * 16 + 16, topY * 16 + 50), ModContent.ItemType<BluGlo>());
                Item.NewItem(Entity.GetSource_None(), new Vector2(topX * 16 + 16, topY * 16 + 50), ModContent.ItemType<BluGlo>());
                PrismaUtils.DisplayColorText("The Reboot Van dropped two BluGlo!", Color.Aqua);
            });
        }
    }
}
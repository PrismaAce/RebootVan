using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Terraria.DataStructures;
using RebootVan.Content.Tiles;

namespace RebootVan
{
    public class GolemMirror : ModItem
    {
        // shameless examplemirror moment
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.maxStack = 1;
            Item.value = 0;
            Item.useAnimation = 80;
            Item.useTime = 80;
            Item.consumable = false;
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item6;

            Item.useStyle = ItemUseStyleID.HoldUp; // Holds up like a summon item.
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            // Each frame, make some dust
            if (Main.rand.NextBool())
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, Color.Orange, 1.1f); // Makes dust from the player's position and copies the hitbox of which the dust may spawn. Change these arguments if needed.
            }

            // This sets up the itemTime correctly.
            if (player.itemTime == 0)
            {
                player.ApplyItemTime(Item);
            }
            else if (player.itemTime == player.itemTimeMax / 2)
            {
                // This code runs once halfway through the useTime of the Item. You'll notice with magic mirrors you are still holding the item for a little bit after you've teleported.

                // Make dust 70 times for a cool effect.
                for (int d = 0; d < 70; d++)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, player.velocity.X * 0.5f, player.velocity.Y * 0.5f, 150, default, 1.5f);
                }

                // This code releases all grappling hooks and kills/despawns them.
                player.RemoveAllGrapplingHooks();

                // The actual method that moves the player back to bed/spawn.
                // recycled from my shimmer scanner
                bool foundShimmer = false;
                int teleX = 0;
                int teleY = 0;
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (Main.tile[i, j].TileType == ModContent.TileType<RebootVanTile>() || Main.tile[i, j].TileType == ModContent.TileType<ActiveRebootVanTile>())
                        {
                            teleX = i;
                            teleY = j;
                            foundShimmer = true;
                            break;
                        }
                    }
                }
                if (foundShimmer && Main.myPlayer == player.whoAmI)
                {
                    player.Teleport(new(teleX * 16f, teleY * 16f), 11);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, teleX * 16, teleY * 16, 1);
                }
                else
                {
                    CombatText.NewText(player.Hitbox, new Color(255, 149, 43), "Altar not found!");
                }
                

                // Make dust 70 times for a cool effect. This dust is the dust at the destination.
                for (int d = 0; d < 70; d++)
                {
                    Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 0f, 0f, 150, default, 1.5f);
                }
            }
        }
    }
}

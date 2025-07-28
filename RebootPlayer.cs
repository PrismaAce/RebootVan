using Microsoft.Xna.Framework;
using RebootVan.Content.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RebootVan
{
    public class RebootPlayer : ModPlayer
    {
        public bool GhostAtTheMoment;
        public bool TBTHardcore;
        public Vector2 telepoint;

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (GhostAtTheMoment) return false;
            else return true;
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Main.myPlayer == Player.whoAmI)
            {
                int i = Item.NewItem(Player.GetSource_DropAsItem(), Player.Hitbox, ModContent.ItemType<RebootCardItem>());
                var reboot = Main.item[i].ModItem as RebootCardItem;
                reboot.PlayerName = Player.name;
                reboot.LoadedPlayer = true;
                if (Player.difficulty == 2) {
                    Player.difficulty = 1;
                    Player.DropItems();
                    GhostAtTheMoment = true;
                }
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
                }
            }
        }
        public override void PostUpdate()
        {
            if (GhostAtTheMoment)
            {
                Player.ghost = true;
                Player.immune = true;
                Player.immuneTime = 180;
                Player.hostile = false;
                Player.DropItems();
                Player.DelBuff(0);
            }
            if (TBTHardcore)
            {
                Player.difficulty = 2;
                Player.Teleport(telepoint, 11);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, Player.whoAmI, telepoint.X, telepoint.Y, 1);
                Player.AddImmuneTime(ImmunityCooldownID.General, 300);
                Player.AddImmuneTime(ImmunityCooldownID.Lava, 300);
                telepoint = Vector2.Zero;
                TBTHardcore = false;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["GhostAtTheMoment"] = GhostAtTheMoment;
            tag["TBTHardcore"] = TBTHardcore;
        }

        public override void LoadData(TagCompound tag)
        {
            GhostAtTheMoment = tag.GetBool("GhostAtTheMoment");
            TBTHardcore = tag.GetBool("TBTHardcore");
        }
    }
}

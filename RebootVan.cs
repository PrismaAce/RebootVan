using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RebootVan
{
	public class RebootVan : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte msgType = reader.ReadByte();
            switch (msgType)
            {
                case 0:
                    int plr = reader.ReadInt32();
                    int tpx = reader.ReadInt32();
                    int tpy = reader.ReadInt32();
                    Vector2 telepoint = new(tpx, tpy);
                    if (Main.player[plr].GetModPlayer<RebootPlayer>().GhostAtTheMoment)
                    {
                        Main.player[plr].GetModPlayer<RebootPlayer>().GhostAtTheMoment = false;
                        Main.player[plr].ghost = false;
                        Main.player[plr].GetModPlayer<RebootPlayer>().TBTHardcore = true;
                        Main.player[plr].GetModPlayer<RebootPlayer>().telepoint = telepoint;
                        PrismaUtils.DisplayText(Main.player[plr].name + " was rebooted");
                    }
                    Main.player[plr].respawnTimer = 1;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket killStar = ModContent.GetInstance<RebootVan>().GetPacket();
                        killStar.Write((byte)0);
                        killStar.Write(plr);
                        killStar.Write(tpx);
                        killStar.Write(tpy);
                        killStar.Send(-1, whoAmI);
                    }
                    break;
                default:
                    Logger.WarnFormat("this message shouldnt appear lol {0}", msgType);
                    break;
            }
        }
    }
}

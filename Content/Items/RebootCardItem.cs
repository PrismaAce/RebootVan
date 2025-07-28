using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using RebootVan.Content.Rarities;
using Terraria.GameContent.Tile_Entities;

namespace RebootVan.Content.Items
{
    public class RebootCardItem : ModItem
    {
        public bool LoadedPlayer = false;
        public bool PlayerIsOnline = false;
        public string PlayerName = "";

        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(8, 24));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;
            Item.value = Item.sellPrice(1, 0, 0, 0);
            Item.rare = RarityType<RebootRarity>();
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            IEntitySource source = Item.GetSource_DropAsItem();
            TooltipLine tooltipLine = list.FirstOrDefault((TooltipLine x) => x.Mod == "Terraria" && x.Name == "Tooltip0");
            DefaultInterpolatedStringHandler str = default(DefaultInterpolatedStringHandler);
            DefaultInterpolatedStringHandler str2 = default(DefaultInterpolatedStringHandler);
            string Display = "";
            if (LoadedPlayer)
            {
                Display = PlayerName;
                str.AppendLiteral("Contains [c/FF00FF:");
                str.AppendFormatted(Display);
                str.AppendLiteral("]'s lifeforce");
            }
            else
            {
                Display = "No Loaded Player";
                str.AppendLiteral("[c/BBBBBB:");
                str.AppendFormatted(Display);
                str.AppendLiteral("]");
            }
            if (!PlayerIsOnline)
            {
                str2.AppendLiteral("[c/999999:This player is not online, so this card is temporarily disabled.]");
            }
            TooltipLine tooltipLine1 = new TooltipLine(Mod, "i1", str.ToStringAndClear());
            list.Add(tooltipLine1);
            TooltipLine tooltipLine2 = new TooltipLine(Mod, "i2", str2.ToStringAndClear());
            list.Add(tooltipLine2);
        }

        public override void UpdateInventory(Player player)
        {
            bool safe = false;
            if (LoadedPlayer)
            {
                foreach (Player p in Main.ActivePlayers)
                {
                    if (p.name == PlayerName)
                    {
                        PlayerIsOnline = true;
                        safe = true;
                        break;
                    }
                }
            }
            if (!safe)
            {
                PlayerIsOnline = false;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag["LoadedPlayer"] = LoadedPlayer;
            tag["PlayerName"] = PlayerName;
        }

        public override void LoadData(TagCompound tag)
        {
            LoadedPlayer = tag.GetBool("LoadedPlayer");
            PlayerName = tag.GetString("PlayerName");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(LoadedPlayer);
            writer.Write(PlayerName);
        }

        public override void NetReceive(BinaryReader reader)
        {
            LoadedPlayer = reader.ReadBoolean();
            PlayerName = reader.ReadString();
        }
    }
}

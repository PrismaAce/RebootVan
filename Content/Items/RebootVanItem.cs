using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using RebootVan.Content.Tiles;
using RebootVan.Content.Rarities;

namespace RebootVan.Content.Items
{
    public class RebootVanItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 14;
            Item.rare = ModContent.RarityType<RebootRarity>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<RebootVanTile>();
        }
    }
}
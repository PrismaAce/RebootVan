using Microsoft.Xna.Framework;
using RebootVan.Content.Items;
using RebootVan.Content.Tiles;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.ModBrowser;

namespace RebootVan
{
    public class CardCommand : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "card";

		public override string Usage
			=> "/card [name on card] - if there's a space, use \\_ instead";

		public override string Description
			=> "Spawns a Reboot Card of the described player";

        public override bool IsCaseSensitive => true;

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
                throw new UsageException("At least one argument was expected.");

            if (!ModContent.GetInstance<RebootConfig>().DebugMode)
                throw new UsageException("Debug mode is disabled.");

            string editedstring = args[0].Replace("\\_", " ");
            Main.NewText(editedstring);
            int i = Item.NewItem(new EntitySource_DebugCommand($"{nameof(RebootVan)}_{nameof(CardCommand)}"), caller.Player.Center, ModContent.ItemType<RebootCardItem>());
            var reboot = Main.item[i].ModItem as RebootCardItem;
            reboot.PlayerName = editedstring;
            reboot.LoadedPlayer = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
            }

        }
    }
}
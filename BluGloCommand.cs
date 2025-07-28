using Microsoft.Xna.Framework;
using RebootVan.Content.Items;
using RebootVan.Content.Tiles;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RebootVan
{
    public class BluGloCommand : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;

		public override string Command
			=> "triggervan";

		public override string Usage
			=> "/triggervan";

		public override string Description
			=> "Trigger reboot van to reactivate";

		public override void Action(CommandCaller caller, string input, string[] args) {
            if (!ModContent.GetInstance<RebootConfig>().DebugMode)
                throw new UsageException("Enable debug mode to use the debug commands");
            else PrismaUtils.RefillVan();
        }


    }
}
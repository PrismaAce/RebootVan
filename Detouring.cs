using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace RebootVan
{
    public class Detouring : ModSystem
    {
        public override void Load()
        {
            On_Player.KillMeForGood += On_Player_KillMeForGood;

        }

        private void On_Player_KillMeForGood(Terraria.On_Player.orig_KillMeForGood orig, Terraria.Player self)
        {
            return;
        }
    }
}

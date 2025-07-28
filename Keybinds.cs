using Terraria.GameInput;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace RebootVan
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind GhostWarp { get; private set; }
        public static ModKeybind UnlockMediumcoreGhost { get; private set; }
        public static ModKeybind LockMediumcoreGhost { get; private set; }

        public override void Load()
        {
            GhostWarp = KeybindLoader.RegisterKeybind(Mod, "GhostWarp", "Q");
            UnlockMediumcoreGhost = KeybindLoader.RegisterKeybind(Mod, "UnlockMediumcoreGhost", "V");
            LockMediumcoreGhost = KeybindLoader.RegisterKeybind(Mod, "LockMediumcoreGhost", "B");
        }

        public override void Unload()
        {
            GhostWarp = null;
            UnlockMediumcoreGhost = null;
            LockMediumcoreGhost = null;
        }
    }

    public class ExampleKeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.GhostWarp.JustPressed && Player.ghost)
            {
                Player.Teleport(Main.MouseWorld, 12);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, Player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y, 12);
            }
            if (KeybindSystem.UnlockMediumcoreGhost.JustPressed && Player.ghost)
            {
                Player.dead = false;
            }
            if (KeybindSystem.LockMediumcoreGhost.JustPressed && Player.ghost)
            {
                Player.dead = true;
            }
        }
    }
}

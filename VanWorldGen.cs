// 1. You'll need various using statements. Visual Studio will suggest these if they are missing, but most are listed here for convenience.
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.IO;
using Terraria.Localization;

namespace RebootVan
{
    // tmod wiki
    // 2. Our world generation code must start from a class extending ModSystem
    public class VanWorldGen : ModSystem
    {
        // 3. These lines setup the localization for the message shown during world generation. Update your localization files after building and reloading the mod to provide values for this.
        public static LocalizedText VanPassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            VanPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(VanPassMessage)}"));
        }

        // 4. We use the ModifyWorldGenTasks method to tell the game the order that our world generation code should run
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // 5. We use FindIndex to locate the index of the vanilla world generation task called "Shinies". This ensures our code runs at the correct step.
            int GuideIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Guide"));
            if (GuideIndex != -1)
            {
                // 6. We register our world generation pass by passing in an instance of our custom GenPass class below. The GenPass class will execute our world generation code.
                tasks.Insert(GuideIndex + 1, new VanPass("Van GenPass", 100f));
            }
        }
    }

    // 7. Make sure to inherit from the GenPass class.
    public class VanPass : GenPass
    {
        public VanPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        // 8. The ApplyPass method is where the actual world generation code is placed.
        // UH OHHHHHHHHHH
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            int x;
            if (WorldGen.genRand.NextBool())
            {
                // Between 1/6 and 2/6
                x = WorldGen.genRand.Next(Main.maxTilesX / 6, Main.maxTilesX*2 / 6);
            }
            else
            {
                // Between 4/6 and 5/6
                x = WorldGen.genRand.Next(Main.maxTilesX*4/ 6, Main.maxTilesX*5/6);
            }
            
            // HOW DO I DODGE FLOATING ISLANDS ON ANY WORLD SIZE HELP
            bool foundSurface = false;
            int y = 200;
            while (y < Main.worldSurface)
            {
                if (WorldGen.SolidTile(x, y))
                {
                    foundSurface = true;
                    break;
                }
                y++;
            }

            if (foundSurface)
            {
                // one day i'll learn how to do this myself but uh
                StructureHelper.API.Generator.GenerateStructure("Structures/RebootVan", new Terraria.DataStructures.Point16(x-9, y-14), ModContent.GetInstance<RebootVan>());
            }
        }
    }
}
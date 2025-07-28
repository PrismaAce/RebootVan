using Microsoft.Xna.Framework;
using RebootVan.Content.Items;
using RebootVan.Content.Tiles;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RebootVan.Content
{
    public class PrismaAlchemyGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void OnKill(NPC npc)
        {
            bool secondTwin = false;
            if (npc.type == NPCID.Retinazer)
                secondTwin = !NPC.AnyNPCs(NPCID.Spazmatism);
            else if (npc.type == NPCID.Spazmatism)
                secondTwin = !NPC.AnyNPCs(NPCID.Retinazer);

            if ((npc.boss && (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)) || npc.type == NPCID.BrainofCthulhu)
            {
                if (!NPC.downedBoss2)
                {
                    PrismaUtils.RefillVan();
                }
            }
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    if (!NPC.downedSlimeKing) {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.EyeofCthulhu:
                    if (!NPC.downedBoss1)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.QueenBee:
                    if (!NPC.downedQueenBee)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.SkeletronHead:
                    if (!NPC.downedBoss3)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.Deerclops:
                    if (!NPC.downedDeerclops)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.WallofFlesh:
                    if (!Main.hardMode)
                    {
                        PrismaUtils.DoubleRefillVan();
                    }
                    break;
                case NPCID.QueenSlimeBoss:
                    if (!NPC.downedQueenSlime)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.TheDestroyer:
                    if (!NPC.downedMechBoss1)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.Spazmatism:
                case NPCID.Retinazer:
                    if (!NPC.downedMechBoss2 && secondTwin)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.SkeletronPrime:
                    if (!NPC.downedMechBoss3)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.Plantera:
                    if (!NPC.downedPlantBoss)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.Golem:
                    if (!NPC.downedGolemBoss)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.DukeFishron:
                    if (!NPC.downedFishron)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.HallowBoss:
                    if (!NPC.downedEmpressOfLight)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                case NPCID.CultistBoss:
                    if (!NPC.downedAncientCultist)
                    {
                        PrismaUtils.RefillVan();
                    }
                    break;
                /*case NPCID.MoonLordCore:
                    if (!NPC.downedMoonlord)
                    {
                        RefillVan();
                    }
                    break;*/
            }
        }
    }
}
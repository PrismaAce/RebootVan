using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ModLoader.Config;

namespace RebootVan
{
    public class RebootConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        /*private const int Min = 0;
        private const int Max = 255;

        [Range(Min, Max)]
        [DefaultValue(0)]
        public int FakeScaling { get; set; }*/

        [DefaultValue(false)]
        public bool DebugMode;
    }
}
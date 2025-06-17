namespace LuneWoL.Common.LWoLWorldGen;

internal class OreDensityGenSystem : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int jngIndex = tasks.FindIndex(pass => pass.Name == "Jungle");
        if (jngIndex != -1)
        {
            tasks.RemoveAt(jngIndex);
            tasks.Insert(jngIndex, new JunglePass());
        }

        int DTMIndex = tasks.FindIndex(pass => pass.Name == "Dirt To Mud");
        if (DTMIndex != -1)
        {
            tasks.RemoveAt(DTMIndex);
            tasks.Insert(DTMIndex, new PassLegacy("Dirt To Mud", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateDirtToMud(progress);
            }));
        }

        int siltIndex = tasks.FindIndex(pass => pass.Name == "Silt");
        if (siltIndex != -1)
        {
            tasks.RemoveAt(siltIndex);
            tasks.Insert(siltIndex, new PassLegacy("Silt", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateSilt(progress);
            }));
        }
        int shiniesIndex = tasks.FindIndex(pass => pass.Name == "Shinies");
        if (shiniesIndex != -1)
        {
            tasks.RemoveAt(shiniesIndex);
            tasks.Insert(shiniesIndex, new PassLegacy("Shinies", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateOres(progress);
            }));
        }

        int UIndex = tasks.FindIndex(pass => pass.Name == "Underworld");
        if (UIndex != -1)
        {
            tasks.RemoveAt(UIndex);
            tasks.Insert(UIndex, new PassLegacy("Underworld", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateUnderworld(progress);
            }));
        }

        int SlushIndex = tasks.FindIndex(pass => pass.Name == "Gems");
        if (SlushIndex != -1)
        {
            tasks.RemoveAt(SlushIndex);
            tasks.Insert(SlushIndex, new PassLegacy("Gems", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateGems(progress);
            }));
        }

        int rigIndex = tasks.FindIndex(pass => pass.Name == "Gems In Ice Biome");
        if (rigIndex != -1)
        {
            tasks.RemoveAt(rigIndex);
            tasks.Insert(rigIndex, new PassLegacy("Gems In Ice Biome", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateIceGems(progress);
            }));
        }

        int RdmGIndex = tasks.FindIndex(pass => pass.Name == "Random Gems");
        if (RdmGIndex != -1)
        {
            tasks.RemoveAt(RdmGIndex);
            tasks.Insert(RdmGIndex, new PassLegacy("Random Gems", delegate (GenerationProgress progress, GameConfiguration config)
            {
                GenerateRandomGems(progress);
            }));
        }

        int MBGIndex = tasks.FindIndex(pass => pass.Name == "Micro Biomes");
        if (MBGIndex != -1)
        {
            tasks.RemoveAt(MBGIndex);
            tasks.Insert(MBGIndex, new PassLegacy("Micro Biomes", GenerateMicroBiomes));
        }
    }

    #region ore meth
    private static int PreHardSS(int num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.PrehardmodeOreAmountPercent == 100) return num;

        num = (int)(num * (float)LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.PrehardmodeOreAmountPercent / 100);
        return num;
    }

    private static double PreHardDensity(double num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.PrehardmodeOreDensityPercent == 100) return num;

        num = num * LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.PrehardmodeOreDensityPercent / 100;
        return num;
    }
    #endregion

    #region gems meth
    internal static int GemSS(int num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.GemStoneAmountPercent == 100) return num;

        num = (int)(num * (float)LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.GemStoneAmountPercent / 100);
        return num;
    }

    internal static double GemDensity(double num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.GemStoneDensityPercent == 100) return num;

        num = num * LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.GemStoneDensityPercent / 100;
        return num;
    }
    #endregion

    #region Slush meth
    private static int SlushSS(int num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SlushAmountPercent == 100) return num;

        num = (int)(num * (float)LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SlushAmountPercent / 100);
        return num;
    }

    private static double SlushDensity(double num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SlushDensityPercent == 100) return num;

        num = num * LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SlushDensityPercent / 100;
        return num;
    }
    #endregion

    #region Silt meth
    private static int SiltSS(int num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SiltAmountPercent == 100) return num;

        num = (int)(num * (float)LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SiltAmountPercent / 100);
        return num;
    }

    private static double SiltDensity(double num)
    {
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SiltDensityPercent == 100) return num;

        num = num * LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.SiltDensityPercent / 100;
        return num;
    }
    #endregion

    public void GenerateOres(GenerationProgress progress)
    {
        {
            progress.Message = Lang.gen[16].Value;
            #region remix shit
            if (WorldGen.remixWorldGen)
            {
                for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00006)); i++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(2), PreHardSS(6)), GenVars.copper);
                }
                for (int j = 0; j < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00008)); j++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), GenVars.copper);
                }
                for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0002)); k++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.copper);
                }
                for (int l = 0; l < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00003)); l++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), WorldGen.genRand.Next(PreHardSS(2), PreHardSS(5)), GenVars.iron);
                }
                for (int m = 0; m < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00008)); m++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), GenVars.iron);
                }
                for (int n = 0; n < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0002)); n++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.iron);
                }
                for (int num = 0; num < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.000026)); num++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer - 100, Main.maxTilesY - 250), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), GenVars.silver);
                }
                for (int num2 = 0; num2 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00015)); num2++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.silver);
                }
                for (int num3 = 0; num3 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00017)); num3++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.silver);
                }
                for (int num4 = 0; num4 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00012)); num4++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.gold = WorldGen.genRand.NextBool(2) ? 8 : 169;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.gold);
                }
                for (int num5 = 0; num5 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00012)); num5++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.gold = WorldGen.genRand.NextBool(2) ? 8 : 169;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow - 20), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.gold);
                }
                if (Main.drunkWorld)
                {
                    for (int num6 = 0; num6 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225) / 2.0); num6++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 204);
                    }
                    for (int num7 = 0; num7 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225) / 2.0); num7++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 22);
                    }
                }
                if (WorldGen.crimson)
                {
                    for (int num8 = 0; num8 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000425)); num8++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 204);
                    }
                }
                else
                {
                    for (int num9 = 0; num9 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000425)); num9++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 22);
                    }
                }
            }
            #endregion
            else
            {
                for (int num10 = 0; num10 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00006)); num10++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(2), PreHardSS(6)), GenVars.copper);
                }
                for (int num11 = 0; num11 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00008)); num11++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), GenVars.copper);

                }
                for (int num12 = 0; num12 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0002)); num12++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.copper = WorldGen.genRand.NextBool(2) ? 7 : 166;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.copper);
                }
                for (int num13 = 0; num13 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00003)); num13++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, (int)GenVars.worldSurfaceHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), WorldGen.genRand.Next(PreHardSS(2), PreHardSS(5)), GenVars.iron);
                }
                for (int num14 = 0; num14 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00008)); num14++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), GenVars.iron);
                }
                for (int num15 = 0; num15 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0002)); num15++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.iron = WorldGen.genRand.NextBool(2) ? 6 : 167;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.iron);
                }
                for (int num16 = 0; num16 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.000026)); num16++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.worldSurfaceHigh, (int)GenVars.rockLayerHigh), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), GenVars.silver);
                }
                for (int num17 = 0; num17 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00015)); num17++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.silver);
                }
                for (int num18 = 0; num18 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00017)); num18++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.silver = WorldGen.genRand.NextBool(2) ? 9 : 168;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(9)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.silver);
                }
                for (int num19 = 0; num19 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00012)); num19++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.gold = WorldGen.genRand.NextBool(2) ? 8 : 169;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.gold);
                }
                for (int num20 = 0; num20 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.00012)); num20++)
                {
                    if (Main.drunkWorld)
                    {
                        GenVars.gold = WorldGen.genRand.NextBool(2) ? 8 : 169;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)GenVars.worldSurfaceLow - 20), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), GenVars.gold);
                }
                if (Main.drunkWorld)
                {
                    for (int num21 = 0; num21 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225) / 2.0); num21++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 204);
                    }
                    for (int num22 = 0; num22 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225) / 2.0); num22++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 22);
                    }
                }
                if (WorldGen.crimson)
                {
                    for (int num23 = 0; num23 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225)); num23++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 204);
                    }
                }
                else
                {
                    for (int num24 = 0; num24 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0000225)); num24++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(6)), WorldGen.genRand.Next(PreHardSS(4), PreHardSS(8)), 22);
                    }
                }
            }
        };
    }
    public void GenerateGems(GenerationProgress progress)
    {
        progress.Message = Lang.gen[23].Value;
        Main.tileSolid[484] = false;
        for (int i = 63; i <= 68; i++)
        {
            double value = (i - 63) / 6.0;
            progress.Set(value);
            double num = 0.0;
            switch (i)
            {
                case 67:
                    num = Main.maxTilesX * GemDensity(0.5);
                    break;
                case 66:
                    num = Main.maxTilesX * GemDensity(0.45);
                    break;
                case 63:
                    num = Main.maxTilesX * GemDensity(0.3);
                    break;
                case 65:
                    num = Main.maxTilesX * GemDensity(0.25);
                    break;
                case 64:
                    num = Main.maxTilesX * GemDensity(0.1);
                    break;
                case 68:
                    num = Main.maxTilesX * GemDensity(0.05);
                    break;
            }
            num *= 0.2;
            for (int j = 0; j < num; j++)
            {
                int num2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                int num3 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
                while (Main.tile[num2, num3].TileType != 1)
                {
                    num2 = WorldGen.genRand.Next(0, Main.maxTilesX);
                    num3 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
                }
                WorldGen.TileRunner(num2, num3, WorldGen.genRand.Next(GemSS(2), GemSS(6)), WorldGen.genRand.Next(GemSS(3), GemSS(7)), i);
            }
        }
        for (int k = 0; k < 2; k++)
        {
            int num4 = 1;
            int num5 = 5;
            int num6 = Main.maxTilesX - 5;
            if (k == 1)
            {
                num4 = -1;
                num5 = Main.maxTilesX - 5;
                num6 = 5;
            }
            for (int l = num5; l != num6; l += num4)
            {
                if (l <= GenVars.UndergroundDesertLocation.Left || l >= GenVars.UndergroundDesertLocation.Right)
                {
                    for (int m = 10; m < Main.maxTilesY - 10; m++)
                    {
                        if (Main.tile[l, m].HasTile && Main.tile[l, m + 1].HasTile && Main.tileSand[Main.tile[l, m].TileType] && Main.tileSand[Main.tile[l, m + 1].TileType])
                        {
                            ushort type = Main.tile[l, m].TileType;
                            int x = l + num4;
                            int n = m + 1;
                            if (!Main.tile[x, m].HasTile && !Main.tile[x, n].HasTile)
                            {
                                for (; !Main.tile[x, n].HasTile; n++)
                                {
                                }
                                n--;
                                WorldGen.KillTile(l, m, noItem: true);
                                WorldGen.PlaceTile(x, n, type, mute: true, forced: true);
                                Main.tile[x, n].TileType = type;
                            }
                        }
                    }
                }
            }
        }
    }
    public void GenerateDirtToMud(GenerationProgress progress)
    {
        progress.Message = Lang.gen[14].Value;
        double num = Main.maxTilesX * Main.maxTilesY * SlushDensity(0.001);
        for (int i = 0; i < num; i++)
        {
            progress.Set(i / num);
            if (WorldGen.remixWorldGen)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)GenVars.rockLayerLow), WorldGen.genRand.Next(SlushSS(2), SlushSS(6)), WorldGen.genRand.Next(SlushSS(2), SlushSS(40)), 59, addTile: false, 0.0, 0.0, noYChange: false, overRide: true, 53);
            }
            else
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.maxTilesY), WorldGen.genRand.Next(SlushSS(2), SlushSS(6)), WorldGen.genRand.Next(SlushSS(2), SlushSS(40)), 59, addTile: false, 0.0, 0.0, noYChange: false, overRide: true, 53);
            }
        }
    }
    public void GenerateSilt(GenerationProgress progress)
    {
        progress.Message = Lang.gen[15].Value;
        for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * SiltDensity(0.0001)); i++)
        {
            int num = WorldGen.genRand.Next(0, Main.maxTilesX);
            int num2 = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY);
            if (WorldGen.remixWorldGen)
            {
                num2 = WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer);
            }
            if (Main.tile[num, num2].WallType != 187 && Main.tile[num, num2].WallType != 216)
            {
                WorldGen.TileRunner(num, num2, WorldGen.genRand.Next(SiltSS(5), SiltSS(12)), WorldGen.genRand.Next(SiltSS(15), SiltSS(50)), 123);
            }
        }
        for (int j = 0; j < (int)(Main.maxTilesX * Main.maxTilesY * SiltDensity(0.0005)); j++)
        {
            int num3 = WorldGen.genRand.Next(0, Main.maxTilesX);
            int num4 = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY);
            if (WorldGen.remixWorldGen)
            {
                num4 = WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer);
            }
            if (Main.tile[num3, num4].WallType != 187 && Main.tile[num3, num4].WallType != 216)
            {
                WorldGen.TileRunner(num3, num4, WorldGen.genRand.Next(SiltSS(2), SiltSS(5)), WorldGen.genRand.Next(SiltSS(2), SiltSS(5)), 123);
            }
        }
    }
    public void GenerateUnderworld(GenerationProgress progress)
    {
        progress.Message = Lang.gen[18].Value;
        progress.Set(0.0);
        int num = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
        for (int i = 0; i < Main.maxTilesX; i++)
        {
            num += WorldGen.genRand.Next(-3, 4);
            if (num < Main.maxTilesY - 190)
            {
                num = Main.maxTilesY - 190;
            }
            if (num > Main.maxTilesY - 160)
            {
                num = Main.maxTilesY - 160;
            }
            for (int j = num - 20 - WorldGen.genRand.Next(3); j < Main.maxTilesY; j++)
            {
                if (j >= num)
                {
                    WorldGen.KillTile(i, j, false, false, true);
                    WorldGen.EmptyLiquid(i, j);
                }
                else
                {
                    Main.tile[i, j].TileType = 57;
                }
            }
        }
        int num2 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
        for (int k = 10; k < Main.maxTilesX - 10; k++)
        {
            num2 += WorldGen.genRand.Next(-10, 11);
            if (num2 > Main.maxTilesY - 60)
            {
                num2 = Main.maxTilesY - 60;
            }
            if (num2 < Main.maxTilesY - 100)
            {
                num2 = Main.maxTilesY - 120;
            }
            for (int l = num2; l < Main.maxTilesY - 10; l++)
            {
                if (!Main.tile[k, l].HasTile)
                {
                    WorldGen.PlaceLiquid(k, l, 1, byte.MaxValue);
                }
            }
        }
        for (int m = 0; m < Main.maxTilesX; m++)
        {
            if (WorldGen.genRand.NextBool(50))
            {
                int num3 = Main.maxTilesY - 65;
                while (!Main.tile[m, num3].HasTile && num3 > Main.maxTilesY - 135)
                {
                    num3--;
                }
                WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num3 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, 57, addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
            }
        }
        Liquid.QuickWater(-2);
        for (int n = 0; n < Main.maxTilesX; n++)
        {
            double num4 = n / (double)(Main.maxTilesX - 1);
            progress.Set((num4 / 2.0) + 0.5);
            if (WorldGen.genRand.NextBool(13))
            {
                int num5 = Main.maxTilesY - 65;
                while ((Main.tile[n, num5].LiquidAmount > 0 || Main.tile[n, num5].HasTile) && num5 > Main.maxTilesY - 140)
                {
                    num5--;
                }
                if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.NextBool(3) || !(n > Main.maxTilesX * 0.4) || !(n < Main.maxTilesX * 0.6))
                {
                    WorldGen.TileRunner(n, num5 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, 57, addTile: true, 0.0, WorldGen.genRand.Next(1, 3), noYChange: true);
                }
                double num6 = WorldGen.genRand.Next(1, 3);
                if (WorldGen.genRand.NextBool(3))
                {
                    num6 *= 0.5;
                }
                if ((!WorldGen.drunkWorldGen && !WorldGen.remixWorldGen) || WorldGen.genRand.NextBool(3) || !(n > Main.maxTilesX * 0.4) || !(n < Main.maxTilesX * 0.6))
                {
                    if (WorldGen.genRand.NextBool(2))
                    {
                        WorldGen.TileRunner(n, num5 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num6), (int)(WorldGen.genRand.Next(10, 15) * num6), 57, addTile: true, 1.0, 0.3);
                    }
                    if (WorldGen.genRand.NextBool(2))
                    {
                        num6 = WorldGen.genRand.Next(1, 3);
                        WorldGen.TileRunner(n, num5 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num6), (int)(WorldGen.genRand.Next(10, 15) * num6), 57, addTile: true, -1.0, 0.3);
                    }
                }
                WorldGen.TileRunner(n + WorldGen.genRand.Next(-10, 10), num5 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                if (WorldGen.genRand.NextBool(3))
                {
                    WorldGen.TileRunner(n + WorldGen.genRand.Next(-10, 10), num5 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                }
                if (WorldGen.genRand.NextBool(5))
                {
                    WorldGen.TileRunner(n + WorldGen.genRand.Next(-15, 15), num5 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
                }
            }
        }
        for (int num7 = 0; num7 < Main.maxTilesX; num7++)
        {
            WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
        }
        if (WorldGen.drunkWorldGen || WorldGen.remixWorldGen)
        {
            for (int num8 = 0; num8 < Main.maxTilesX * 2; num8++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next((int)(Main.maxTilesX * 0.35), (int)(Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2);
            }
        }
        for (int num9 = 0; num9 < Main.maxTilesX; num9++)
        {
            if (!Main.tile[num9, Main.maxTilesY - 145].HasTile)
            {
                WorldGen.PlaceLiquid(num9, Main.maxTilesY - 145, 1, byte.MaxValue);
            }
            if (!Main.tile[num9, Main.maxTilesY - 144].HasTile)
            {
                WorldGen.PlaceLiquid(num9, Main.maxTilesY - 144, 1, byte.MaxValue);
            }
        }
        for (int num10 = 0; num10 < (int)(Main.maxTilesX * Main.maxTilesY * PreHardDensity(0.0008)); num10++)
        {
            WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(PreHardSS(2), PreHardSS(7)), WorldGen.genRand.Next(PreHardSS(3), PreHardSS(7)), 58); //Hellstone
        }
        if (WorldGen.remixWorldGen)
        {
            int num11 = (int)(Main.maxTilesX * 0.38);
            int num12 = (int)(Main.maxTilesX * 0.62);
            int num13 = num11;
            int num14 = Main.maxTilesY - 1;
            int num15 = Main.maxTilesY - 135;
            int num16 = Main.maxTilesY - 160;
            bool flag = false;
            Liquid.QuickWater(-2);
            for (; num14 < Main.maxTilesY - 1 || num13 < num12; num13++)
            {
                if (!flag)
                {
                    num14 -= WorldGen.genRand.Next(1, 4);
                    if (num14 < num15)
                    {
                        flag = true;
                    }
                }
                else if (num13 >= num12)
                {
                    num14 += WorldGen.genRand.Next(1, 4);
                    if (num14 > Main.maxTilesY - 1)
                    {
                        num14 = Main.maxTilesY - 1;
                    }
                }
                else
                {
                    if ((num13 <= (Main.maxTilesX / 2) - 5 || num13 >= (Main.maxTilesX / 2) + 5) && WorldGen.genRand.NextBool(4))
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            num14 += WorldGen.genRand.Next(-1, 2);
                        }
                        else if (WorldGen.genRand.NextBool(6))
                        {
                            num14 += WorldGen.genRand.Next(-2, 3);
                        }
                        else if (WorldGen.genRand.NextBool(8))
                        {
                            num14 += WorldGen.genRand.Next(-4, 5);
                        }
                    }
                    if (num14 < num16)
                    {
                        num14 = num16;
                    }
                    if (num14 > num15)
                    {
                        num14 = num15;
                    }
                }
                for (int num17 = num14; num17 > num14 - 20; num17--)
                {
                    Main.tile[num13, num17].LiquidAmount = 0;
                }
                for (int num18 = num14; num18 < Main.maxTilesY; num18++)
                {
                    WorldGen.KillTile(num13, num18, false, false, true);
                    WorldGen.PlaceTile(num13, num18, 57);
                }
            }
            Liquid.QuickWater(-2);
            for (int num19 = num11; num19 < num12 + 15; num19++)
            {
                for (int num20 = Main.maxTilesY - 300; num20 < num15 + 20; num20++)
                {
                    Main.tile[num19, num20].LiquidAmount = 0;
                    if (Main.tile[num19, num20].TileType == 57 && Main.tile[num19, num20].HasTile && (!Main.tile[num19 - 1, num20 - 1].HasTile || !Main.tile[num19, num20 - 1].HasTile || !Main.tile[num19 + 1, num20 - 1].HasTile || !Main.tile[num19 - 1, num20].HasTile || !Main.tile[num19 + 1, num20].HasTile || !Main.tile[num19 - 1, num20 + 1].HasTile || !Main.tile[num19, num20 + 1].HasTile || !Main.tile[num19 + 1, num20 + 1].HasTile))
                    {
                        Main.tile[num19, num20].TileType = 633;
                    }
                }
            }
            for (int num21 = num11; num21 < num12 + 15; num21++)
            {
                for (int num22 = Main.maxTilesY - 200; num22 < num15 + 20; num22++)
                {
                    if (Main.tile[num21, num22].TileType == 633 && Main.tile[num21, num22].HasTile && !Main.tile[num21, num22 - 1].HasTile && WorldGen.genRand.NextBool(3))
                    {
                        WorldGen.TryGrowingTreeByType(634, num21, num22);
                    }
                }
            }
        }
        else if (!WorldGen.drunkWorldGen)
        {
            for (int num23 = 25; num23 < Main.maxTilesX - 25; num23++)
            {
                if (num23 < Main.maxTilesX * 0.17 || num23 > Main.maxTilesX * 0.83)
                {
                    for (int num24 = Main.maxTilesY - 300; num24 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num24++)
                    {
                        if (Main.tile[num23, num24].TileType == 57 && Main.tile[num23, num24].HasTile && (!Main.tile[num23 - 1, num24 - 1].HasTile || !Main.tile[num23, num24 - 1].HasTile || !Main.tile[num23 + 1, num24 - 1].HasTile || !Main.tile[num23 - 1, num24].HasTile || !Main.tile[num23 + 1, num24].HasTile || !Main.tile[num23 - 1, num24 + 1].HasTile || !Main.tile[num23, num24 + 1].HasTile || !Main.tile[num23 + 1, num24 + 1].HasTile))
                        {
                            Main.tile[num23, num24].TileType = 633;
                        }
                    }
                }
            }
            for (int num25 = 25; num25 < Main.maxTilesX - 25; num25++)
            {
                if (num25 < Main.maxTilesX * 0.17 || num25 > Main.maxTilesX * 0.83)
                {
                    for (int num26 = Main.maxTilesY - 200; num26 < Main.maxTilesY - 50; num26++)
                    {
                        if (Main.tile[num25, num26].TileType == 633 && Main.tile[num25, num26].HasTile && !Main.tile[num25, num26 - 1].HasTile && WorldGen.genRand.NextBool(3))
                        {
                            WorldGen.TryGrowingTreeByType(634, num25, num26);
                        }
                    }
                }
            }
        }
        WorldGen.AddHellHouses();
        if (WorldGen.drunkWorldGen)
        {
            for (int num27 = 25; num27 < Main.maxTilesX - 25; num27++)
            {
                for (int num28 = Main.maxTilesY - 300; num28 < Main.maxTilesY - 100 + WorldGen.genRand.Next(-1, 2); num28++)
                {
                    if (Main.tile[num27, num28].TileType == 57 && Main.tile[num27, num28].HasTile && (!Main.tile[num27 - 1, num28 - 1].HasTile || !Main.tile[num27, num28 - 1].HasTile || !Main.tile[num27 + 1, num28 - 1].HasTile || !Main.tile[num27 - 1, num28].HasTile || !Main.tile[num27 + 1, num28].HasTile || !Main.tile[num27 - 1, num28 + 1].HasTile || !Main.tile[num27, num28 + 1].HasTile || !Main.tile[num27 + 1, num28 + 1].HasTile))
                    {
                        Main.tile[num27, num28].TileType = 633;
                    }
                }
            }
            for (int num29 = 25; num29 < Main.maxTilesX - 25; num29++)
            {
                for (int num30 = Main.maxTilesY - 200; num30 < Main.maxTilesY - 50; num30++)
                {
                    if (Main.tile[num29, num30].TileType == 633 && Main.tile[num29, num30].HasTile && !Main.tile[num29, num30 - 1].HasTile && WorldGen.genRand.NextBool(3))
                    {
                        WorldGen.TryGrowingTreeByType(634, num29, num30);
                    }
                }
            }
        }
    }
    public void GenerateIceGems(GenerationProgress progress)
    {
        progress.Set(1.0);
        for (int i = 0; i < Main.maxTilesX * GemDensity(0.25); i++)
        {
            int num = (!WorldGen.remixWorldGen) ? WorldGen.genRand.Next((int)(Main.worldSurface + Main.rockLayer) / 2, GenVars.lavaLine) : WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
            int num2 = WorldGen.genRand.Next(GenVars.snowMinX[num], GenVars.snowMaxX[num]);
            if (Main.tile[num2, num].HasTile && (Main.tile[num2, num].TileType == 147 || Main.tile[num2, num].TileType == 161 || Main.tile[num2, num].TileType == 162 || Main.tile[num2, num].TileType == 224))
            {
                int num3 = WorldGen.genRand.Next(1, 4);
                int num4 = WorldGen.genRand.Next(1, 4);
                int num5 = WorldGen.genRand.Next(1, 4);
                int num6 = WorldGen.genRand.Next(1, 4);
                int num7 = WorldGen.genRand.Next(12);
                int num8 = 0;
                num8 = (num7 >= 3) ? ((num7 < 6) ? 1 : ((num7 < 8) ? 2 : ((num7 < 10) ? 3 : ((num7 >= 11) ? 5 : 4)))) : 0;
                for (int j = num2 - num3; j < num2 + num4; j++)
                {
                    for (int k = num - num5; k < num + num6; k++)
                    {
                        if (!Main.tile[j, k].HasTile)
                        {
                            WorldGen.PlaceTile(j, k, 178, mute: true, forced: false, -1, num8);
                        }
                    }
                }
            }
        }
    }
    public void GenerateRandomGems(GenerationProgress progress)
    {
        progress.Set(1.0);
        for (int i = 0; i < GemDensity(Main.maxTilesX); i++)
        {
            int num = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
            int num2 = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 300);
            if (!Main.tile[num, num2].HasTile && Main.tile[num, num2].LiquidType != 1 && !Main.wallDungeon[Main.tile[num, num2].WallType] && Main.tile[num, num2].WallType != 27)
            {
                int num3 = WorldGen.genRand.Next(12);
                int num4 = 0;
                num4 = (num3 >= 3) ? ((num3 < 6) ? 1 : ((num3 < 8) ? 2 : ((num3 < 10) ? 3 : ((num3 >= 11) ? 5 : 4)))) : 0;
                WorldGen.PlaceTile(num, num2, 178, mute: true, forced: false, -1, num4);
            }
        }
        for (int j = 0; j < GemDensity(Main.maxTilesX); j++)
        {
            int num5 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
            int num6 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
            if (!Main.tile[num5, num6].HasTile && Main.tile[num5, num6].LiquidType != 1 && (Main.tile[num5, num6].WallType == 216 || Main.tile[num5, num6].WallType == 187))
            {
                int num7 = WorldGen.genRand.Next(1, 4);
                int num8 = WorldGen.genRand.Next(1, 4);
                int num9 = WorldGen.genRand.Next(1, 4);
                int num10 = WorldGen.genRand.Next(1, 4);
                for (int k = num5 - num7; k < num5 + num8; k++)
                {
                    for (int l = num6 - num9; l < num6 + num10; l++)
                    {
                        if (!Main.tile[k, l].HasTile)
                        {
                            WorldGen.PlaceTile(k, l, 178, mute: true, forced: false, -1, 6);
                        }
                    }
                }
            }
        }
    }
    public void GenerateMicroBiomes(GenerationProgress progress, GameConfiguration passConfig)
    {
        progress.Message = Lang.gen[76].Value + "..Dead Man's Chests";
        _ = Main.maxTilesX * Main.maxTilesY / 5040000.0;
        double num = 10.0;
        if (WorldGen.getGoodWorldGen || WorldGen.noTrapsWorldGen)
        {
            num *= 3.0;
        }
        DeadMansChestBiome deadMansChestBiome = GenVars.configuration.CreateBiome<DeadMansChestBiome>();
        List<int> possibleChestsToTrapify = deadMansChestBiome.GetPossibleChestsToTrapify(GenVars.structures);
        int random = passConfig.Get<WorldGenRange>("DeadManChests").GetRandom(WorldGen.genRand);
        int num2 = 0;
        int num3 = 3000;
        while (num2 < random && possibleChestsToTrapify.Count > 0)
        {
            num3--;
            if (num3 <= 0)
            {
                break;
            }
            int num4 = possibleChestsToTrapify[WorldGen.genRand.Next(possibleChestsToTrapify.Count)];
            Point origin = new(Main.chest[num4].x, Main.chest[num4].y);
            deadMansChestBiome.Place(origin, GenVars.structures);
            num2++;
            possibleChestsToTrapify.Remove(num4);
        }
        progress.Message = Lang.gen[76].Value + "..Thin Ice";
        progress.Set(1.0 / num);
        if (!WorldGen.notTheBees || WorldGen.remixWorldGen)
        {
            ThinIceBiome thinIceBiome = GenVars.configuration.CreateBiome<ThinIceBiome>();
            int random2 = passConfig.Get<WorldGenRange>("ThinIcePatchCount").GetRandom(WorldGen.genRand);
            int num5 = 0;
            int num6 = 1000;
            int num7 = 0;
            while (num7 < random2)
            {
                if (thinIceBiome.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface + 20, 50, 200, 50), GenVars.structures))
                {
                    num7++;
                    num5 = 0;
                }
                else
                {
                    num5++;
                    if (num5 > num6)
                    {
                        num7++;
                        num5 = 0;
                    }
                }
            }
        }
        progress.Message = Lang.gen[76].Value + "..Sword Shrines";
        progress.Set(0.1);
        progress.Set(2.0 / num);
        EnchantedSwordBiome enchantedSwordBiome = GenVars.configuration.CreateBiome<EnchantedSwordBiome>();
        int num8 = passConfig.Get<WorldGenRange>("SwordShrineAttempts").GetRandom(WorldGen.genRand);
        double num9 = passConfig.Get<double>("SwordShrinePlacementChance");
        if (WorldGen.tenthAnniversaryWorldGen)
        {
            num8 *= 2;
            num9 /= 2.0;
        }
        Point origin2 = default(Point);
        for (int i = 0; i < num8; i++)
        {
            if ((i == 0 && WorldGen.tenthAnniversaryWorldGen) || !(WorldGen.genRand.NextDouble() > num9))
            {
                int num10 = 0;
                while (num10++ <= Main.maxTilesX)
                {
                    origin2.Y = (int)GenVars.worldSurface + WorldGen.genRand.Next(50, 100);
                    origin2.X = WorldGen.genRand.NextBool(2)
                        ? WorldGen.genRand.Next(50, (int)(Main.maxTilesX * 0.3))
                        : WorldGen.genRand.Next((int)(Main.maxTilesX * 0.7), Main.maxTilesX - 50);
                    if (enchantedSwordBiome.Place(origin2, GenVars.structures))
                    {
                        break;
                    }
                }
            }
        }
        progress.Message = Lang.gen[76].Value + "..Campsites";
        progress.Set(0.2);
        progress.Set(3.0 / num);
        if (!WorldGen.notTheBees || WorldGen.remixWorldGen)
        {
            CampsiteBiome campsiteBiome = GenVars.configuration.CreateBiome<CampsiteBiome>();
            int random3 = passConfig.Get<WorldGenRange>("CampsiteCount").GetRandom(WorldGen.genRand);
            num3 = 1000;
            int num11 = 0;
            while (num11 < random3)
            {
                num3--;
                if (num3 <= 0)
                {
                    break;
                }
                if (campsiteBiome.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface, WorldGen.beachDistance, 200, WorldGen.beachDistance), GenVars.structures))
                {
                    num11++;
                }
            }
        }
        if (LuneWoL.LWoLAdvancedServerSettings.OreDensityCfg.DynamiteVein)
        {
            progress.Message = Lang.gen[76].Value + "..Explosive Traps";
            progress.Set(4.0 / num);
            if (!WorldGen.notTheBees || WorldGen.remixWorldGen)
            {
                MiningExplosivesBiome miningExplosivesBiome = GenVars.configuration.CreateBiome<MiningExplosivesBiome>();
                int num12 = passConfig.Get<WorldGenRange>("ExplosiveTrapCount").GetRandom(WorldGen.genRand);
                if ((WorldGen.getGoodWorldGen || WorldGen.noTrapsWorldGen) && !WorldGen.notTheBees)
                {
                    num12 = (int)(num12 * 1.5);
                }
                num3 = 3000;
                int num13 = 0;
                while (num13 < num12)
                {
                    num3--;
                    if (num3 <= 0)
                    {
                        break;
                    }
                    if (WorldGen.remixWorldGen)
                    {
                        if (miningExplosivesBiome.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface, WorldGen.beachDistance, (int)GenVars.rockLayer, WorldGen.beachDistance), GenVars.structures))
                        {
                            num13++;
                        }
                    }
                    else if (miningExplosivesBiome.Place(WorldGen.RandomWorldPoint((int)GenVars.rockLayer, WorldGen.beachDistance, 200, WorldGen.beachDistance), GenVars.structures))
                    {
                        num13++;
                    }
                }
            }
        }
        progress.Message = Lang.gen[76].Value + "..Living Trees";
        progress.Set(0.3);
        progress.Set(5.0 / num);
        MahoganyTreeBiome mahoganyTreeBiome = GenVars.configuration.CreateBiome<MahoganyTreeBiome>();
        int random4 = passConfig.Get<WorldGenRange>("LivingTreeCount").GetRandom(WorldGen.genRand);
        int num14 = 0;
        int num15 = 0;
        while (num14 < random4 && num15 < 20000)
        {
            if (mahoganyTreeBiome.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface + 50, 50, 500, 50), GenVars.structures))
            {
                num14++;
            }
            num15++;
        }
        progress.Message = Lang.gen[76].Value + "..Long Minecart Tracks";
        progress.Set(0.4);
        progress.Set(6.0 / num);
        progress.Set(7.0 / num);
        TrackGenerator trackGenerator = new();
        int random5 = passConfig.Get<WorldGenRange>("LongTrackCount").GetRandom(WorldGen.genRand);
        WorldGenRange worldGenRange = passConfig.Get<WorldGenRange>("LongTrackLength");
        int maxTilesX = Main.maxTilesX;
        int num16 = 0;
        int num17 = 0;
        while (num17 < random5)
        {
            if (trackGenerator.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface, 10, 200, 10), worldGenRange.ScaledMinimum, worldGenRange.ScaledMaximum))
            {
                num17++;
                num16 = 0;
            }
            else
            {
                num16++;
                if (num16 > maxTilesX)
                {
                    num17++;
                    num16 = 0;
                }
            }
        }
        progress.Message = Lang.gen[76].Value + "..Standard Minecart Tracks";
        progress.Set(8.0 / num);
        random5 = passConfig.Get<WorldGenRange>("StandardTrackCount").GetRandom(WorldGen.genRand);
        worldGenRange = passConfig.Get<WorldGenRange>("StandardTrackLength");
        num16 = 0;
        int num18 = 0;
        while (num18 < random5)
        {
            if (trackGenerator.Place(WorldGen.RandomWorldPoint((int)Main.worldSurface, 10, 200, 10), worldGenRange.ScaledMinimum, worldGenRange.ScaledMaximum))
            {
                num18++;
                num16 = 0;
            }
            else
            {
                num16++;
                if (num16 > maxTilesX)
                {
                    num18++;
                    num16 = 0;
                }
            }
        }
        progress.Message = Lang.gen[76].Value + "..Lava Traps";
        progress.Set(9.0 / num);
        if (!WorldGen.notTheBees)
        {
            double num19 = Main.maxTilesX * 0.02;
            if (WorldGen.noTrapsWorldGen)
            {
                num *= 5.0;
            }
            else if (WorldGen.getGoodWorldGen)
            {
                num *= 2.0;
            }
            for (int j = 0; j < num19; j++)
            {
                for (int k = 0; k < 10150; k++)
                {
                    int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                    int y = WorldGen.genRand.Next(GenVars.lavaLine - 100, Main.maxTilesY - 210);
                    if (WorldGen.placeLavaTrap(x, y))
                    {
                        break;
                    }
                }
            }
        }
        progress.Set(1.0);
    }
}
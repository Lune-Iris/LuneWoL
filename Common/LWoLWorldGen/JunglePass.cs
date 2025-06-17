namespace LuneWoL.Common.LWoLWorldGen;

public class JunglePass : GenPass
{
    private double _worldScale;

    public JunglePass()
        : base("Jungle", 10154.65234375)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = Lang.gen[11].Value;
        _worldScale = Main.maxTilesX / 4200.0 * 1.5;
        double worldScale = _worldScale;
        Point point = CreateStartPoint();
        int x = point.X;
        int y = point.Y;
        Point zero = Point.Zero;
        ApplyRandomMovement(ref x, ref y, 100, 100);
        zero.X += x;
        zero.Y += y;
        PlaceFirstPassMud(x, y, 3);
        PlaceGemsAt(x, y, 63, 2);
        progress.Set(0.15);
        ApplyRandomMovement(ref x, ref y, 250, 150);
        zero.X += x;
        zero.Y += y;
        PlaceFirstPassMud(x, y, 0);
        PlaceGemsAt(x, y, 65, 2);
        progress.Set(0.3);
        int oldX = x;
        int oldY = y;
        ApplyRandomMovement(ref x, ref y, 400, 150);
        zero.X += x;
        zero.Y += y;
        PlaceFirstPassMud(x, y, -3);
        PlaceGemsAt(x, y, 67, 2);
        progress.Set(0.45);
        x = zero.X / 3;
        y = zero.Y / 3;
        int num = _random.Next((int)(400.0 * worldScale), (int)(600.0 * worldScale));
        int num2 = (int)(25.0 * worldScale);
        x = Utils.Clamp(x, GenVars.leftBeachEnd + (num / 2) + num2, GenVars.rightBeachStart - (num / 2) - num2);
        GenVars.mudWall = true;
        WorldGen.TileRunner(x, y, num, 10000, 59, addTile: false, 0.0, -20.0, noYChange: true);
        GenerateTunnelToSurface(x, y);
        GenVars.mudWall = false;
        progress.Set(0.6);
        GenerateHolesInMudWalls();
        GenerateFinishingTouches(progress, oldX, oldY);
    }

    private void PlaceGemsAt(int x, int y, ushort baseGem, int gemVariants)
    {
        for (int i = 0; i < GemDensity(6.0) * _worldScale; i++)
            WorldGen.TileRunner(x + _random.Next(-(int)(125.0 * _worldScale), (int)(125.0 * _worldScale)), y + _random.Next(-(int)(125.0 * _worldScale), (int)(125.0 * _worldScale)), _random.Next(GemSS(3), GemSS(7)), _random.Next(GemSS(3), GemSS(8)), _random.Next(baseGem, baseGem + gemVariants));
    }

    private void PlaceFirstPassMud(int x, int y, int xSpeedScale)
    {
        GenVars.mudWall = true;
        WorldGen.TileRunner(x, y, _random.Next((int)(250.0 * _worldScale), (int)(500.0 * _worldScale)), _random.Next(50, 150), 59, addTile: false, GenVars.dungeonSide * xSpeedScale);
        GenVars.mudWall = false;
    }

    private Point CreateStartPoint() => new(GenVars.jungleOriginX, (int)(Main.maxTilesY + Main.rockLayer) / 2);

    private void ApplyRandomMovement(ref int x, ref int y, int xRange, int yRange)
    {
        x += _random.Next((int)(-xRange * _worldScale), 1 + (int)(xRange * _worldScale));
        y += _random.Next((int)(-yRange * _worldScale), 1 + (int)(yRange * _worldScale));
        y = Utils.Clamp(y, (int)Main.rockLayer, Main.maxTilesY);
    }

    private void GenerateTunnelToSurface(int i, int j)
    {
        double num = _random.Next(5, 11);
        Vector2D vector2D = new()
        {
            X = i,
            Y = j
        };
        Vector2D vector2D2 = new()
        {
            X = _random.Next(-10, 11) * 0.1,
            Y = _random.Next(10, 20) * 0.1
        };
        int num2 = 0;
        bool flag = true;
        while (flag)
        {
            if (vector2D.Y < Main.worldSurface)
            {
                if (WorldGen.drunkWorldGen)
                    flag = false;
                int value = (int)vector2D.X;
                int value2 = (int)vector2D.Y;
                value = Utils.Clamp(value, 10, Main.maxTilesX - 10);
                value2 = Utils.Clamp(value2, 10, Main.maxTilesY - 10);
                if (value2 < 5)
                    value2 = 5;
                if (Main.tile[value, value2].WallType == 0 && !Main.tile[value, value2].HasTile && Main.tile[value, value2 - 3].WallType == 0 && !Main.tile[value, value2 - 3].HasTile && Main.tile[value, value2 - 1].WallType == 0 && !Main.tile[value, value2 - 1].HasTile && Main.tile[value, value2 - 4].WallType == 0 && !Main.tile[value, value2 - 4].HasTile && Main.tile[value, value2 - 2].WallType == 0 && !Main.tile[value, value2 - 2].HasTile && Main.tile[value, value2 - 5].WallType == 0 && !Main.tile[value, value2 - 5].HasTile)
                    flag = false;
            }
            GenVars.JungleX = (int)vector2D.X;
            num += _random.Next(-20, 21) * 0.1;
            if (num < 5.0)
                num = 5.0;
            if (num > 10.0)
                num = 10.0;
            int value3 = (int)(vector2D.X - (num * 0.5));
            int value4 = (int)(vector2D.X + (num * 0.5));
            int value5 = (int)(vector2D.Y - (num * 0.5));
            int value6 = (int)(vector2D.Y + (num * 0.5));
            int num3 = Utils.Clamp(value3, 10, Main.maxTilesX - 10);
            value4 = Utils.Clamp(value4, 10, Main.maxTilesX - 10);
            value5 = Utils.Clamp(value5, 10, Main.maxTilesY - 10);
            value6 = Utils.Clamp(value6, 10, Main.maxTilesY - 10);
            for (int k = num3; k < value4; k++)
                for (int l = value5; l < value6; l++)
                    if (Math.Abs(k - vector2D.X) + Math.Abs(l - vector2D.Y) < num * 0.5 * (1.0 + (_random.Next(-10, 11) * 0.015)))
                        WorldGen.KillTile(k, l);
            num2++;
            if (num2 > 10 && _random.Next(50) < num2)
            {
                num2 = 0;
                int num4 = -2;
                if (_random.NextBool(2))
                    num4 = 2;
                WorldGen.TileRunner((int)vector2D.X, (int)vector2D.Y, _random.Next(3, 20), _random.Next(10, 100), -1, addTile: false, num4);
            }
            vector2D += vector2D2;
            vector2D2.Y += _random.Next(-10, 11) * 0.01;
            if (vector2D2.Y > 0.0)
                vector2D2.Y = 0.0;
            if (vector2D2.Y < -2.0)
                vector2D2.Y = -2.0;
            vector2D2.X += _random.Next(-10, 11) * 0.1;
            if (vector2D.X < i - 200)
                vector2D2.X += _random.Next(5, 21) * 0.1;
            if (vector2D.X > i + 200)
                vector2D2.X -= _random.Next(5, 21) * 0.1;
            if (vector2D2.X > 1.5)
                vector2D2.X = 1.5;
            if (vector2D2.X < -1.5)
                vector2D2.X = -1.5;
        }
    }

    private void GenerateHolesInMudWalls()
    {
        for (int i = 0; i < Main.maxTilesX / 4; i++)
        {
            int num = _random.Next(20, Main.maxTilesX - 20);
            int num2 = _random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
            while (Main.tile[num, num2].WallType != 64 && Main.tile[num, num2].WallType != 15)
            {
                num = _random.Next(20, Main.maxTilesX - 20);
                num2 = _random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
            }
            WorldGen.MudWallRunner(num, num2);
        }
    }

    private void GenerateFinishingTouches(GenerationProgress progress, int oldX, int oldY)
    {
        int num = oldX;
        int num2 = oldY;
        double worldScale = _worldScale;
        for (int i = 0; i <= 20.0 * worldScale; i++)
        {
            progress.Set((60.0 + (i / worldScale)) * 0.01);
            num += _random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
            num2 += _random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
            WorldGen.TileRunner(num, num2, _random.Next(40, 100), _random.Next(300, 500), 59);
        }
        for (int j = 0; j <= 10.0 * worldScale; j++)
        {
            progress.Set((80.0 + (j / worldScale * 2.0)) * 0.01);
            num = oldX + _random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
            num2 = oldY + _random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
            while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].TileType != 59)
            {
                num = oldX + _random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
                num2 = oldY + _random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
            }
            for (int k = 0; k < 8.0 * worldScale; k++)
            {
                num += _random.Next(-30, 31);
                num2 += _random.Next(-30, 31);
                int type = -1;
                if (_random.NextBool(7))
                    type = -2;
                WorldGen.TileRunner(num, num2, _random.Next(10, 20), _random.Next(30, 70), type);
            }
        }
        for (int l = 0; l <= 300.0 * worldScale; l++)
        {
            num = oldX + _random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
            num2 = oldY + _random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
            while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].TileType != 59)
            {
                num = oldX + _random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
                num2 = oldY + _random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
            }
            WorldGen.TileRunner(num, num2, _random.Next(4, 10), _random.Next(5, 30), 1);
            if (_random.NextBool(4))
            {
                int type2 = _random.Next(63, 69);
                WorldGen.TileRunner(num + _random.Next(-1, 2), num2 + _random.Next(-1, 2), _random.Next(3, 7), _random.Next(4, 8), type2);
            }
        }
    }
}

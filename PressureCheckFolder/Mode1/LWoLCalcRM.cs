namespace LuneWoL.PressureCheckFolder.Mode1;

public partial class PressureModeOne : ModPlayer
{
    public float rD = 0f, lDD, tDC, tD;
    public int mD, pDTA, rDD;

    //TODO: add difficulty options and mod compat such as calamity < too lazy ill just add advanced options lmfao

    public int MD() // Max Depth
    {
        mD = Player.arcticDivingGear ? 500 :
             Player.accDivingHelm && Player.accFlipper ? 450 :
             Player.accDivingHelm ? 350 :
             Player.gills ? 250 : 200;
        return mD;
    }

    public float RD() // Reduced Depth
    {
        rD = 1f -
            (Player.arcticDivingGear ? 0.15f : 0f) -
            (Player.accDivingHelm && Player.accFlipper ? 0.1f : 0f) -
            (Player.accDivingHelm ? 0.1f : 0f) -
            (Player.gills ? 0.05f : 0f);

        if (rD <= 0.25f)
        {
            rD = 0.25f;
        }
        return rD;
    }

    public int RDD() => rDD = (int)((LP.position.Y - ModeOne.EntryPoint.Y) * ModeOne.rD) / 16; // Reduced Depth Difference

    public float TDC() => tDC = Math.Clamp(rDD, 0, ModeOne.mD); // Tile Difference Clamped

    public float TD() => tD = (int)(LP.position.Y - ModeOne.EntryPoint.Y) / 16; // Tile Difference

    public int PDTA() => pDTA = ModeOne.rDD - ModeOne.mD; // Pressure Dammage To Apply

    public float LDD() => lDD = ModeOne.tDC / ModeOne.mD; // Light Depth Difference
}

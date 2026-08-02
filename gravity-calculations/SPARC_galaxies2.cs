using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using astronomy_tools;
using DXF_NS;
using galaxy_tools;
using global_helpers;

namespace GravityCalculations
{
    internal partial class SPARC_galaxies
    {
        public static void DrawCurves_DDO064_Burkert(string sBasePath)
        {
            // UGC 05272

            // Constellation: Leo
            // Location:
            // Distance: 3.800-7.110 Mpc
            // Redshift (v. Heliocentric): 513 ± 2 km/s
            // Redshift (v. Galactocentric): 474 ± 3 km/s
            // Redshift (v. Local Group): 453 ± 4 km/s
            // Redshift (v. 3K CMB): 784 ± 19 km/s
            // (M/L)d: .49

            // Fit: 4.5

            GalaxyParams cDDO064 = new GalaxyParams()
            {
                Name = "DDO064_Burkert",
                Hf_km_s_Mpc = 4,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .12,
                R_DataMax_kpc = 3.03,
                R_Max_kpc = 3.48899, // 3 + this: (25.115 px / 51.361 px) * 1 kpc = 0.48899
                V_Max_km_s = 60,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 116.633
            };

            cDDO064.Path = Path.Combine(sBasePath, cDDO064.Name);
            cDDO064.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cDDO064);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 3.03;
            int nIndex2 = (int)((1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.8, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO064, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 6.90E+38, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_DDO154_coreNFW_flat(string sBasePath)
        {
            // NGC 4789A

            // Constellation: Coma Berenices
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.040 Mpc
            // Redshift (v. Heliocentric): 364 ± 1 km/s
            // Redshift (v. Galactocentric): 372 ± 1 km/s
            // Redshift (v. Local Group): 344 ± 1 km/s
            // Redshift (v. 3K CMB): 639 ± 19 km/s
            // (M/L)d: .29

            // Fit: 4.5

            GalaxyParams cDDO154 = new GalaxyParams()
            {
                Name = "DDO154_coreNFW_flat",
                Hf_km_s_Mpc = 3.8,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .52,
                R_DataMax_kpc = 5.8,
                R_Max_kpc = 6.68350, // 6 + this: (18.326 px / 53.624 px) * 2 kpc = 0.68350
                V_Max_km_s = 60,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cDDO154.Path = Path.Combine(sBasePath, cDDO154.Name);
            cDDO154.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cDDO154);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 5.97;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((.7 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 3, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 6.2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO154, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 4.75E+38, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_DDO168_Lucky13_LCDM(string sBasePath)
        {
            // UGC 8320

            // Constellation: Canes Venatici
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.250 Mpc
            // Redshift (v. Heliocentric): 192 ± 1 km/s
            // Redshift (v. Galactocentric): 269 ± 3 km/s
            // Redshift (v. Local Group): 270 ± 5 km/s
            // Redshift (v. 3K CMB): 380 ± 13 km/s
            // (M/L)d: .55

            // Fit: 3.5

            GalaxyParams cDDO168 = new GalaxyParams()
            {
                Name = "DDO168_Lucky13_LCDM",
                Hf_km_s_Mpc = 2.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .43,
                R_DataMax_kpc = 4.07,
                R_Max_kpc = 4.69761, // 4 + this: (26.611 px / 38.146 px) * 1 kpc = 0.69761
                V_Max_km_s = 60,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cDDO168.Path = Path.Combine(sBasePath, cDDO168.Name);
            cDDO168.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cDDO168);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 4.02;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .5, 0, 0);
            //LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 12, .5);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO168, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 7.69E+38, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_ESO444_G084_NFW_LCDM(string sBasePath)
        {
            // Constellation: Centaurus
            // Location: Abell 3558
            // Distance: 4.530 Mpc
            // Redshift (v. Heliocentric): 587 ± 3 km/s
            // Redshift (v. Galactocentric): 460 ± 6 km/s
            // Redshift (v. Local Group): 380 ± 13 km/s
            // Redshift (v. 3K CMB): 870 ± 20 km/s
            // (M/L)d: .52

            // Fit: 4.5

            GalaxyParams cESO444_G084 = new GalaxyParams()
            {
                Name = "ESO444_G084_NFW_LCDM",
                Hf_km_s_Mpc = 4.0,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .29,
                R_DataMax_kpc = 4.42,
                R_Max_kpc = 5.10976, // 5 + this: (3.849 px / 35.069 px) * 1 kpc = 0.10976
                V_Max_km_s = 60,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 117.132
            };

            cESO444_G084.Path = Path.Combine(sBasePath, cESO444_G084.Name);
            cESO444_G084.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cESO444_G084);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 4.43;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cESO444_G084, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 4.49E+38, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_IC2574_DC14_flat(string sBasePath)
        {
            // Coddington's Nebula, DDO 081

            // Constellation: Ursa Major
            // Location: outlying member of the M81 Group
            // Distance: 3.890-3.940 Mpc
            // Redshift (v. Heliocentric): 57 ± 2 km/s
            // Redshift (v. Galactocentric): 164 ± 5 km/s
            // Redshift (v. Local Group): 197 ± 9 km/s
            // Redshift (v. 3K CMB): 148 ± 7 km/s
            // (M/L)d: .44

            // Fit: 5

            GalaxyParams cIC2574 = new GalaxyParams()
            {
                Name = "IC2574_DC14_flat",
                Hf_km_s_Mpc = 1.8,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .95,
                R_DataMax_kpc = 10.2,
                R_Max_kpc = 11.78005, // 10 + this: (27.079 px / 30.425 px) * 2 kpc = 1.78005
                V_Max_km_s = 80,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 117.344
            };

            cIC2574.Path = Path.Combine(sBasePath, cIC2574.Name);
            cIC2574.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cIC2574);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 10.5;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((3.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .25, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 11, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 18, 7);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[2];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.85, 0);
            acNudges2[1] = new FullParabolicNudge(0, nIndex2b, 0, -.2, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 15, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 27, 10);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cIC2574, null, cDisk, cGas);
            // dTotalMass_kg = 3.24E+39, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC2976_Burkert(string sBasePath)
        {
            // Constellation: Ursa Major
            // Location: M81 Group
            // Distance: 3.520-3.630 Mpc
            // Redshift (v. Heliocentric): 3 ± 2 km/s
            // Redshift (v. Galactocentric): 105 ± 5 km/s
            // Redshift (v. Local Group): 138 ± 8 km/s
            // Redshift (v. 3K CMB): 90 ± 7 km/s
            // (M/L)d: .48

            // Fit: 5

            GalaxyParams cNGC2976 = new GalaxyParams()
            {
                Name = "NGC2976_Burkert",
                Hf_km_s_Mpc = 1.9,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .13,
                R_DataMax_kpc = 2.25,
                R_Max_kpc = 2.59989, // 2.5 + this: (6.885 px / 34.463 px) * .5 kpc = 0.09989 
                V_Max_km_s = 100,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 122.682
            };

            cNGC2976.Path = Path.Combine(sBasePath, cNGC2976.Name);
            cNGC2976.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2976);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 2.25;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[1];
            //acNudges[0] = new ParabolicNudge(0, nIndex2, -.7, 0, 0);
            acNudges[0] = new LinearNudge(0, nIndex2, -.6, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.6, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 54, 1.7);

            int nIndex1 = (int)((0 / dR2_kpc) * nNumDiskBodyLayers);
            nIndex2 = (int)((1.1 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex2b = (int)((.6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[2];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.8, 0);
            acNudges2[1] = new FullParabolicNudge(nIndex1, nIndex2b, 0, -.4, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 30, 2);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2976, null, cDisk, cGas);
            // dTotalMass_kg = 2.30E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC3109_NFW_LCDM(string sBasePath)
        {
            // Constellation: Hydra
            // Location: Local Group
            // Distance: 1.333 Mpc
            // Redshift (v. Heliocentric): 403 ± 1 km/s
            // Redshift (v. Galactocentric): 193 ± 8 km/s
            // Redshift (v. Local Group): 109 ± 18 km/s
            // Redshift (v. 3K CMB): 738 ± 24 km/s
            // (M/L)d: .92

            // Fit: 5

            GalaxyParams cNGC3109 = new GalaxyParams()
            {
                Name = "NGC3109_NFW_LCDM",
                Hf_km_s_Mpc = 3.6,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .34,
                R_DataMax_kpc = 7.16,
                R_Max_kpc = 8.28896, // 8 + this: (6.247 px / 43.238 px) * 2 kpc = 0.28896
                V_Max_km_s = 70, // 60 + this: (16.869 px / 33.738 px) * 20 km/s = 0.28896
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 118.084
            };

            cNGC3109.Path = Path.Combine(sBasePath, cNGC3109.Name);
            cNGC3109.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC3109);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 7, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC3109, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.35E+39, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC4214_NFW_LCDM(string sBasePath)
        {
            // Constellation: Canes Venatici
            // Location: M81 Group
            // Distance: 2.700-2.930 Mpc
            // Redshift (v. Heliocentric): 291 ± 1 km/s
            // Redshift (v. Galactocentric): 312 ± 1 km/s
            // Redshift (v. Local Group): 295 ± 1 km/s
            // Redshift (v. 3K CMB): 550 ± 18 km/s
            // (M/L)d: .54

            // Fit: 2.5

            GalaxyParams cNGC4214 = new GalaxyParams()
            {
                Name = "NGC4214_NFW_LCDM",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .24,
                R_DataMax_kpc = 5.63,
                R_Max_kpc = 6.49287, // 6 + this: (13.603 px / 55.199 px) * 2 kpc = 0.49287
                V_Max_km_s = 90, //80,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.959
            };

            cNGC4214.Path = Path.Combine(sBasePath, cNGC4214.Name);
            cNGC4214.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC4214);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 5.63;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((3 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((2.2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.05, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nNumDiskBodyLayers = 3000;
            nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, -.5, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC4214, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.44E+39, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC6789_ML1p0(string sBasePath)
        {
            // Constellation: Draco
            // Location: Local Void
            // Distance: 3.600 Mpc
            // Redshift (v. Heliocentric): -151 ± 2 km/s
            // Redshift (v. Galactocentric): 66 ± 9 km/s
            // Redshift (v. Local Group): 134 ± 17 km/s
            // Redshift (v. 3K CMB): -275 ± 9 km/s
            // (M/L)d coreNFW_LCDM: .62
            // (M/L)d Lucky13_LCDM: .69
            // (M/L)d DC14_LCDM: 1.20
            // (M/L)d: 1.0

            // Use DC14_LCDM error bars!

            // Fit: 3.5.

            GalaxyParams cNGC6789 = new GalaxyParams()
            {
                Name = "NGC6789_ML1p0",
                Hf_km_s_Mpc = 3.0,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .11,
                R_DataMax_kpc = .73,
                R_Max_kpc = .85572, // .8 + this: (11.669 px / 41.883 px) * .2 kpc = 0.05572
                V_Max_km_s = 60,
                R_LongTick_kpc = .2,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 120.326
            };

            cNGC6789.Path = Path.Combine(sBasePath, cNGC6789.Name);
            cNGC6789.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC6789);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            // The disk density profile can be derived via the velocity data.
            double dR2_m = cDisk.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cDiskBody = new ShellBody(0, dR2_m, 1, 1000);
            cDiskBody.SetDensityCurveFromVelocityArray(cDisk.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cDiskBody.CalculateBodyMass();
            cDisk.Body = cDiskBody;

            // The gas density profile can be derived via the velocity data.
            dR2_m = cGas.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cGasBody = new ShellBody(0, dR2_m, 1, 1000);
            cGasBody.SetDensityCurveFromVelocityArray(cGas.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dGasMass_kg = cGasBody.CalculateBodyMass();
            cGas.Body = cGasBody;

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC6789, cDisk, null, null, null, null, false, cGas);
            // dTotalMass_kg = 2.05E+38, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC08490_DC14_LCDM(string sBasePath)
        {
            // NGC 5204

            // Constellation: Ursa Major
            // Location: M101 Group
            // Distance: 4.790-5.550 Mpc
            // Redshift (v. Heliocentric): 201 ± 1 km/s
            // Redshift (v. Galactocentric): 318 ± 5 km/s
            // Redshift (v. Local Group): 339 ± 8 km/s
            // Redshift (v. 3K CMB): 322 ± 8 km/s
            // (M/L)d: .96

            // Fit: 4.5

            GalaxyParams cUGC08490 = new GalaxyParams()
            {
                Name = "UGC08490_DC14_LCDM",
                Hf_km_s_Mpc = 2.2,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .5,
                R_DataMax_kpc = 12.5,
                R_Max_kpc = 14.66512, // 10 + this: (57.005 px / 61.097 px) * 5 kpc = 4.66512
                V_Max_km_s = 90,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cUGC08490.Path = Path.Combine(sBasePath, cUGC08490.Name);
            cUGC08490.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC08490);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 12;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((9 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 11, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC08490, null, cDisk, cGas);
            // dTotalMass_kg = 4.95E+39, verified 6/29/25

            int n1 = 1;
        }
    }
}

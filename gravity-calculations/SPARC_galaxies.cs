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
        // From Catalog of Dark Matter Halo Models for SPARC Galaxies.pdf:
        // We find that cored halo models, such as the DC14 and Burkert profiles,
        // generally provide better fits to rotation curves than the cuspy NFW profile.

        // Rotation data are from:
        // http://astroweb.case.edu/SPARC/ and
        // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "SPARC MASS MODELS FOR 175 DISK GALAXIES.pdf"

        // M/L ratios for SPARC galaxies can be adjusted here:
        // https://guustaaf.com/galaxies/?g=NGC6789

        public static void DrawCurves_CamB_NFW_flat(string sBasePath)
        {
            // Camelopardalis B

            // Constellation: Camelopardalis
            // Location:
            // Distance: 3.500 Mpc
            // Redshift (v. Heliocentric): 77 ± 0 km/s
            // Redshift (v. Galactocentric): 206 ± 5 km/s
            // Redshift (v. Local Group): 266 ± 11 km/s
            // Redshift (v. 3K CMB): 23 ± 4 km/s
            // (M/L)d: .37

            // Fit: 1

            GalaxyParams cCamB = new GalaxyParams()
            {
                Name = "CamB_NFW_flat",
                Hf_km_s_Mpc = .07,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .16,
                R_DataMax_kpc = 1.68,
                R_Max_kpc = 1.94998, // 1.5 + this: (41.352 px / 45.949 px) * .5 kpc = 0.44998 
                V_Max_km_s = 25,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 5,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cCamB.Path = Path.Combine(sBasePath, cCamB.Name);
            cCamB.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cCamB);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1.7, 0, .2, 0, DensityDistribution.HalfParabola_WaterSlide, null, 8.2, 1.15);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cCamB, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 7.12E+37, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_DDO064_Einasto_LCDM(string sBasePath)
        {
            // UGC 05272

            // Constellation: Leo
            // Location: (near UGC 05209)
            // Distance: 3.800-7.110 Mpc
            // Redshift (v. Heliocentric): 513 ± 2 km/s
            // Redshift (v. Galactocentric): 474 ± 3 km/s
            // Redshift (v. Local Group): 453 ± 4 km/s
            // Redshift (v. 3K CMB): 784 ± 19 km/s
            // (M/L)d: .50

            // Fit: 4
            // The fit is close to the MOND fit in "Testing galaxy formation in LSB galaxies.pdf".

            GalaxyParams cDDO064 = new GalaxyParams()
            {
                Name = "DDO064_Einasto_LCDM",
                Hf_km_s_Mpc = 7.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .10,
                R_DataMax_kpc = 2.41,
                R_Max_kpc = 2.77985, // 2.5 + this: (18.04 px / 32.231 px) * .5 kpc = 0.27985
                V_Max_km_s = 60,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 116.632
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
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 3.03;
            int nIndex2 = (int)((.7 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.8, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4.5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO064, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 5.04E+38

            int n1 = 1;
        }

        public static void DrawCurves_DDO154_Burkert(string sBasePath)
        {
            // NGC 4789A

            // Constellation: Coma Berenices
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.040 Mpc
            // Redshift (v. Heliocentric): 364 ± 1 km/s
            // Redshift (v. Galactocentric): 372 ± 1 km/s
            // Redshift (v. Local Group): 344 ± 1 km/s
            // Redshift (v. 3K CMB): 639 ± 19 km/s
            // (M/L)d: .43

            // Fit: 4

            GalaxyParams cDDO154 = new GalaxyParams()
            {
                Name = "DDO154_Burkert",
                Hf_km_s_Mpc = 3.2,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .53,
                R_DataMax_kpc = 5.97,
                R_Max_kpc = 6.88274, // 6 + this: (22.983 px / 52.072 px) * 2 kpc = 0.88274
                V_Max_km_s = 50,
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
            int nIndex2 = (int)((.8 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 3, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 6.5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO154, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 5.23E+38

            int n1 = 1;
        }

        public static void DrawCurves_DDO168_Burkert(string sBasePath)
        {
            // UGC 8320

            // Constellation: Canes Venatici
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.250 Mpc
            // Redshift (v. Heliocentric): 192 ± 1 km/s
            // Redshift (v. Galactocentric): 269 ± 3 km/s
            // Redshift (v. Local Group): 270 ± 5 km/s
            // Redshift (v. 3K CMB): 380 ± 13 km/s
            // (M/L)d: .45

            // Fit: 3.5

            GalaxyParams cDDO168 = new GalaxyParams()
            {
                Name = "DDO168_Burkert",
                Hf_km_s_Mpc = 4,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .43,
                R_DataMax_kpc = 4.02,
                R_Max_kpc = 4.63358, // 4 + this: (24.503 px / 38.674 px) * 1 kpc = 0.63358
                V_Max_km_s = 70,
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
            // dTotalMass_kg = 7.22E+38

            int n1 = 1;
        }

        public static void DrawCurves_ESO444_G084_Burkert(string sBasePath)
        {
            // Constellation: Centaurus
            // Location: Abell 3558
            // Distance: 4.530 Mpc
            // Redshift (v. Heliocentric): 587 ± 3 km/s
            // Redshift (v. Galactocentric): 460 ± 6 km/s
            // Redshift (v. Local Group): 380 ± 13 km/s
            // Redshift (v. 3K CMB): 870 ± 20 km/s
            // (M/L)d: .54

            // Fit: 5

            GalaxyParams cESO444_G084 = new GalaxyParams()
            {
                Name = "ESO444_G084_Burkert",
                Hf_km_s_Mpc = 10,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .29,
                R_DataMax_kpc = 4.43,
                R_Max_kpc = 5.12794, // 5 + this: (4.471 px / 34.945 px) * 1 kpc = 0.12794
                V_Max_km_s = 70,
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
            // dTotalMass_kg = 4.53E+38

            int n1 = 1;
        }

        public static void DrawCurves_IC2574_Burkert(string sBasePath)
        {
            // Coddington's Nebula, DDO 081

            // Constellation: Ursa Major
            // Location: outlying member of the M81 Group
            // Distance: 3.890-3.940 Mpc
            // Redshift (v. Heliocentric): 57 ± 2 km/s
            // Redshift (v. Galactocentric): 164 ± 5 km/s
            // Redshift (v. Local Group): 197 ± 9 km/s
            // Redshift (v. 3K CMB): 148 ± 7 km/s
            // (M/L)d: .69

            // Fit: 5

            GalaxyParams cIC2574 = new GalaxyParams()
            {
                Name = "IC2574_Burkert",
                Hf_km_s_Mpc = 1.2,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .95,
                R_DataMax_kpc = 10.5,
                R_Max_kpc = 12.11833, // 10 + this: (31.325 px / 73.938 px) * 5 kpc = 2.11833
                V_Max_km_s = 80,
                R_LongTick_kpc = 5,
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
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 12, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 22, 7);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[2];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.85, 0);
            acNudges2[1] = new FullParabolicNudge(0, nIndex2b, 0, -.2, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 15, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 27, 10);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cIC2574, null, cDisk, cGas);
            // dTotalMass_kg = 3.74E+39

            int n1 = 1;
        }

        public static void DrawCurves_NGC0055_Einasto_flat(string sBasePath)
        {
            // Along with its neighbor NGC 300, it is one of the closest galaxies to the Local Group,
            // probably lying between the Milky Way and the Sculptor Group.

            // Constellation: Sculptor
            // Location: foreground of the Sculptor Group
            // Distance: 1.850-2.340 Mpc
            // Redshift (v. Heliocentric): 131 ± 2 km/s
            // Redshift (v. Galactocentric): 100 ± 2 km/s
            // Redshift (v. Local Group): 113 ± 2 km/s
            // Redshift (v. 3K CMB): -115 ± 17 km/s
            // (M/L)d: .49

            // Fit: 5

            GalaxyParams cNGC0055 = new GalaxyParams()
            {
                Name = "NGC0055_Einasto_flat",
                Hf_km_s_Mpc = 1.3,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.3,
                R_DataMax_kpc = 13.36,
                R_Max_kpc = 15.46979, // 15 + this: (5.442 px / 57.92 px) * 5 kpc = 0.46979
                V_Max_km_s = 100,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 124.989
            };

            cNGC0055.Path = Path.Combine(sBasePath, cNGC0055.Name);
            cNGC0055.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0055);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 13.36;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 18, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 31, 12);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0055, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 8.97E+39, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC0247_coreNFW_LCDM(string sBasePath)
        {
            // Constellation: Sculptor
            // Location: Sculptor Group.
            // Distance: 3.270-3.670 Mpc
            // Redshift (v. Heliocentric): 156 ± 2 km/s
            // Redshift (v. Galactocentric): 172 ± 2 km/s
            // Redshift (v. Local Group): 211 ± 4 km/s
            // Redshift (v. 3K CMB): -143 ± 21 km/s
            // (M/L)d: 1.52

            // Fit: 3.5

            GalaxyParams cNGC0247 = new GalaxyParams()
            {
                Name = "NGC0247_coreNFW_LCDM",
                Hf_km_s_Mpc = 1, //.5,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.2,
                R_DataMax_kpc = 14.5,
                R_Max_kpc = 17.12342, // 15 + this: (22.222 px / 52.326 px) * 5 kpc = 2.12342
                V_Max_km_s = 120,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.959
            };

            cNGC0247.Path = Path.Combine(sBasePath, cNGC0247.Name);
            cNGC0247.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0247);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 14.5;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.58, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14.3, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0247, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 2.27E+40

            int n1 = 1;
        }

        public static void DrawCurves_NGC0247_DC14_flat(string sBasePath)
        {
            // Constellation: Sculptor
            // Location: Sculptor Group.
            // Distance: 3.270-3.670 Mpc
            // Redshift (v. Heliocentric): 156 ± 2 km/s
            // Redshift (v. Galactocentric): 172 ± 2 km/s
            // Redshift (v. Local Group): 211 ± 4 km/s
            // Redshift (v. 3K CMB): -143 ± 21 km/s
            // (M/L)d: .50

            // Fit: 3

            GalaxyParams cNGC0247 = new GalaxyParams()
            {
                Name = "NGC0247_DC14_flat",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.2,
                R_DataMax_kpc = 14.5,
                R_Max_kpc = 16.85256, // 15 + this: (19.699 px / 53.167 px) * 5 kpc = 1.85256
                V_Max_km_s = 120,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.958
            };

            cNGC0247.Path = Path.Combine(sBasePath, cNGC0247.Name);
            cNGC0247.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0247);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 14.5;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.58, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14.3, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0247, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.04E+40

            int n1 = 1;
        }

        public static void DrawCurves_NGC0247_ML1p0(string sBasePath)
        {
            // Constellation: Sculptor
            // Location: Sculptor Group.
            // Distance: 3.270-3.670 Mpc
            // Redshift (v. Heliocentric): 156 ± 2 km/s
            // Redshift (v. Galactocentric): 172 ± 2 km/s
            // Redshift (v. Local Group): 211 ± 4 km/s
            // Redshift (v. 3K CMB): -143 ± 21 km/s
            // (M/L)d DC14_flat: .50
            // (M/L)d coreNFW_LCDM: 1.52
            // (M/L)d: 1.0

            // Use coreNFW_LCDM error bars!

            // Fit: 3.5

            GalaxyParams cNGC0247 = new GalaxyParams()
            {
                Name = "NGC0247_ML1p0",
                Hf_km_s_Mpc = 1.5,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.2,
                R_DataMax_kpc = 14.5,
                R_Max_kpc = 17.12342, // 15 + this: (22.222 px / 52.326 px) * 5 kpc = 2.12342
                V_Max_km_s = 120,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.959
            };

            cNGC0247.Path = Path.Combine(sBasePath, cNGC0247.Name);
            cNGC0247.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0247);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("observed", "observed.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 14.5;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.58, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14.3, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0247, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.62E+40, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC0247_NFW_LCDM(string sBasePath)
        {
            // Constellation: Sculptor
            // Location: Sculptor Group.
            // Distance: 3.270-3.670 Mpc
            // Redshift (v. Heliocentric): 156 ± 2 km/s
            // Redshift (v. Galactocentric): 172 ± 2 km/s
            // Redshift (v. Local Group): 211 ± 4 km/s
            // Redshift (v. 3K CMB): -143 ± 21 km/s
            // (M/L)d: .66

            // Fit: 3

            GalaxyParams cNGC0247 = new GalaxyParams()
            {
                Name = "NGC0247_NFW_LCDM",
                Hf_km_s_Mpc = 2.5,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.2,
                R_DataMax_kpc = 14.5,
                R_Max_kpc = 16.92322, // 15 + this: (20.365 px / 52.945 px) * 5 kpc = 1.92322
                V_Max_km_s = 120,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.959
            };

            cNGC0247.Path = Path.Combine(sBasePath, cNGC0247.Name);
            cNGC0247.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0247);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 14.5;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.58, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14.3, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0247, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.23E+40

            int n1 = 1;
        }

        public static void DrawCurves_NGC0300_Lucky13_LCDM(string sBasePath)
        {
            // Sculptor Pinwheel Galaxy

            // Constellation: Sculptor
            // Location: foreground of the Sculptor Group
            // Distance: 1.790-2.170 Mpc
            // Redshift (v. Heliocentric): 144 ± 1 km/s
            // Redshift (v. Galactocentric): 101 ± 2 km/s
            // Redshift (v. Local Group): 114 ± 2 km/s
            // Redshift (v. 3K CMB): -91 ± 16 km/s
            // (M/L)d: .73

            // Fit: 5

            GalaxyParams cNGC0300 = new GalaxyParams()
            {
                Name = "NGC0300_Lucky13_LCDM",
                Hf_km_s_Mpc = .9,
                R_Increment_kpc = .05,
                R_DataMin_kpc = 1.03,
                R_DataMax_kpc = 12.04,
                R_Max_kpc = 13.93490, // 10 + this: (50.602 px / 64.299 px) * 5 kpc = 3.93490
                V_Max_km_s = 90,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.958
            };

            cNGC0300.Path = Path.Combine(sBasePath, cNGC0300.Name);
            cNGC0300.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC0300);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 12.04;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 3, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 11, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 44, 4);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[2];
            acNudges2[0] = new LinearNudge(0, nIndex2, -1, 0);
            acNudges2[1] = new FullParabolicNudge(0, nIndex2b, 0, -.5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 11, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC0300, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 6.21E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC2366_Burkert(string sBasePath)
        {
            // DDO 042

            // Constellation: Camelopardalis
            // Location: outlying member of the M81 Group
            // Distance: 3.200-3.340 Mpc
            // Redshift (v. Heliocentric): 103 ± 1 km/s
            // Redshift (v. Galactocentric): 213 ± 4 km/s
            // Redshift (v. Local Group): 258 ± 9 km/s
            // Redshift (v. 3K CMB): 134 ± 2 km/s
            // (M/L)d: .38

            // Fit: 2

            GalaxyParams cNGC2366 = new GalaxyParams()
            {
                Name = "NGC2366_Burkert",
                Hf_km_s_Mpc = 1.3,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .16,
                R_DataMax_kpc = 6.02,
                R_Max_kpc = 6.96432, // 6 + this: (24.813 px / 51.462 px) * 2 kpc = 0.96432
                V_Max_km_s = 60,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cNGC2366.Path = Path.Combine(sBasePath, cNGC2366.Name);
            cNGC2366.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2366);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 6.02;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((3 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 14, 1);

            nNumDiskBodyLayers = 4000;
            nIndex2 = (int)((1.8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2366, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.48E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void NGC2403_combine_inputs(string sBasePath)
        {
            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403_Burkert",
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 19.6
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput[] acInputs = new GalaxyRotationInput[2];
            acInputs[0] = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            acInputs[1] = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            GalacticRotationCurveBuilder.CombineInputs(cNGC2403, acInputs, "disk-and-gas");
        }

        public static void DrawCurves_NGC2403_Burkert(string sBasePath)
        {
            // Fireworks Galaxy

            // Constellation: Camelopardalis
            // Location: outlying member of the M81 Group
            // Distance: 3.010-3.930 Mpc
            // Redshift (v. Heliocentric): 133 ± 0 km/s
            // Redshift (v. Galactocentric): 230 ± 4 km/s
            // Redshift (v. Local Group): 270 ± 8 km/s
            // Redshift (v. 3K CMB): 182 ± 3 km/s
            // (M/L)d: .90

            // Fit: 4

            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403_Burkert",
                Hf_km_s_Mpc = 3.1,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 19.6,
                R_Max_kpc = 22.67653, // 20 + this: (21.151 px / 39.512 px) * 5 kpc = 2.67653
                V_Max_km_s = 160, // 150 + this: (7.581 px / 37.906 px) * 50 kpc = 10
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 121.298
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);
            cNGC2403.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 19.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((11 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.4, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.55, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 98, 2.5);

            nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 20, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 45, 12);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, null, cDisk, cGas);
            // dTotalMass_kg = 2.52E+40, verified 6/29/26

            int n1 = 1;
        }

        public static void DrawCurves_NGC2403_Burkert_test1(string sBasePath)
        {
            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403_Burkert_test1",
                Hf_km_s_Mpc = 3.1,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 19.6,
                R_Max_kpc = 22.67653, // 20 + this: (21.151 px / 39.512 px) * 5 kpc = 2.67653
                V_Max_km_s = 160, // 150 + this: (7.581 px / 37.906 px) * 50 kpc = 10
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 121.298
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);
            cNGC2403.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

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

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, cDisk, null, null, null, null, false, cGas);
            // dTotalMass_kg = 3.30E+40;

            int n1 = 1;
        }

        public static void DrawCurves_NGC2403_Burkert_test2(string sBasePath)
        {
            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403_Burkert_test2",
                Hf_km_s_Mpc = 3.1,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 19.6,
                R_Max_kpc = 22.67653, // 20 + this: (21.151 px / 39.512 px) * 5 kpc = 2.67653
                V_Max_km_s = 160, // 150 + this: (7.581 px / 37.906 px) * 50 kpc = 10
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 121.298
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);
            cNGC2403.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput cDiskAndGas = cCurveBuilder.LoadRotationInput("disk-and-gas", "disk-and-gas.csv", 1, 1, DXF_COLOR.dblue);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            /*
            double dR2_kpc = 19.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1c = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((11 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2c = (int)((19.6 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[3];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.4, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.55, 0);
            acNudges[2] = new LinearNudge(nIndex1c, nIndex2c, 0, 1);
            LayeredBody cDiskBody = cDiskAndGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);
            */

            /*
            double dR2_kpc = 19.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1c = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            //int nIndex2b = (int)((11 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((11.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2c = (int)((19.6 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[3];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.4, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.7, 0);
            acNudges[2] = new LinearNudge(nIndex1c, nIndex2c, 0, 1);
            LayeredBody cDiskBody = cDiskAndGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 1.5, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 105, 2.3);
            */

            /*
            double dR2_kpc = 19.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((11 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.4, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.55, 0);
            LayeredBody cDiskBody = cDiskAndGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 98, 2.5);
            */

            double dR2_m = cDiskAndGas.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cBulgeBody = new ShellBody(0, dR2_m, 1, 1000);
            cBulgeBody.SetDensityCurveFromVelocityArray(cDiskAndGas.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dDiskAndGasMass_kg = cBulgeBody.CalculateBodyMass();
            cDiskAndGas.Body = cBulgeBody;

            //double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, null, cDiskAndGas, null);
            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, cDiskAndGas, null, null);
            // dTotalMass_kg = 3.10E+40;

            int n1 = 1;
        }

        /*
        public static void DrawCurves_NGC2403_DC14_flat(string sBasePath)
        {
            // Fireworks Galaxy

            // Constellation: Camelopardalis
            // Location: outlying member of the M81 Group
            // Distance: 3.010-3.930 Mpc
            // Redshift (v. Heliocentric): 133 ± 0 km/s
            // Redshift (v. Galactocentric): 230 ± 4 km/s
            // Redshift (v. Local Group): 270 ± 8 km/s
            // Redshift (v. 3K CMB): 182 ± 3 km/s
            // (M/L)d: .56

            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403_DC14_flat",
                Hf_km_s_Mpc = 3.1,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 19.6,
                R_Max_kpc = 22.67653, // 20 + this: (21.151 px / 39.512 px) * 5 kpc = 2.67653
                V_Max_km_s = 160, // 150 + this: (7.581 px / 37.906 px) * 50 kpc = 10
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 121.298
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);
            cNGC2403.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 19.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((11 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.4, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.55, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 98, 2.5);

            nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 20, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 45, 12);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, null, cDisk, cGas);
            dTotalMass_kg = 7.23E+40;

            int n1 = 1;
        }
        */

        public static void DrawCurves_NGC2915_Burkert(string sBasePath)
        {
            // Constellation: Chamaeleon
            // Location: right on the edge of the Local Group
            // Distance: 3.580-4.290 Mpc
            // Redshift (v. Heliocentric): 468 ± 3 km/s
            // Redshift (v. Galactocentric): 265 ± 9 km/s
            // Redshift (v. Local Group): 192 ± 17 km/s
            // Redshift (v. 3K CMB): 588 ± 9 km/s
            // (M/L)d: .42

            // Fit: 2

            GalaxyParams cNGC2915 = new GalaxyParams()
            {
                Name = "NGC2915_Burkert",
                Hf_km_s_Mpc = 12,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .4,
                R_DataMax_kpc = 9.9,
                R_Max_kpc = 11.41348, // 10 + this: (22.193 px / 31.402 px) * 2 kpc = 1.41348
                V_Max_km_s = 100,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.958
            };

            cNGC2915.Path = Path.Combine(sBasePath, cNGC2915.Name);
            cNGC2915.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2915);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 9.9;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 30, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 32, .5);

            nIndex2 = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges, 20, 10);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2915, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.56E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC2976_DC14_LCDM(string sBasePath)
        {
            // Constellation: Ursa Major
            // Location: M81 Group
            // Distance: 3.520-3.630 Mpc
            // Redshift (v. Heliocentric): 3 ± 2 km/s
            // Redshift (v. Galactocentric): 105 ± 5 km/s
            // Redshift (v. Local Group): 138 ± 8 km/s
            // Redshift (v. 3K CMB): 90 ± 7 km/s
            // (M/L)d: .60

            // Fit: 4.5

            GalaxyParams cNGC2976 = new GalaxyParams()
            {
                Name = "NGC2976_DC14_LCDM",
                Hf_km_s_Mpc = .2,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .13,
                R_DataMax_kpc = 2.3,
                R_Max_kpc = 2.68005, // 2.5 + this: (12.039 px / 33.432 px) * .5 kpc = 0.18005
                V_Max_km_s = 90,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 122.681
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

            double dR2_kpc = 2.3;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((.6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new LinearNudge(0, nIndex2, -.6, 0);
            acNudges[1] = new FullParabolicNudge(0, nIndex2b, 0, -.2, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.6, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 62, 1.8);

            nIndex2 = (int)((1.1 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            nIndex2b = (int)((.6 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.8, 0);
            acNudges[1] = new FullParabolicNudge(0, nIndex2b, 0, -.4, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 30, 2);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2976, null, cDisk, cGas);
            // dTotalMass_kg = 2.73E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC3109_Burkert(string sBasePath)
        {
            // Constellation: Hydra
            // Location: Local Group
            // Distance: 1.333 Mpc
            // Redshift (v. Heliocentric): 403 ± 1 km/s
            // Redshift (v. Galactocentric): 193 ± 8 km/s
            // Redshift (v. Local Group): 109 ± 18 km/s
            // Redshift (v. 3K CMB): 738 ± 24 km/s
            // (M/L)d: .51

            // Fit: 5

            GalaxyParams cNGC3109 = new GalaxyParams()
            {
                Name = "NGC3109_Burkert",
                Hf_km_s_Mpc = 8,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 6.35,
                R_Max_kpc = 7.36648, // 6 + this: (33.241 px / 48.652 px) * 2 kpc = 1.36648 
                V_Max_km_s = 80,
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
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 6, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC3109, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.03E+39

            int n1 = 1;
        }

        public static void DrawCurves_NGC3741_Burkert(string sBasePath)
        {
            // Constellation: Ursa Major
            // Location: M94 Group
            // Distance: 3.150-3.230 Mpc
            // Redshift (v. Heliocentric): 230 ± 1 km/s
            // Redshift (v. Galactocentric): 268 ± 2 km/s
            // Redshift (v. Local Group): 264 ± 2 km/s
            // Redshift (v. 3K CMB): 456 ± 16 km/s
            // (M/L)d: .77

            // Fit: 4.5. The fit is 4 for an Hf = 5.

            GalaxyParams cNGC3741 = new GalaxyParams()
            {
                Name = "NGC3741_Burkert",
                Hf_km_s_Mpc = 5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .28,
                R_DataMax_kpc = 7.13,
                R_Max_kpc = 8.22033, // 8 + this: (4.803 px / 43.599 px) * 2 kpc = 0.22033
                V_Max_km_s = 60,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 118.141
            };

            cNGC3741.Path = Path.Combine(sBasePath, cNGC3741.Name);
            cNGC3741.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC3741);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 7.13;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .5, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 12, .5);

            nIndex2 = (int)((1 / dR2_kpc) * nNumDiskBodyLayers);
            //acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC3741, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 4.99E+38, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC4068_coreNFW_LCDM(string sBasePath)
        {
            // Constellation: Ursa Major
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.300 Mpc
            // Redshift (v. Heliocentric): 206 ± 2 km/s
            // Redshift (v. Galactocentric): 278 ± 4 km/s
            // Redshift (v. Local Group): 286 ± 5 km/s
            // Redshift (v. 3K CMB): 388 ± 13 km/s
            // (M/L)d: .45

            // Fit: 4.5

            GalaxyParams cNGC4068 = new GalaxyParams()
            {
                Name = "NGC4068_coreNFW_LCDM",
                Hf_km_s_Mpc = 2.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .23,
                R_DataMax_kpc = 2.28,
                R_Max_kpc = 2.64915, // 2.5 + this: (10.089 px / 33.821 px) * .5 kpc = 0.14915
                V_Max_km_s = 60,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 117.45
            };

            cNGC4068.Path = Path.Combine(sBasePath, cNGC4068.Name);
            cNGC4068.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC4068);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.6, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 2.28;
            int nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.9, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3.6, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC4068, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 4.84E+38, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC4214_Einasto_LCDM(string sBasePath)
        {
            // Constellation: Canes Venatici
            // Location: M81 Group
            // Distance: 2.700-2.930 Mpc
            // Redshift (v. Heliocentric): 291 ± 1 km/s
            // Redshift (v. Galactocentric): 312 ± 1 km/s
            // Redshift (v. Local Group): 295 ± 1 km/s
            // Redshift (v. 3K CMB): 550 ± 18 km/s
            // (M/L)d: .48

            // Fit: 3.5. At an M/L (disk) ratio of .6 (basically, the disk curve from NFW LCDM), the same fit
            // is achieved with an Hf = 6.

            GalaxyParams cNGC4214 = new GalaxyParams()
            {
                Name = "NGC4214_Einasto_LCDM",
                Hf_km_s_Mpc = 7,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .24,
                R_DataMax_kpc = 5.61,
                R_Max_kpc = 6.46282, // 6 + this: (12.833 px / 55.456 px) * 2 kpc = 0.46282
                V_Max_km_s = 90,
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

            double dR2_kpc = 5.61;
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
            // dTotalMass_kg = 1.28E+39

            int n1 = 1;
        }

        public static void DrawCurves_NGC6789_coreNFW_LCDM(string sBasePath)
        {
            // Constellation: Draco
            // Location: Local Void
            // Distance: 3.600 Mpc
            // Redshift (v. Heliocentric): -151 ± 2 km/s
            // Redshift (v. Galactocentric): 66 ± 9 km/s
            // Redshift (v. Local Group): 134 ± 17 km/s
            // Redshift (v. 3K CMB): -275 ± 9 km/s
            // (M/L)d: .62

            // Fit: 2.5. A higher ML (disk) ratio (such as that of the DC14 LCDM model) will lower the Hl.

            GalaxyParams cNGC6789 = new GalaxyParams()
            {
                Name = "NGC6789_coreNFW_LCDM",
                Hf_km_s_Mpc = 3.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .11,
                R_DataMax_kpc = .73,
                R_Max_kpc = .84611, // .8 + this: (9.765 px / 42.359 px) * .2 kpc = 0.04611
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
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

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
            // dTotalMass_kg = 1.47E+38

            int n1 = 1;
        }

        public static void DrawCurves_NGC6946_Burkert(string sBasePath)
        {
            // Fireworks Galaxy

            // Constellation:
            // Location: near the Local Group
            // Distance: 4.510 Mpc
            // Redshift (v. Heliocentric): 48 ± 2 km/s
            // Redshift (v. Galactocentric): 275 ± 9 km/s
            // Redshift (v. Local Group): 352 ± 18 km/s
            // Redshift (v. 3K CMB): -133 ± 13 km/s
            // (M/L)b: .63
            // (M/L)d: .58

            // Fit: 5

            GalaxyParams cNGC6946 = new GalaxyParams()
            {
                Name = "NGC6946_Burkert",
                Hf_km_s_Mpc = 1.1,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .3,
                R_DataMax_kpc = 18.9,
                R_Max_kpc = 21.87985, // 20 + this: (15.396 px / 40.95 px) * 5 kpc = 1.87985
                V_Max_km_s = 200,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 122.62
            };

            cNGC6946.Path = Path.Combine(sBasePath, cNGC6946.Name);
            cNGC6946.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC6946);
            GalaxyRotationInput cBulge = cCurveBuilder.LoadRotationInput("bulge", "bulge.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the bulge, stellar-disk, and gas bodies.

            // The bulge density profile can be derived via the velocity data.
            double dR2_m = cBulge.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cBulgeBody = new ShellBody(0, dR2_m, 1, 1000);
            cBulgeBody.SetDensityCurveFromVelocityArray(cBulge.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cBulgeBody.CalculateBodyMass();
            cBulge.Body = cBulgeBody;

            double dR2_kpc = 18.9;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((10 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((7 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((12 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 3.2, 0, 0);
            acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.4, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 147, 6);

            nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.7, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 20, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2, 40, 13.5);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC6946, cBulge, cDisk, cGas);
            // dTotalMass_kg = 7.23E+40, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_NGC7793_Burkert(string sBasePath)
        {
            // Constellation: Sculptor
            // Location: Sculptor Group
            // Distance: 3.390-3.840 Mpc
            // Redshift (v. Heliocentric): 227 ± 2 km/s
            // Redshift (v. Galactocentric): 226 ± 2 km/s
            // Redshift (v. Local Group): 250 ± 3 km/s
            // Redshift (v. 3K CMB): -53 ± 20 km/s
            // (M/L)d: .56

            // Fit: 4

            GalaxyParams cNGC7793 = new GalaxyParams()
            {
                Name = "NGC7793_Burkert",
                Hf_km_s_Mpc = .35,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .17,
                R_DataMax_kpc = 7.97,
                R_Max_kpc = 9.19388, // 8 + this: (23.27 px / 38.982 px) * 2 kpc = 1.19388
                V_Max_km_s = 120,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.958
            };

            cNGC7793.Path = Path.Combine(sBasePath, cNGC7793.Name);
            cNGC7793.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC7793);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 7.97;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 70, 2.5);

            nIndex2 = (int)((3 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.6, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC7793, null, cDisk, cGas);
            // dTotalMass_kg = 9.83E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC04305_Burkert(string sBasePath)
        {
            // Holmberg II

            // Constellation: Ursa Major
            // Location: M81 Group.
            // Distance: 3.240 Mpc
            // Redshift (v. Heliocentric): 157 ± 1 km/s
            // Redshift (v. Galactocentric): 269 ± 5 km/s
            // Redshift (v. Local Group): 311 ± 9 km/s
            // Redshift (v. 3K CMB): 203 ± 3 km/s
            // (M/L)d: .50

            // Fit: 2

            GalaxyParams cUGC04305 = new GalaxyParams()
            {
                Name = "UGC04305_Burkert",
                Hf_km_s_Mpc = 2,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .29,
                R_DataMax_kpc = 5.45,
                R_Max_kpc = 6.31698, // 6 + this: (8.992 px / 56.735 px) * 2 kpc = 0.31698
                V_Max_km_s = 70,
                R_LongTick_kpc = 2,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 119.729
            };

            cUGC04305.Path = Path.Combine(sBasePath, cUGC04305.Name);
            cUGC04305.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC04305);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 5.45;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, 1, 0);
            //acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 24, 3);

            nNumDiskBodyLayers = 3000;
            nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.5, 0);
            //acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 6, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges, 26, 5.45);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC04305, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.70E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC04483_Burkert(string sBasePath)
        {
            // Constellation: Ursa Major
            // Location: outlying member of the M81 Group
            // Distance: 3.200 Mpc
            // Redshift (v. Heliocentric): 156 ± 0 km/s
            // Redshift (v. Galactocentric): 263 ± 4 km/s
            // Redshift (v. Local Group): 303 ± 9 km/s
            // Redshift (v. 3K CMB): 213 ± 4 km/s
            // (M/L)d: .50

            // Fit: 4

            GalaxyParams cUGC04483 = new GalaxyParams()
            {
                Name = "UGC04483_Burkert",
                Hf_km_s_Mpc = 1.8,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .09,
                R_DataMax_kpc = 1.21,
                R_Max_kpc = 1.40141, // 1 + this: (51.329 px / 63.936 px) * .5 kpc = 0.40141
                V_Max_km_s = 30,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 5,
                R_Max_px = 179.2,
                V_Max_px = 117.509
            };

            cUGC04483.Path = Path.Combine(sBasePath, cUGC04483.Name);
            cUGC04483.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC04483);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 1.21;
            int nNumDiskBodyLayers = 4000;
            int nIndex2 = (int)((.3 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .3, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1, 0, -.2, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 5.1, .3);

            nNumDiskBodyLayers = 4000;
            nIndex2 = (int)((1.8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null, 12, 1.2);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC04483, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 8.18E+37, verified 6/29/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC07232_Burkert(string sBasePath)
        {
            // NGC 4190

            // Constellation: Canes Venatici
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 2.950 Mpc
            // Redshift (v. Heliocentric): 228 ± 1 km/s
            // Redshift (v. Galactocentric): 250 ± 1 km/s
            // Redshift (v. Local Group): 232 ± 1 km/s
            // Redshift (v. 3K CMB): 486 ± 18 km/s
            // (M/L)d: .50

            // Fit: 3.5

            GalaxyParams cUGC07232 = new GalaxyParams()
            {
                Name = "UGC07232_Burkert",
                Hf_km_s_Mpc = 4,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .22,
                R_DataMax_kpc = .82,
                R_Max_kpc = .94879, // .8 + this: (28.102 px / 37.774 px) * .2 kpc = 0.14879
                V_Max_km_s = 50,
                R_LongTick_kpc = .2,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 117.625
            };

            cUGC07232.Path = Path.Combine(sBasePath, cUGC07232.Name);
            cUGC07232.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC07232);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

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

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC07232, cDisk, null, null, null, null, false, cGas);
            // dTotalMass_kg = 1.74E+38, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC07524_DC14_flat(string sBasePath)
        {
            // NGC 4395

            // Constellation: Canes Venatici
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.410-4.610 Mpc
            // Redshift (v. Heliocentric): 319 ± 1 km/s
            // Redshift (v. Galactocentric): 335 ± 1 km/s
            // Redshift (v. Local Group): 314 ± 1 km/s
            // Redshift (v. 3K CMB): 585 ± 19 km/s
            // (M/L)d: .43

            // Fit: 1

            GalaxyParams cUGC07524 = new GalaxyParams()
            {
                Name = "UGC07524_DC14_flat",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .45,
                R_DataMax_kpc = 10.5,
                R_Max_kpc = 12.12777, // 10 + this: (31.44 px / 73.88 px) * 5 kpc = 2.12777
                V_Max_km_s = 100,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 120.958
            };

            cUGC07524.Path = Path.Combine(sBasePath, cUGC07524.Name);
            cUGC07524.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC07524);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 10.5;
            int nNumDiskBodyLayers = 4000;
            int nIndex2 = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 15, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new LinearNudge(0, nIndex2, -.8, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 15, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC07524, null, cDisk, cGas);
            // dTotalMass_kg = 6.06E+39, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC07559_coreNFW_LCDM(string sBasePath)
        {
            // DDO 126

            // Constellation:
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.970 Mpc
            // Redshift (v. Heliocentric): 219 ± 2 km/s
            // Redshift (v. Galactocentric): 248 ± 2 km/s
            // Redshift (v. Local Group): 233 ± 2 km/s
            // Redshift (v. 3K CMB): 470 ± 18 km/s
            // (M/L)d: .43

            // Fit: 4

            GalaxyParams cUGC07559 = new GalaxyParams()
            {
                Name = "UGC07559_coreNFW_LCDM",
                Hf_km_s_Mpc = 1.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .37,
                R_DataMax_kpc = 2.43,
                R_Max_kpc = 2.81089, // 2.5 + this: (19.82 px / 31.876 px) * .5 kpc = 0.31089
                V_Max_km_s = 40,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cUGC07559.Path = Path.Combine(sBasePath, cUGC07559.Name);
            cUGC07559.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC07559);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 2.43;
            int nIndex2 = (int)((2 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.75, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC07559, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 3.10E+38, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC07577_coreNFW_LCDM(string sBasePath)
        {
            // DDO 125

            // Constellation:
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 2.600 Mpc
            // Redshift (v. Heliocentric): 195 ± 0 km/s
            // Redshift (v. Galactocentric): 245 ± 2 km/s
            // Redshift (v. Local Group): 239 ± 3 km/s
            // Redshift (v. 3K CMB): 417 ± 16 km/s
            // (M/L)d: .37

            // Fit: 3.5

            // Hf is anomalous.

            // From "Baryonic distributions in galaxy dark matter haloes – II. Final results"
            // https://academic.oup.com/mnras/article/476/4/5127/4907988

            // UGC 07577 is a nearby (2.6 Mpc; Dalcanton et al. 2009) dwarf irregular galaxy.Archival
            // VLA D configuration data were originally published in Hunter et al. (1998) as part of a
            // mosaic around the large irregular galaxy NGC 4449.The H i disc roughly follows the
            // exponential stellar distribution.The highest column density regions appear to overlap
            // with the brightest ionized gas emission in the narrow-band H α image.

            // Unlike the ionized gas kinematics that are dominated by turbulent motions, the neutral
            // gas velocity field indicates clear solid body rotation. However, the H i rotation curve
            // only reaches circular velocities on the order of 20 km s−1 at the last measured point.
            // The same result has been found by numerous other studies using WSRT (e.g.Tully et al. 1978;
            // Stil & Israel 2002; Swaters et al. 2009) and VLA(e.g.Ott et al. 2012) observations.
            // Swaters(1999) additionally found that the rotation curve of UGC 07577 could be decomposed
            // without the need for dark matter and suggest that it could have formed in tidal debris
            // around NGC 4449(e.g.Barnes & Hernquist 1992).UGC 07577’s proximity to NGC 4449(projected
            // distance ∼40 kpc) and the existence of the tidal streams around NGC 4449 make UGC 07577 a
            // strong tidal dwarf galaxy candidate(e.g.Hunter, Hunsberger & Roye 2000).In light of this,
            // UGC 07577 is not included in the kinematic analysis.

            GalaxyParams cUGC07577 = new GalaxyParams()
            {
                Name = "UGC07577_coreNFW_LCDM",
                Hf_km_s_Mpc = .7,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .19,
                R_DataMax_kpc = 1.56,
                R_Max_kpc = 1.80660, // 1.5 + this: (19.82 px / 31.876 px) * .5 kpc = 0.30660
                V_Max_km_s = 25,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 5,
                R_Max_px = 179.2,
                V_Max_px = 116.187
            };

            cUGC07577.Path = Path.Combine(sBasePath, cUGC07577.Name);
            cUGC07577.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC07577);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null, 7, 1.2);

            double dR2_kpc = 1.56;
            int nIndex2 = (int)((.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.6, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC07577, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 7.55E+37, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC07866_coreNFW_LCDM(string sBasePath)
        {
            // DDO 141, IC 3687

            // Constellation:
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.570 Mpc
            // Redshift (v. Heliocentric): 354 ± 1 km/s
            // Redshift (v. Galactocentric): 394 ± 2 km/s
            // Redshift (v. Local Group): 381 ± 2 km/s
            // Redshift (v. 3K CMB): 592 ± 17 km/s
            // (M/L)d: .48

            // Fit: 3.5 - Fit is within the error bars, but there is some skew where, at small radii,
            // the output is at the bottom of the error bars and skews to the top of the error bars
            // at larger radii.

            GalaxyParams cUGC07866 = new GalaxyParams()
            {
                Name = "UGC07866_coreNFW_LCDM",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .35,
                R_DataMax_kpc = 2.29,
                R_Max_kpc = 2.64907, // 2.5 + this: (10.084 px / 33.823 px) * .5 kpc = 0.14907
                V_Max_km_s = 45,
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 118.283
            };

            cUGC07866.Path = Path.Combine(sBasePath, cUGC07866.Name);
            cUGC07866.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC07866);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 2.5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 2.29;
            int nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((.6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new LinearNudge(0, nIndex2, -.75, 0);
            acNudges[1] = new FullParabolicNudge(0, nIndex2b, 0, -.35, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC07866, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 2.23E+38, verified 6/30/25

            int n1 = 1;
        }

        public static void DrawCurves_UGC08490_Burkert(string sBasePath)
        {
            // NGC 5204

            // Constellation: Ursa Major
            // Location: M101 Group
            // Distance: 4.790-5.550 Mpc
            // Redshift (v. Heliocentric): 201 ± 1 km/s
            // Redshift (v. Galactocentric): 318 ± 5 km/s
            // Redshift (v. Local Group): 339 ± 8 km/s
            // Redshift (v. 3K CMB): 322 ± 8 km/s
            // (M/L)d: .81

            // Fit: 4.5

            GalaxyParams cUGC08490 = new GalaxyParams()
            {
                Name = "UGC08490_Burkert",
                Hf_km_s_Mpc = 2.5, //3,
                R_Increment_kpc = .05,
                R_DataMin_kpc = .40,
                R_DataMax_kpc = 11,
                R_Max_kpc = 12.77955, // 10 + this: (21.151 px / 39.512 px) * 5 kpc = 2.77955
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

            double dR2_kpc = 11;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4.5, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            nIndex2 = (int)((4.5 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new LinearNudge(0, nIndex2, -.5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 10, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC08490, null, cDisk, cGas);
            // dTotalMass_kg = 3.62E+39

            int n1 = 1;
        }

        public static void DrawCurves_UGCA444_Burkert(string sBasePath)
        {
            // WLM (Wolf–Lundmark–Melotte), UGCA 444

            // Constellation: Hydra
            // Location: Local Group
            // Distance: 930 ± 30 kpc
            // Redshift (v. Heliocentric): -122 ± 1 km/s
            // Redshift (v. Galactocentric): -65 ± 3 km/s
            // Redshift (v. Local Group): -16 ± 6 km/s
            // Redshift (v. 3K CMB): -457 ± 23 km/s
            // (M/L)d: .49

            // Fit: 4

            GalaxyParams cUGCA444 = new GalaxyParams()
            {
                Name = "UGCA444_Burkert",
                Hf_km_s_Mpc = 3.2,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .18,
                R_DataMax_kpc = 2.6,
                R_Max_kpc = 3,
                V_Max_km_s = 50,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 122.328
            };

            cUGCA444.Path = Path.Combine(sBasePath, cUGCA444.Name);
            cUGCA444.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGCA444);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 2.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 2, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 4, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges, 4, 1.5);

            int nIndex1 = (int)((.1 / dR2_kpc) * nNumDiskBodyLayers);
            nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            //acNudges[0] = new ParabolicNudge(nIndex1, nIndex2, .5, 0, 0);
            acNudges[0] = new LinearNudge(nIndex1, nIndex2, .5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, .1, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGCA444, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.88E+38, verified 6/30/25

            int n1 = 1;
        }
    }
}

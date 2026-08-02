using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    internal partial class Program
    {
        // Galaxy-rotation data are here:
        // C:\BooksPapersAndManuals\physics\astrophysics\galaxy_rotation_papers

        private static void DrawCurves_DDO154(string sBasePath)
        {
            // OBSOLETE!!
            // See DrawCurves_DDO154_coreNFW_flat() has the latest curves.

            // Rotation data are from:
            // https://tritonstation.com/2018/11/ -> i=61

            // Constellation: Coma Berenices
            // Location: M94 Group (Canes I Group or Canes Venatici I Group)
            // Distance: 4.040 Mpc
            // Redshift (v. Heliocentric): 364 ± 1 km/s
            // Redshift (v. Galactocentric): 372 ± 1 km/s
            // Redshift (v. Local Group): 344 ± 1 km/s
            // Redshift (v. 3K CMB): 639 ± 19 km/s

            // https://www.sci.news/astronomy/hubble-image-dwarf-galaxy-ngc-4789a-04360.html
            // Also known as LEDA 43869, DDO 154 and UGC 8024, this galaxy is a physical
            // companion of the famous spiral galaxy Messier 64 (NGC 4826).
            //
            // In 1975, astronomers suggested that these two galaxies form a small group, or cloud, 
            // of galaxies with the huge spiral Messier 94 and a number of fainter galaxies.

            // M64 is part of the Canes Venatici I Group

            // M94 Group: https://en.wikipedia.org/wiki/M94_Group, https://www.universeguide.com/galaxy/ugc7559

            // Fit: 3

            GalaxyParams cDDO154 = new GalaxyParams()
            {
                Name = "DDO154",
                Hf_km_s_Mpc = 4.5,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .17,
                R_DataMax_kpc = 7.75,
                R_Max_kpc = 8,
                V_Max_km_s = 65,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 99.174
            };

            cDDO154.Path = Path.Combine(sBasePath, cDDO154.Name);
            cDDO154.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1.3

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cDDO154);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cMOND = cCurveBuilder.LoadRotationInput("MOND", "MOND.csv", 1, 1, DXF_COLOR.green);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 7.75;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((7.75 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 7, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, null /*acNudges*/);

            dR2_kpc = 7.75;
            nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, 3000, 0, 9, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO154, null, cDisk, cGas);
        }

        private static void DrawCurves_DDO210_LT(string sBasePath)
        {
            // DDO 210, Aquarius Dwarf

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MODELS OF DWARF GALAXIES FROM LITTLE THINGS.pdf"

            // Constellation: Aquarius
            // Location: Local Group
            // Distance: .977-1.030 Mpc
            // Redshift (v. Heliocentric): -140 ± 1 km/s
            // Redshift (v. Galactocentric): -26 ± 5 km/s
            // Redshift (v. Local Group): -10 ± 9 km/s
            // Redshift (v. 3K CMB): -419 ± 20 km/s
            // (M/L)d: .43

            // Fit: 4.5

            GalaxyParams cDDO210 = new GalaxyParams()
            {
                Name = "DDO210_LT",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = .001,
                R_DataMin_kpc = .042,
                R_DataMax_kpc = .307,
                R_Max_kpc = .4,
                V_Max_km_s = 15,
                R_LongTick_kpc = .2,
                V_LongTick_km_s = 5,
                R_Max_px = 179.2,
                V_Max_px = 110.373
            };

            cDDO210.Path = Path.Combine(sBasePath, cDDO210.Name);
            cDDO210.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cDDO210);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null, 2.1, .3);

            double dR2_kpc = .5;
            int nIndex1 = (int)((0 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2 = (int)((.1 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((.4 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            //acNudges[0] = new ParabolicNudge(0, nIndex2, 4, 0, 0);
            //acNudges[0] = new LinearNudge(0, nIndex2, 14, 0);
            acNudges[0] = new LinearNudge(0, nIndex2, -.3, 0);
            acNudges[1] = new LinearNudge(0, nIndex3, .5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1.0, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 4.7, .3);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cDDO210, null, cDisk, cGas, null, null, false);
            // 9.87E+37 1.10E+37

            int n1 = 1;
        }

        private static void DrawCurves_IC1613_LT(string sBasePath)
        {
            // IC 1613, DDO 008

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MODELS OF DWARF GALAXIES FROM LITTLE THINGS.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 714 kpc
            // Redshift (v. Heliocentric): -234 ± 1 km/s
            // Redshift (v. Galactocentric): -155 ± 3 km/s
            // Redshift (v. Local Group): -91 ± 9 km/s
            // Redshift (v. 3K CMB): -560 ± 23 km/s

            // Fit: poor. This is a tiny galaxy with poor curve and Hl fits.

            GalaxyParams cIC1613 = new GalaxyParams()
            {
                Name = "IC1613_LT",
                Hf_km_s_Mpc = .1,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .16,
                R_DataMax_kpc = 2.7,
                R_Max_kpc = 2.8,
                V_Max_km_s = 24,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 109.005
            };

            cIC1613.Path = Path.Combine(sBasePath, cIC1613.Name);
            cIC1613.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cIC1613);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 2.6;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 6.4, 2.6);

            int nIndex1 = (int)((.1 / dR2_kpc) * nNumDiskBodyLayers);
            nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            //acNudges[0] = new ParabolicNudge(nIndex1, nIndex2, .5, 0, 0);
            acNudges[0] = new LinearNudge(nIndex1, nIndex2, .5, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.Linear_RampDown, null, 12.0, 2.6);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cIC1613, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.88E+38

            int n1 = 1;
        }

        private static void DrawCurves_M31(string sBasePath)
        {
            // NGC 0224

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "M31-Andromeda.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 765 kpc
            // Redshift (v. Heliocentric) -297 ± 1 km/s
            // Redshift (v. Galactocentric): -119 ± 7 km/s
            // Redshift (v. Local Group): -31 ± 16 km/s
            // Redshift (v. 3K CMB): -582 ± 20 km/s
            // (M/L)b: 2.2
            // (M/L)d: 1.7

            // Stellar Mass: 10E10 - 15E10 SM
            // Neutral hydrogen: 7.2E9 SM
            // Molecular hydrogen: 3.4E8 SM

            // Fit: 3.5 (Hl = 4)
            // Fit: 3.5 (Hl = 5)
            // Fit: 3.0 (Hl = 6)

            GalaxyParams cM31 = new GalaxyParams()
            {
                Name = "M31",
                Hf_km_s_Mpc = 3.5, //5, //4,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .05,
                R_DataMax_kpc = 38,
                R_Max_kpc = 40,
                V_Max_km_s = 350,
                R_LongTick_kpc = 10,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 108.731
            };

            cM31.Path = Path.Combine(sBasePath, cM31.Name);
            cM31.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM31);

            // Setting the color to white will result in the raw curve not being drawn.
            GalaxyRotationInput cBlackHole = cCurveBuilder.LoadRotationInput("black-hole", "black-dotted.csv", 1, 1, DXF_COLOR.white);
            double[] vi_BlackHole_km_s = cBlackHole.Data.Data;

            GalaxyRotationInput cAtomicH = cCurveBuilder.LoadRotationInput("neutral-H", "blue-solid.csv", 1, 1, DXF_COLOR.magenta);

            // Setting the color to white will result in the raw curve not being drawn.
            GalaxyRotationInput cMolecularH2 = cCurveBuilder.LoadRotationInput("molecular-H2", "blue-dotted.csv", 1, 1, DXF_COLOR.magenta);

            GalaxyRotationInput cBulge = cCurveBuilder.LoadRotationInput("bulge", "red-dotted1.csv", 1, 1, DXF_COLOR.magenta);

            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "red-solid.csv", 1, 1, DXF_COLOR.magenta);

            cCurveBuilder.LoadRotationInput("dark-matter", "net-curve.csv", 1, 1, DXF_COLOR.green);

            var cInputArray = cCurveBuilder.InputArray;
            double[] v1 = new double[cInputArray.Length];
            double[] v1p2 = new double[cInputArray.Length];

            double Hf_km_s_kpc = cM31.Hf_km_s_Mpc / 1000;
            double Hf_s = Hf_km_s_kpc / AstronomicalConversions.km_per_kpc;
            double cHf_km_s_kpc = AstronomicalConstants.c_km_s * Hf_km_s_kpc;

            ////////////////////////////////
            // Calculate the black hole mass.

            int nIndexBH = cInputArray.GetIndex(.5); // .5 kpc
            double r_p55_kpc = cInputArray.GetFloatingIndex(nIndexBH);
            Debug.Assert(DebugHelpers.XDecimalSanityCheck(r_p55_kpc, .55, 6));

            double v_p55_km_s = vi_BlackHole_km_s[nIndexBH];
            double dBlackHoleMass_kg = AstronomyHelpers.GetGalacticMassFromCircularOrbitVelocity_kg(v_p55_km_s, r_p55_kpc);
            cBlackHole.Mass_kg = dBlackHoleMass_kg;

#if DEBUG
            {
                // Note the net velocities are much higher here in these sanity checks than in actuality 
                // because the total mass passed into GetCircularOrbitVelocityFromMass_km_s() is the
                // black-hole mass, where it should be the galaxy mass, which is orders of magnitude larger.

                double v1_N_km_s = AstronomyHelpers.GetNewtonianCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 1 * AstronomicalConversions.m_per_kpc);
                Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(v1_N_km_s, 20.649));

                /*
                double v1_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 1 * AstronomicalConversions.m_per_kpc, Hf_s, dBlackHoleMass_kg);
                double v1b_km_s = v1_N_km_s + Math.Sqrt(cHf_km_s_kpc * 1);
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(v1_km_s, v1b_km_s, 6));

                double v5_N_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 5 * AstronomicalConversions.m_per_kpc);
                Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(v5_N_km_s, 9.234));

                double v5_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 5 * AstronomicalConversions.m_per_kpc, Hf_s, dBlackHoleMass_kg);
                double v5b_km_s = v5_N_km_s + Math.Sqrt(cHf_km_s_kpc * 5);
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(v5_km_s, v5b_km_s, 6));

                double v10_N_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 10 * AstronomicalConversions.m_per_kpc);
                Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(v10_N_km_s, 6.530));

                double v10_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 10 * AstronomicalConversions.m_per_kpc, Hf_s, dBlackHoleMass_kg);
                double v10b_km_s = v10_N_km_s + Math.Sqrt(cHf_km_s_kpc * 10);
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(v10_km_s, v10b_km_s, 6));

                double v20_N_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 20 * AstronomicalConversions.m_per_kpc);
                Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(v20_N_km_s, 4.617));

                double v20_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 20 * AstronomicalConversions.m_per_kpc, Hf_s, dBlackHoleMass_kg);
                double v20b_km_s = v20_N_km_s + Math.Sqrt(cHf_km_s_kpc * 20);
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(v20_km_s, v20b_km_s, 6));

                double v30_N_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 30 * AstronomicalConversions.m_per_kpc);
                Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(v30_N_km_s, 3.770));

                double v30_km_s = AstronomyHelpers.GetCircularOrbitVelocityFromMass_km_s(dBlackHoleMass_kg, 30 * AstronomicalConversions.m_per_kpc, Hf_s, dBlackHoleMass_kg);
                double v30b_km_s = v30_N_km_s + Math.Sqrt(cHf_km_s_kpc * 30);
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(v30_km_s, v30b_km_s, 6));
                */
            }
#endif

            ////////////////////////////////
            // Create the bulge and disk bodies.

            // The bulge density profile can be derived via velocity data.
            double dR2_m = cBulge.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cBulgeBody = new ShellBody(0, dR2_m, 1, 1000);
            cBulgeBody.SetDensityCurveFromVelocityArray(cBulge.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cBulgeBody.CalculateBodyMass();
            cBulge.Body = cBulgeBody;

            // The disk density profile has to be generated via primatives and nudges.
            // Unlike the case for shells, accelerations (and thus velocity curves) from
            // a disk are very sensitive to subtle changes in the density profile.
            double dR2_kpc = 40;
            int nNumDiskBodyLayers = 2000;
            int nIndex2 = cDisk.Data.GetIndex(35);
            double r2_kpc = ((double)35 / (double)2000) * dR2_kpc;
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, 1.7, 0);

            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 40, 0, -8, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            ////////////////////////////////
            // Atomic H.

            acNudges[0] = new LinearNudge(0, 1300, -.15, 0);

            LayeredBody cAtomicHBody = cAtomicH.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 40, 0, 20, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 59, 34);

            ////////////////////////////////
            // Molecular H2.

            //nNumDiskBodyLayers = 3000;
            int nIndex1 = (int)(7.5 / 40.0) * nNumDiskBodyLayers;
            nIndex2 = (int)(30.0 / 40.0) * nNumDiskBodyLayers;
            acNudges[0] = new LinearNudge(nIndex1, nIndex2, 2, 0);
            //acNudges[0] = new ParabolicNudge(nIndex1, nIndex2, 2, 0, 0);

            LayeredBody cMolecularH2Body = cMolecularH2.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 7.5, 30, 0, -10, 0, DensityDistribution.HalfParabola_WaterSlide, null /*acNudges*/, 14, 11.5);
            double dMolecularH2Mass_kg = cMolecularH2Body.TotalMass_kg;

            ////////////////////////////////
            // Draw the rotation curves.

            /*
            double dTotalMass_SM = 2.45E+41 / AstronomicalConversions.kg_per_sm;
            // dTotalMass_SM = 123,210,307,422

            double dMd50_kg = AstronomyHelpers.GetGalacticDynamicMassFromMass(dTotalMass_kg, 50, cM31.Hf_km_s_Mpc);
            double dMd50_SM = dMd50_kg / AstronomicalConversions.kg_per_sm;
            // 340,549,409,248.63104

            double dMd150_kg = AstronomyHelpers.GetGalacticDynamicMassFromMass(dTotalMass_kg, 150, cM31.Hf_km_s_Mpc);
            double dMd150_SM = dMd150_kg / AstronomicalConversions.kg_per_sm;
            // 775,227,612,901.31409

            double dMd300_kg = AstronomyHelpers.GetGalacticDynamicMassFromMass(dTotalMass_kg, 300, cM31.Hf_km_s_Mpc);
            double dMd300_SM = dMd300_kg / AstronomicalConversions.kg_per_sm;
            // 1,427,244,918,380.3386
            */

            double dR_10kpc_m = 10 * AstronomicalConversions.m_per_kpc;
            double dR_log_10kpc_m = Math.Log10(dR_10kpc_m);

            double dR_20kpc_m = 20 * AstronomicalConversions.m_per_kpc;
            double dR_log_20kpc_m = Math.Log10(dR_20kpc_m);

            double dR_30kpc_m = 30 * AstronomicalConversions.m_per_kpc;
            double dR_log_30kpc_m = Math.Log10(dR_30kpc_m);

            double dR_40kpc_m = 40 * AstronomicalConversions.m_per_kpc;
            double dR_log_40kpc_m = Math.Log10(dR_40kpc_m);

            double dR_50kpc_m = 50 * AstronomicalConversions.m_per_kpc;
            double dR_log_50kpc_m = Math.Log10(dR_50kpc_m);

            // Set bCombineSelectCurves to true to fold the black hole into the
            // bulge and to combine the molecular and atomic H into one curve.
            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM31, cBulge, cDisk, cAtomicH, cMolecularH2, cBlackHole, false);
            // dTotalMass_kg = 2.45E+41

            int n1 = 1;
        }

        private static void DrawCurves_M31b(string sBasePath)
        {
            // NGC 0224

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "M31-Andromeda.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 765 kpc
            // Redshift (v. Heliocentric) -297 ± 1 km/s
            // Redshift (v. Galactocentric): -119 ± 7 km/s
            // Redshift (v. Local Group): -31 ± 16 km/s
            // Redshift (v. 3K CMB): -582 ± 20 km/s
            // (M/L)b: .8
            // (M/L)d: 1.7

            // Stellar Mass: 10E10 - 15E10 SM
            // Neutral hydrogen: 7.2E9 SM
            // Molecular hydrogen: 3.4E8 SM

            // Fit: 3.5

            GalaxyParams cM31 = new GalaxyParams()
            {
                Name = "M31b",
                Hf_km_s_Mpc = 6, //5.5,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .05,
                R_DataMax_kpc = 38,
                R_Max_kpc = 40,
                V_Max_km_s = 350,
                R_LongTick_kpc = 10,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 108.731
            };

            cM31.Path = Path.Combine(sBasePath, cM31.Name);
            cM31.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM31);

            // Setting the color to white will result in the raw curve not being drawn.
            GalaxyRotationInput cBlackHole = cCurveBuilder.LoadRotationInput("black-hole", "black-dotted.csv", 1, 1, DXF_COLOR.magenta);
            double[] vi_BlackHole_km_s = cBlackHole.Data.Data;

            GalaxyRotationInput cAtomicH = cCurveBuilder.LoadRotationInput("neutral-H", "blue-solid.csv", 1, 1, DXF_COLOR.magenta);

            // Setting the color to white will result in the raw curve not being drawn.
            GalaxyRotationInput cMolecularH2 = cCurveBuilder.LoadRotationInput("molecular-H2", "blue-dotted.csv", 1, 1, DXF_COLOR.magenta);

            GalaxyRotationInput cBulge = cCurveBuilder.LoadRotationInput("bulge", "red-dotted2.csv", 1, 1, DXF_COLOR.magenta);

            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "red-solid.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Calculate the black hole mass.

            var cInputArray = cCurveBuilder.InputArray;
            int nIndexBH = cInputArray.GetIndex(.5); // .5 kpc
            double r_p55_kpc = cInputArray.GetFloatingIndex(nIndexBH);
            Debug.Assert(DebugHelpers.XDecimalSanityCheck(r_p55_kpc, .55, 6));

            double v_p55_km_s = vi_BlackHole_km_s[nIndexBH];
            double dBlackHoleMass_kg = AstronomyHelpers.GetGalacticMassFromCircularOrbitVelocity_kg(v_p55_km_s, r_p55_kpc);
            cBlackHole.Mass_kg = dBlackHoleMass_kg;

            ////////////////////////////////
            // Create the bulge and disk bodies.

            // The bulge density profile can be derived via velocity data.
            double dR2_m = cBulge.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cBulgeBody = new ShellBody(0, dR2_m, 1, 1000);
            cBulgeBody.SetDensityCurveFromVelocityArray(cBulge.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cBulgeBody.CalculateBodyMass();
            cBulge.Body = cBulgeBody;

            // The disk density profile has to be generated via primatives and nudges.
            // Unlike the case for shells, accelerations (and thus velocity curves) from
            // a disk are very sensitive to subtle changes in the density profile.
            double dR2_kpc = 40;
            int nNumDiskBodyLayers = 2000;
            int nIndex2 = cDisk.Data.GetIndex(35);
            double r2_kpc = ((double)35 / (double)2000) * dR2_kpc;
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, 1.7, 0);

            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 40, 0, -8, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            ////////////////////////////////
            // Atomic H.

            acNudges[0] = new LinearNudge(0, 1300, -.15, 0);

            LayeredBody cAtomicHBody = cAtomicH.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 40, 0, 20, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 59, 34);

            ////////////////////////////////
            // Molecular H2.

            //nNumDiskBodyLayers = 3000;
            int nIndex1 = (int)(7.5 / 40.0) * nNumDiskBodyLayers;
            nIndex2 = (int)(30.0 / 40.0) * nNumDiskBodyLayers;
            acNudges[0] = new LinearNudge(nIndex1, nIndex2, 2, 0);
            //acNudges[0] = new ParabolicNudge(nIndex1, nIndex2, 2, 0, 0);

            LayeredBody cMolecularH2Body = cMolecularH2.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 7.5, 30, 0, -10, 0, DensityDistribution.HalfParabola_WaterSlide, null /*acNudges*/, 14, 11.5);
            double dMolecularH2Mass_kg = cMolecularH2Body.TotalMass_kg;

            ////////////////////////////////
            // Draw the rotation curves.

            // Set bCombineSelectCurves to true to fold the black hole into the
            // bulge and to combine the molecular and atomic H into one curve.
            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM31, cBulge, cDisk, cAtomicH, cMolecularH2, cBlackHole, false);
            // dTotalMass_kg = 1.80E+41

            int n1 = 1;
        }

        private static void DrawCurves_M31_extended(string sBasePath)
        {
            // NGC 0224

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "rotation curve and mass distribution of M31.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 765 kpc
            // Redshift (v. Heliocentric) -297 ± 1 km/s
            // Redshift (v. Galactocentric): -119 ± 7 km/s
            // Redshift (v. Local Group): -31 ± 16 km/s
            // Redshift (v. 3K CMB): -582 ± 20 km/s

            GalaxyParams cM31 = new GalaxyParams()
            {
                Name = "M31_extended",
                Hf_km_s_Mpc = 5, //6,
                R_Increment_kpc = 1, //2, //.2,
                R_DataMin_kpc = .12, // log(.12) = -.92
                R_DataMax_kpc = 140, // log(140) = 2.146128
                R_Min_kpc = 0, //.1, // log(.1) = -1
                R_Max_kpc = 140.26367, // 10 ^ 2 + this: (14.075 px / 95.784 px) = 0.146945
                V_Max_km_s = 290.21773, // 250 + this: (28.039 px / 34.859 px) * 50 kpc = 40.21773
                R_LongTick_kpc = 10, // log(10) = 1
                V_LongTick_km_s = 50,
                R_Max_px = 301.401,
                V_Max_px = 209.447,
                //R_AxisIsLog = true
            };

            cM31.Path = Path.Combine(sBasePath, cM31.Name);
            cM31.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM31);
            GalaxyRotationInput cBulge = cCurveBuilder.LoadRotationInput("bulge", "bulge.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cDiskAndGas = cCurveBuilder.LoadRotationInput("disk-and-gas", "disk-and-gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the bulge and disk bodies.

            // The bulge density profile can be derived via velocity data.
            double dR2_m = cBulge.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cBulgeBody = new ShellBody(0, dR2_m, 1, 1000);
            cBulgeBody.SetDensityCurveFromVelocityArray(cBulge.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cBulgeBody.CalculateBodyMass();
            cBulge.Body = cBulgeBody;

            // The disk density profile has to be generated via primatives and nudges.
            // Unlike the case for shells, accelerations (and thus velocity curves) from
            // a disk are very sensitive to subtle changes in the density profile.
            double dR2_kpc = 140;
            int nNumDiskBodyLayers = 2000;
            int nIndex2 = (int)((30 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex1b = (int)((15 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((40 / dR2_kpc) * nNumDiskBodyLayers);
            //int nIndex2 = (int)((15 / dR2_kpc) * nNumDiskBodyLayers);
            double r2_kpc = ((double)35 / (double)2000) * dR2_kpc;
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, 1.7, 0);
            //acNudges[1] = new FullParabolicNudge(nIndex1b, nIndex2b, 0, -.05, 0);
            //acNudges[0] = new LinearNudge(0, nIndex2, 2.5, 0);
            //LayeredBody cDiskBody = cDiskAndGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 40, 0, -8, 0, DensityDistribution.HalfParabola_WaterSlide, null);
            LayeredBody cDiskBody = cDiskAndGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 38, 0, -8, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            ////////////////////////////////
            // Draw the rotation curves.

            // Set bCombineSelectCurves to true to fold the black hole into the
            // bulge and to combine the molecular and atomic H into one curve.
            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM31, cBulge, cDiskAndGas, null, null, null, false);
            // dTotalMass_kg = 2.4517287710177792E+41
            // dTotalMass_kg = 2.45E+41

            int n1 = 1;
        }

        private static void DrawCurves_M33(string sBasePath)
        {
            // NGC 0598

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "The global stability of M33.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // M/L = 

            // Fit: 3.5 (Hl = 4)
            // Fit: 3 (Hl = 5)

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33",
                Hf_km_s_Mpc = 4,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .2,
                R_DataMax_kpc = 19.8,
                R_Max_kpc = 20,
                V_Max_km_s = 140,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 40,
                R_Max_px = 179.2,
                V_Max_px = 143.352
            };

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 15;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex3 = (int)((15 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 62, 4);

            dR2_kpc = 15;
            nNumDiskBodyLayers = 4000; // 2000;
            nIndex2 = (int)((15 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            acNudges[0] = new LinearNudge(0, nIndex2, 25, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, null, cDisk, cGas);
            // dTotalMass_kg = 1.333E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33b(string sBasePath)
        {
            // NGC 0598

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MOND in Local Group.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/L)d: .58

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33b",
                Hf_km_s_Mpc = 4, //2.2,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .2,
                R_DataMax_kpc = 18.7,
                R_Max_kpc = 19.5,
                V_Max_km_s = 160,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 107.54
            };

            // Fit: 3.5 (Hl = 2.2)
            // Fit: 3 (Hl = 4)

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cSpheroid = cCurveBuilder.LoadRotationInput("spheroid", "spheroid.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("MOND", "MOND.csv", 1, 1, DXF_COLOR.green);

            ////////////////////////////////
            // Create the bulge and disk bodies.

            // The bulge density profile can be derived via velocity data.
            double dR2_m = cSpheroid.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cSpheroidBody = new ShellBody(0, dR2_m, 1, 1000);
            cSpheroidBody.SetDensityCurveFromVelocityArray(cSpheroid.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cSpheroidBody.CalculateBodyMass();
            cSpheroid.Body = cSpheroidBody;

            double dR2_kpc = 15;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex3 = (int)((14 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges /*, 62, 4*/);

            int nIndex2a = (int)((0 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3a = (int)((2 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex2b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3b = (int)((8 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            //acNudges2[0] = new FullParabolicNudge(nIndex2a, nIndex3a, 0, -.15, 0);
            //acNudges2[1] = new FullParabolicNudge(nIndex2b, nIndex3b, 0, .15, 0);
            acNudges2[0] = new FullParabolicNudge(nIndex2b, nIndex3b, 0, .15, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2 /*null*/);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, cSpheroid, cDisk, cGas);
            // dTotalMass_kg = 1.95E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33c(string sBasePath)
        {
            // NGC 0598

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "The Extended Rotation Curve M33.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/Lb)d: .8 (Lb is total blue luminosity)

            // Fit: 3.5

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33c",
                Hf_km_s_Mpc = 4.5,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .5,
                R_DataMax_kpc = 15.3,
                R_Max_kpc = 16,
                V_Max_km_s = 160,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 118.214
            };

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 15.3;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((13 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 67, 3);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13.5, 0, 1, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, null, cDisk, cGas);
            // dTotalMass_kg = 1.19E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33d(string sBasePath)
        {
            // NGC 0598

            // Rotation disc and gas data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "The Extended Rotation Curve M33.pdf"

            // Error bars are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "The global stability of M33.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/Lb)d: .8 (Lb is total blue luminosity)

            // Fit: 4

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33d",
                Hf_km_s_Mpc = 4.5,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .2,
                R_DataMax_kpc = 19.8,
                R_Max_kpc = 20,
                V_Max_km_s = 140,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 40,
                R_Max_px = 179.2,
                V_Max_px = 143.352
            };

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            //cCurveBuilder.LoadRotationInput("dark-matter", "with-dark-matter.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 15.3;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((13 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 67, 3);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, null, cDisk, cGas);
            // dTotalMass_kg = 1.29E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33e(string sBasePath)
        {
            // NGC 0598

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MOND in Local Group.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/L)d: .58

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33e",
                Hf_km_s_Mpc = 2.3,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .2,
                R_DataMax_kpc = 18.7,
                R_Max_kpc = 19.5,
                V_Max_km_s = 160,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 107.54
            };

            // Fit: 4.5

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cSpheroid = cCurveBuilder.LoadRotationInput("bulge", "bulge.csv", 1, 1, DXF_COLOR.magenta);
            cCurveBuilder.LoadRotationInput("MOND", "MOND.csv", 1, 1, DXF_COLOR.green);

            ////////////////////////////////
            // Create the bulge and disk bodies.

            // The bulge density profile can be derived via velocity data.
            double dR2_m = cSpheroid.Data.FloatingIndex2 * AstronomicalConversions.m_per_kpc;
            ShellBody cSpheroidBody = new ShellBody(0, dR2_m, 1, 1000);
            cSpheroidBody.SetDensityCurveFromVelocityArray(cSpheroid.Data, Conversions.c_d_m_per_km, AstronomicalConversions.m_per_kpc);
            double dBulgeMass_kg = cSpheroidBody.CalculateBodyMass();
            cSpheroid.Body = cSpheroidBody;

            double dR2_kpc = 15;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex3 = (int)((14 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 14, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges /*, 62, 4*/);

            int nIndex2a = (int)((0 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3a = (int)((2 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            int nIndex2b = (int)((4 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3b = (int)((8 / dR2_kpc) * nNumDiskBodyLayers) - 1;
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            //acNudges2[0] = new FullParabolicNudge(nIndex2a, nIndex3a, 0, -.15, 0);
            //acNudges2[1] = new FullParabolicNudge(nIndex2b, nIndex3b, 0, .15, 0);
            acNudges2[0] = new FullParabolicNudge(nIndex2b, nIndex3b, 0, .15, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 16, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges2 /*null*/);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, cSpheroid, cDisk, cGas);
            // dTotalMass_kg = 1.87E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33f(string sBasePath)
        {
            // NGC 0598

            // Rotation disc and gas data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "The Extended Rotation Curve M33.pdf"

            // Error bars are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MOND in Local Group.pdf"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/Lb)d: .8 (Lb is total blue luminosity)

            // Fit: 5

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33f",
                Hf_km_s_Mpc = 3.3,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .2,
                R_DataMax_kpc = 18.7,
                R_Max_kpc = 19.5,
                V_Max_km_s = 160,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 107.54
            };

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 15.3;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((13 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 13, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 67, 3);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, null, cDisk, cGas);
            // dTotalMass_kg = 1.29E+40

            int n1 = 1;
        }

        private static void DrawCurves_M33g1(string sBasePath)
        {
            // NGC 0598

            // Rotation disc and gas data are from: "corbelli-baryons in m33.pdf"
            // "Dynamical signatures of a ΛCDM-halo and the distribution of the baryons in M 33⋆"

            // Constellation:
            // Location: Local Group
            // Distance: 970 kpc
            // Redshift (v. Heliocentric) -179 ± 1 km/s
            // Redshift (v. Galactocentric): -45 ± 5 km/s
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s
            // (M/Lb)d: 1.2-1.5

            // Fit: 4.5

            GalaxyParams cM33 = new GalaxyParams()
            {
                Name = "M33g1",
                Hf_km_s_Mpc = 3.3,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .39,
                R_DataMax_kpc = 22.6,
                R_Max_kpc = 25,
                V_Max_km_s = 150,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 104.55
            };

            cM33.Path = Path.Combine(sBasePath, cM33.Name);
            cM33.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = variable ratio
            // This block models the stellar-disk and gas as "disk" bodies
            // and then calculates the accelerations from these bodies.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cM33);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            /*
            cDisk.ComputeHfx = true;
            cGas.ComputeHfx = true;
            */
            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 18.1;
            int nNumDiskBodyLayers = 4000;
            int nIndex2 = (int)((5/*5.5*/ / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((14 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 19, 0, -3, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 68, 3);

            int nIndex4 = (int)((6 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges2 = new ArrayNudge[1];
            acNudges2[0] = new ParabolicNudge(0, nIndex4, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 20, 0, -5, 0, DensityDistribution.HalfParabola_WaterSlide, /*null*/ acNudges2 /*, 30, 8.5*/);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cM33, null, cDisk, cGas);
            double dTotalMass_sm = dTotalMass_kg / AstronomicalConstants.Sun_Mass_kg;
            // dTotalMass_kg = 1.49E+40 <- 1.4926334109274423E+40
            // dTotalMass_sm = 7.51E+9
            // dTotalStellarMass_kg = 9.14E+39
            // dTotalStellarMass_sm = 4.6E+9
        }

        private static void DrawCurves_NGC2403(string sBasePath)
        {
            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "Modified Newtonian Dynamics A Falsification of Dark Matter.pdf"
            // Location: M81 Group
            // Distance: 2.96 Mpc

            GalaxyParams cNGC2403 = new GalaxyParams()
            {
                Name = "NGC2403",
                Hf_km_s_Mpc = .93,
                R_Increment_kpc = .1,
                R_DataMin_kpc = .4,
                R_DataMax_kpc = 21.5,
                R_Max_kpc = 23,
                V_Max_km_s = 175,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 126.998
            };

            cNGC2403.Path = Path.Combine(sBasePath, cNGC2403.Name);
            cNGC2403.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = .9

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2403);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 21.5;
            int nNumDiskBodyLayers = 3500;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((18.1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, 6, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 18.1, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges, 89, 3);

            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, 3000, 1.7, 24.2, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2403, null, cDisk, cGas);
        }

        private static void DrawCurves_NGC2841(string sBasePath)
        {
            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "accelerations SPARC galaxies.pdf"
            // Constellation: Ursa Major
            // Location: Leo Spur
            // Distance: 15.5 Mpc

            GalaxyParams cNGC2841 = new GalaxyParams()
            {
                Name = "NGC2841",
                Hf_km_s_Mpc = 1.0,
                R_Increment_kpc = .1,
                R_DataMin_kpc = 3.8,
                R_DataMax_kpc = 69.8,
                R_Max_kpc = 80,
                V_Max_km_s = 350,
                R_LongTick_kpc = 10,
                V_LongTick_km_s = 50,
                R_Max_px = 280,
                V_Max_px = 217.80625

                // print
                // R_Max_px = 179.2 -> 64%
                // V_Max_px = 139.396 -> 64%
            };

            cNGC2841.Path = Path.Combine(sBasePath, cNGC2841.Name);
            cNGC2841.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC2841);
            GalaxyRotationInput cBulge = cCurveBuilder.LoadRotationInput("bulge", "bulge.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);
            GalaxyRotationInput cMOND = cCurveBuilder.LoadRotationInput("MOND", "MOND.csv", 1, 1, DXF_COLOR.green);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC2841, cBulge, cDisk, cGas);
        }

        private static void DrawCurves_NGC3198_1p1(string sBasePath)
        {
            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MOND rotation curves Cepheid-based distances.pdf"

            GalaxyParams cNGC3198 = new GalaxyParams()
            {
                Name = "NGC3198",
                Hf_km_s_Mpc = .65, //.6,
                R_Increment_kpc = .1,
                R_DataMin_kpc = 1,
                R_DataMax_kpc = 42,
                R_Max_kpc = 45,
                V_Max_km_s = 200,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 50,
                R_Max_px = 280,
                V_Max_px = 179.6672

                // print
                // R_Max_px = 179.2 -> 64%
                // V_Max_px = 114.987 -> 64%
            };

            cNGC3198.Path = Path.Combine(sBasePath, cNGC3198.Name);
            cNGC3198.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1.1

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC3198);

            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk_1p1.csv", 1, 1, DXF_COLOR.dblue);

#if DEBUG // Spot check the data load.
            {
                double dV1_km_s = cDisk.Data[1.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV1_km_s, 61.03986571803213, 6));

                double dV5_km_s = cDisk.Data[5.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV5_km_s, 99.85840294303337, 6));

                double dV10_km_s = cDisk.Data[10.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV10_km_s, 103.18570613374776, 6));

                double dV15_km_s = cDisk.Data[15.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV15_km_s, 92.37197076392599, 6));

                double dV20_km_s = cDisk.Data[20.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV20_km_s, 80.72640959642561, 6));

                double dV25_km_s = cDisk.Data[25.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV25_km_s, 67.97174736535379, 6));

                double dV30_km_s = cDisk.Data[30.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV30_km_s, 60.48531518624637, 6));

                double dV35_km_s = cDisk.Data[35.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV35_km_s, 55.77163566606774, 6));

                double dV40_km_s = cDisk.Data[40.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV40_km_s, 51.6125066776747, 6));

                double dV42_km_s = cDisk.Data[42.0];
                Debug.Assert(DebugHelpers.XDecimalSanityCheck(dV42_km_s, 50.50340561410323, 6));
            }
#endif

            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas_1p1.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 42;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((17 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .5, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            dR2_kpc = 55;
            nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, 3000, 2, 58, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC3198, null, cDisk, cGas);
        }

        private static void DrawCurves_NGC3198_1p3(string sBasePath)
        {
            // Title: NGC 3198, 12.5 Mpc, (M/Lb) disk = 1.3

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MOND rotation curves Cepheid-based distances.pdf"
            // Constellation: Ursa Major
            // Location: Leo Spur
            // Distance: 12.5 Mpc

            GalaxyParams cNGC3198 = new GalaxyParams()
            {
                Name = "NGC3198",
                NameSuffix = "_1p3",
                Hf_km_s_Mpc = .55, //.6,
                R_Increment_kpc = .1,
                R_DataMin_kpc = 1.2,
                R_DataMax_kpc = 38,
                R_Max_kpc = 40,
                V_Max_km_s = 200,
                R_LongTick_kpc = 10,
                V_LongTick_km_s = 50,
                R_Max_px = 280,
                V_Max_px = 194.9796875

                // print
                // R_Max_px = 179.2 -> 64%
                // V_Max_px = 124.787 -> 64%
            };

            cNGC3198.Path = Path.Combine(sBasePath, cNGC3198.Name);
            cNGC3198.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            // M/L = 1.3

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cNGC3198);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk_1p3.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas_1p3.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            /*
            double dR2_kpc = 42;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((17 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .5, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);
            */
            /*
            double dR2_kpc = 38;
            int nNumDiskBodyLayers = 3500;
            int nIndex2 = (int)((8 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((17 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new ParabolicNudge(0, nIndex2, .5, 0, 0);
            acNudges[1] = new ParabolicNudge(0, nIndex3, .5, 0, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);
            */

            double dR2_kpc = 38;
            int nNumDiskBodyLayers = 3500;
            int nIndex2 = (int)((17.1 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex3 = (int)((17.1 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new ParabolicNudge(0, nIndex2, 1, 0, 0);
            //acNudges[1] = new ParabolicNudge(0, nIndex3, .5, 0, 0);
            //LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 17.1, 0, -2, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            dR2_kpc = 55;
            nIndex2 = (int)((5 / dR2_kpc) * nNumDiskBodyLayers);
            acNudges[0] = new ParabolicNudge(0, nIndex2, -.5, 0, 0);
            //LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, 3000, 2, 58, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cNGC3198, null, cDisk, cGas);
        }

        private static void DrawCurves_UGC8508_LT(string sBasePath)
        {
            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "MODELS OF DWARF GALAXIES FROM LITTLE THINGS.pdf"
            // Constellation: Ursa Major
            // Location: M101 Group
            // Distance: 2.580 Mpc
            // Redshift (v. Heliocentric): 61 ± 1 km/s
            // Redshift (v. Galactocentric): 169 ± 4 km/s
            // Redshift (v. Local Group): 185 ± 8 km/s
            // Redshift (v. 3K CMB): 198 ± 10 km/s

            // Bad fit!!

            GalaxyParams cUGC8508 = new GalaxyParams()
            {
                Name = "UGC8508_LT",
                Hf_km_s_Mpc = 12, // ??
                R_Increment_kpc = .01,
                R_DataMin_kpc = .084,
                R_DataMax_kpc = 1.85,
                R_Max_kpc = 2,
                V_Max_km_s = 55,
                R_LongTick_kpc = 1,
                V_LongTick_km_s = 20,
                R_Max_px = 179.2,
                V_Max_px = 139.825
            };

            cUGC8508.Path = Path.Combine(sBasePath, cUGC8508.Name);
            cUGC8508.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cUGC8508);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("stellar-disk", "stellar-disk.csv", 1, 1, DXF_COLOR.dblue);
            GalaxyRotationInput cGas = cCurveBuilder.LoadRotationInput("gas", "gas.csv", 1, 1, DXF_COLOR.magenta);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            int nNumDiskBodyLayers = 3000;
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null);

            double dR2_kpc = 2;
            int nIndex2 = (int)((.4 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[1];
            acNudges[0] = new LinearNudge(0, nIndex2, -.7, 0);
            LayeredBody cGasBody = cGas.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 1.8, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, acNudges);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cUGC8508, null, cDisk, cGas, null, null, false);
            // dTotalMass_kg = 1.88E+38

            int n1 = 1;
        }

        private static void DrawCurves_WLM_LT(string sBasePath)
        {
            // WLM (Wolf–Lundmark–Melotte), UGCA 444

            // Rotation data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "LITTLE THINGS in 3D.pdf"

            // Constellation: Hydra
            // Location: Local Group
            // Distance: 930 ± 30 kpc
            // Redshift (v. Heliocentric): -122 ± 1 km/s
            // Redshift (v. Galactocentric): -65 ± 3 km/s
            // Redshift (v. Local Group): -16 ± 6 km/s
            // Redshift (v. 3K CMB): -457 ± 23 km/s

            // Fit: 2

            GalaxyParams cWLM = new GalaxyParams()
            {
                Name = "WLM_LT",
                Hf_km_s_Mpc = 4,
                R_Increment_kpc = .01,
                R_DataMin_kpc = .14,
                R_DataMax_kpc = 2.72,
                R_Max_kpc = 2.8, // 2.5 + this: (19.199 px / 32 px) * .5 kpc = .3 
                V_Max_km_s = 43.153, // 40 + this: (13.719 px / 43.511 px) * 10 km/s = 3.153
                R_LongTick_kpc = .5,
                V_LongTick_km_s = 10,
                R_Max_px = 179.2,
                V_Max_px = 188.631
            };

            cWLM.Path = Path.Combine(sBasePath, cWLM.Name);
            cWLM.SetDXF();

            ////////////////////////////////
            // Scenario 1.

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cWLM);
            GalaxyRotationInput cDisk = cCurveBuilder.LoadRotationInput("disk", "disk.csv", 1, 1, DXF_COLOR.dblue);

            ////////////////////////////////
            // Create the stellar-disk and gas bodies.

            double dR2_kpc = 2.72;
            int nNumDiskBodyLayers = 3000;
            int nIndex2 = (int)((1.5 / dR2_kpc) * nNumDiskBodyLayers);
            int nIndex2b = (int)((.7 / dR2_kpc) * nNumDiskBodyLayers);
            ArrayNudge[] acNudges = new ArrayNudge[2];
            acNudges[0] = new LinearNudge(0, nIndex2, -.55, 0);
            acNudges[1] = new FullParabolicNudge(0, nIndex2b, 0, -.1, 0);
            LayeredBody cDiskBody = cDisk.CreateBody(LayeredBodyType.Disk, nNumDiskBodyLayers, 0, 3, 0, 0, 0, DensityDistribution.Linear_RampDown, acNudges, 15, 2.3);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cWLM, null, cDisk, null, null, null, false);
            // dTotalMass_kg = 1.025E+39

            int n1 = 1;
        }
    }
}

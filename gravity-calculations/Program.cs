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
        static void Main(string[] args)
        {
            /*
            ShellBodyTests.CalculateDiskBodyMass();
            ShellBodyTests.CalculateDiskBodyGravity();
            ShellBodyTests.CalculateLinearMass();
            ShellBodyTests.CalculateFullParabolaDensity();
            ShellBodyTests.CalculateHalfParabolaDensity();
            ShellBodyTests.CalculateExponentialDensity();
            ShellBodyTests.CalculateShellBodyMass();
            ShellBodyTests.CalculateShellDensityCurveFromVelocityArray();
            ShellBodyTests.CalculateEarthGravityViaShells();
            ShellBodyTests.CalculateHfp();
            */

            // Chart specs: [font: Calibri], positive x is pointed rightward, positive y is downward
            // top-left coordinate (px): (30, 14) or (36, 14) with a y-axis legend
            // width: 179.2 px, (height varies)
            // title: [11.76 pt, align left, bottom-left coordinate (px): (30.1, 14.7)]

            // csv data can be extracted from images with this site: https://apps.automeris.io/wpd4/

            // Distance and CMB reshift (km/s) data are from:
            // https://ned.ipac.caltech.edu/

            // https://www.nature.com/articles/s41586-020-2794-7
            // Baryonic processes in galaxy evolution include the infall of gas onto galaxies to form
            // neutral atomic hydrogen, which is then converted to the molecular state (H2), and, finally,
            // the conversion of H2 to stars.

            // Other useful sites:
            // http://astroweb.case.edu/ssm/ASTR328/lectures.html
            // https://theplanets.org/constellations/canes-venatici-constellation/

            // From "MODELS OF DWARF GALAXIES FROM LITTLE THINGS.pdf"
            // DDO 210 (Aquarius Dwarf)
            // IC 10

            string sBasePath = @"C:\PROJECTS\SOFTWARE\modified-gravity-simulation\output";

            //goto DDO154;
            //goto DDO210_LT; //*
            //goto IC1613_LT; //?
            //goto M31; //*
            //goto M31b;
            //goto M31_extended;
            //goto M33;
            //goto M33b;
            //goto M33c;
            //goto M33d; //*
            //goto M33e;
            //goto M33f;
            //goto M33g1;
            //goto NGC2403;
            //goto NGC2841;
            //goto NGC2976;
            //goto NGC3198_1p1;
            //goto NGC3198_1p3;
            //goto NGC7793;
            //goto UGC8508_LT; //?
            //goto WLM_LT;

            //goto Calculate_Hf;
            //goto Calculate_Rf;
            //goto CheckBTFR_InLocalGroup;
            //goto DrawCurves_SphereTest_ConstantDensity;
            //goto DrawCurves_SphereTest_WaterSlideDensity;
            goto DrawHfx;
            //goto DrawHf_v_Vel;
            //goto RAR;
            //goto Calculate_Redshift;

            //DrawSPARC_galaxy(sBasePath, "CamB_NFW_flat");
            //DrawSPARC_galaxy(sBasePath, "DDO064_Burkert");
            //DrawSPARC_galaxy(sBasePath, "DDO064_Einasto_LCDM");
            //DrawSPARC_galaxy(sBasePath, "DDO154_Burkert");
            //DrawSPARC_galaxy(sBasePath, "DDO154_coreNFW_flat");
            //DrawSPARC_galaxy(sBasePath, "DDO168_Burkert");
            //DrawSPARC_galaxy(sBasePath, "DDO168_Lucky13_LCDM");
            //DrawSPARC_galaxy(sBasePath, "ESO444_G084_Burkert");
            //DrawSPARC_galaxy(sBasePath, "ESO444_G084_NFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "IC2574_Burkert");
            //DrawSPARC_galaxy(sBasePath, "IC2574_DC14_flat");
            //DrawSPARC_galaxy(sBasePath, "NGC0055_Einasto_flat");
            //DrawSPARC_galaxy(sBasePath, "NGC0247_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC0247_DC14_flat");
            //DrawSPARC_galaxy(sBasePath, "NGC0247_ML1p0");
            //DrawSPARC_galaxy(sBasePath, "NGC0247_NFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC0300_Lucky13_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC2366_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NNGC2403_combine_inputs");
            //DrawSPARC_galaxy(sBasePath, "NGC2403_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC2403_Burkert_test1");
            //DrawSPARC_galaxy(sBasePath, "NGC2403_Burkert_test2");
            //DrawSPARC_galaxy(sBasePath, "NGC2915_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC2976_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC2976_DC14_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC3109_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC3109_NFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC3741_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC4068_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC4214_Einasto_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC4214_NFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC6789_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "NGC6789_ML1p0");
            //DrawSPARC_galaxy(sBasePath, "NGC6946_Burkert");
            //DrawSPARC_galaxy(sBasePath, "NGC7793_Burkert");
            //DrawSPARC_galaxy(sBasePath, "UGC04305_Burkert");
            //DrawSPARC_galaxy(sBasePath, "UGC04483_Burkert");
            //DrawSPARC_galaxy(sBasePath, "UGC07232_Burkert");
            //DrawSPARC_galaxy(sBasePath, "UGC07524_DC14_flat");
            //DrawSPARC_galaxy(sBasePath, "UGC07559_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "UGC07577_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "UGC07866_coreNFW_LCDM");
            //DrawSPARC_galaxy(sBasePath, "UGC08490_Burkert");
            //DrawSPARC_galaxy(sBasePath, "UGC08490_DC14_LCDM");
            //DrawSPARC_galaxy(sBasePath, "UGCA444_Burkert");

            return;

        DDO154:

            DrawCurves_DDO154(sBasePath);
            return;

        DDO210_LT:

            DrawCurves_DDO210_LT(sBasePath);
            return;

        IC1613_LT:

            DrawCurves_IC1613_LT(sBasePath);
            return;

        M31:

            DrawCurves_M31(sBasePath);
            return;

        M31b:

            DrawCurves_M31b(sBasePath);
            return;

        M31_extended:

            DrawCurves_M31_extended(sBasePath);
            return;

        M33:

            DrawCurves_M33(sBasePath);
            return;

        M33b:

            DrawCurves_M33b(sBasePath);
            return;

        M33c:

            DrawCurves_M33c(sBasePath);
            return;

        M33d:

            DrawCurves_M33d(sBasePath);
            return;

        M33e:

            DrawCurves_M33e(sBasePath);
            return;

        M33f:

            DrawCurves_M33f(sBasePath);
            return;

        M33g1:

            DrawCurves_M33g1(sBasePath);
            return;

        NGC2403:

            DrawCurves_NGC2403(sBasePath);
            return;

        NGC2841:

            DrawCurves_NGC2841(sBasePath);
            return;

        NGC3198_1p1:

            DrawCurves_NGC3198_1p1(sBasePath);
            return;

        NGC3198_1p3:

            DrawCurves_NGC3198_1p3(sBasePath);
            return;

        UGC8508_LT:

            DrawCurves_UGC8508_LT(sBasePath);
            return;

        WLM_LT:

            DrawCurves_WLM_LT(sBasePath);
            return;

        Calculate_Hf:

            Calculate_Hf(sBasePath);
            return;

        Calculate_Rf:

            Calculate_Rf(sBasePath);
            return;

        Calculate_Redshift:

            Calculate_Redshift(sBasePath);
            return;

        CheckBTFR_InLocalGroup:

            CheckBTFR_InLocalGroup(sBasePath);
            return;

        DrawCurves_SphereTest_ConstantDensity:

            DrawCurves_SphereTest_ConstantDensity(sBasePath);
            return;

        DrawCurves_SphereTest_WaterSlideDensity:

            DrawCurves_SphereTest_WaterSlideDensity(sBasePath);
            return;

        DrawHfx:

            DrawCurves_Hfx(sBasePath);
            return;

        DrawHf_v_Vel:

            DrawHf_v_Vel(sBasePath);
            return;

        RAR:

            DrawRAR(sBasePath);
            return;
        }

        private static void DrawSPARC_galaxy(string sBasePath, string sGalaxy)
        {
            sBasePath += "SPARC_galaxies\\";

            switch (sGalaxy)
            {
                case "CamB_NFW_flat":
                    SPARC_galaxies.DrawCurves_CamB_NFW_flat(sBasePath);
                    break;

                case "DDO064_Burkert":
                    SPARC_galaxies.DrawCurves_DDO064_Burkert(sBasePath);
                    break;

                case "DDO064_Einasto_LCDM":
                    SPARC_galaxies.DrawCurves_DDO064_Einasto_LCDM(sBasePath);
                    break;

                case "DDO154_Burkert":
                    SPARC_galaxies.DrawCurves_DDO154_Burkert(sBasePath);
                    break;

                case "DDO154_coreNFW_flat":
                    SPARC_galaxies.DrawCurves_DDO154_coreNFW_flat(sBasePath);
                    break;

                case "DDO168_Burkert":
                    SPARC_galaxies.DrawCurves_DDO168_Burkert(sBasePath);
                    break;

                case "DDO168_Lucky13_LCDM":
                    SPARC_galaxies.DrawCurves_DDO168_Lucky13_LCDM(sBasePath);
                    break;

                case "ESO444_G084_Burkert":
                    SPARC_galaxies.DrawCurves_ESO444_G084_Burkert(sBasePath);
                    break;

                case "ESO444_G084_NFW_LCDM":
                    SPARC_galaxies.DrawCurves_ESO444_G084_NFW_LCDM(sBasePath);
                    break;

                case "IC2574_Burkert":
                    SPARC_galaxies.DrawCurves_IC2574_Burkert(sBasePath);
                    break;

                case "IC2574_DC14_flat":
                    SPARC_galaxies.DrawCurves_IC2574_DC14_flat(sBasePath);
                    break;

                case "NGC0055_Einasto_flat":
                    SPARC_galaxies.DrawCurves_NGC0055_Einasto_flat(sBasePath);
                    break;

                case "NGC0247_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC0247_coreNFW_LCDM(sBasePath);
                    break;

                case "NGC0247_DC14_flat":
                    SPARC_galaxies.DrawCurves_NGC0247_DC14_flat(sBasePath);
                    break;

                case "NGC0247_ML1p0":
                    SPARC_galaxies.DrawCurves_NGC0247_ML1p0(sBasePath);
                    break;

                case "NGC0247_NFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC0247_NFW_LCDM(sBasePath);
                    break;

                case "NGC0300_Lucky13_LCDM":
                    SPARC_galaxies.DrawCurves_NGC0300_Lucky13_LCDM(sBasePath);
                    break;

                case "NGC2366_Burkert":
                    SPARC_galaxies.DrawCurves_NGC2366_Burkert(sBasePath);
                    break;

                case "NNGC2403_combine_inputs":
                    SPARC_galaxies.NGC2403_combine_inputs(sBasePath);
                    break;

                case "NGC2403_Burkert":
                    SPARC_galaxies.DrawCurves_NGC2403_Burkert(sBasePath);
                    break;

                case "NGC2403_Burkert_test1":
                    SPARC_galaxies.DrawCurves_NGC2403_Burkert_test1(sBasePath);
                    break;

                case "NGC2403_Burkert_test2":
                    SPARC_galaxies.DrawCurves_NGC2403_Burkert_test2(sBasePath);
                    break;

                case "NGC2915_Burkert":
                    SPARC_galaxies.DrawCurves_NGC2915_Burkert(sBasePath);
                    break;

                case "NGC2976_Burkert":
                    SPARC_galaxies.DrawCurves_NGC2976_Burkert(sBasePath);
                    break;

                case "NGC2976_DC14_LCDM":
                    SPARC_galaxies.DrawCurves_NGC2976_DC14_LCDM(sBasePath);
                    break;

                case "NGC3109_Burkert":
                    SPARC_galaxies.DrawCurves_NGC3109_Burkert(sBasePath);
                    break;

                case "NGC3109_NFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC3109_NFW_LCDM(sBasePath);
                    break;

                case "NGC3741_Burkert":
                    SPARC_galaxies.DrawCurves_NGC3741_Burkert(sBasePath);
                    break;

                case "NGC4068_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC4068_coreNFW_LCDM(sBasePath);
                    break;

                case "NGC4214_Einasto_LCDM":
                    SPARC_galaxies.DrawCurves_NGC4214_Einasto_LCDM(sBasePath);
                    break;

                case "NGC4214_NFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC4214_NFW_LCDM(sBasePath);
                    break;

                case "NGC6789_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_NGC6789_coreNFW_LCDM(sBasePath);
                    break;

                case "NGC6789_ML1p0":
                    SPARC_galaxies.DrawCurves_NGC6789_ML1p0(sBasePath);
                    break;

                case "NGC6946_Burkert":
                    SPARC_galaxies.DrawCurves_NGC6946_Burkert(sBasePath);
                    break;

                case "NGC7793_Burkert":
                    SPARC_galaxies.DrawCurves_NGC7793_Burkert(sBasePath);
                    break;

                case "UGC04305_Burkert":
                    SPARC_galaxies.DrawCurves_UGC04305_Burkert(sBasePath);
                    break;

                case "UGC04483_Burkert":
                    SPARC_galaxies.DrawCurves_UGC04483_Burkert(sBasePath);
                    break;

                case "UGC07232_Burkert":
                    SPARC_galaxies.DrawCurves_UGC07232_Burkert(sBasePath);
                    break;

                case "UGC07524_DC14_flat":
                    SPARC_galaxies.DrawCurves_UGC07524_DC14_flat(sBasePath);
                    break;

                case "UGC07559_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_UGC07559_coreNFW_LCDM(sBasePath);
                    break;

                case "UGC07577_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_UGC07577_coreNFW_LCDM(sBasePath);
                    break;

                case "UGC07866_coreNFW_LCDM":
                    SPARC_galaxies.DrawCurves_UGC07866_coreNFW_LCDM(sBasePath);
                    break;

                case "UGC08490_Burkert":
                    SPARC_galaxies.DrawCurves_UGC08490_Burkert(sBasePath);
                    break;

                case "UGC08490_DC14_LCDM":
                    SPARC_galaxies.DrawCurves_UGC08490_DC14_LCDM(sBasePath);
                    break;

                case "UGCA444_Burkert":
                    SPARC_galaxies.DrawCurves_UGCA444_Burkert(sBasePath);
                    break;
            }
        }

        private static void Calculate_Hf(string sBasePath)
        {
            double dNetCurveV_km_s, dBulgeV_km_s, dDiskV_km_s, dGasV_km_s, dMass_kg, dHf_V_km_s, dVf_km_s, dHf_km_s_Mpc;

            ////////////////////////////////
            // ESO444_G084 (870 km/s)

            // Burkert
            dNetCurveV_km_s = 60;
            dDiskV_km_s = 6;
            dGasV_km_s = 14;
            dMass_kg = 4.53E+38;
            // dHf_V_km_s 58
            // dVf_km_s 5703
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, 0, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 9.7 (10)

            // DC14_LCDM
            dNetCurveV_km_s = 47;  
            dDiskV_km_s = 7;
            dGasV_km_s = 15;
            dMass_kg = 6E+38; // guestimate
            // dHf_V_km_s 44
            // dVf_km_s 354
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, 0, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 2.4

            // NFW_LCDM
            dNetCurveV_km_s = 50;
            dDiskV_km_s = 6;
            dGasV_km_s = 14;
            dMass_kg = 4.49E+38;
            // dHf_V_km_s 47.6
            // dVf_km_s 1194
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, 0, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 4.4 (4)

            ////////////////////////////////
            // M31 (582 km/s)

            // "MOND in Local Group.pdf"
            dNetCurveV_km_s = 225;
            dBulgeV_km_s = 83;
            dDiskV_km_s = 102;
            dGasV_km_s = 28;
            dMass_kg = 1.80E+41;
            // dHf_V_km_s 180.4
            // dVf_km_s 315
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, dBulgeV_km_s, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 2.27

            ////////////////////////////////
            // M33 (460 km/s)

            // e
            dNetCurveV_km_s = 114;
            dBulgeV_km_s = 10;
            dDiskV_km_s = 31;
            dGasV_km_s = 37;
            dMass_kg = 1.87E+40;
            // dHf_V_km_s 102.8
            // dVf_km_s 324
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, dBulgeV_km_s, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 2.3 (2.3)

            // f
            dNetCurveV_km_s = 111;
            dBulgeV_km_s = 0;
            dDiskV_km_s = 30;
            dGasV_km_s = 30;
            dMass_kg = 1.29E+40;
            // dHf_V_km_s 102.6
            // dVf_km_s 670
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, dBulgeV_km_s, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 3.3 (3.3)

            // Ave Hf: 2.8
            double dV2_ave_km_s;
            double dVf_ave_km_s = GetVframe_km_s(2.8, 339, out dV2_ave_km_s); // 480

            ////////////////////////////////
            // Milky Way (550 km/s)

            dHf_km_s_Mpc = Math.Sqrt(550 / AstronomicalConstants.c_km_s) * AstronomicalConstants.Ho_km_s_Mpc;
            // 2.998
            
            double dHf2_km_s_Mpc = Math.Sqrt(550 / AstronomicalConstants.c_km_s) * AstronomicalConstants.Ho2_km_s_Mpc;
            // 3.170

            ////////////////////////////////
            // NGC3109 (738 km/s)

            // Burkert
            dNetCurveV_km_s = 68;
            dDiskV_km_s = 9;
            dGasV_km_s = 19.5;
            dMass_kg = 1.03E+39;
            // dHf_V_km_s 64.5
            // dVf_km_s 2574
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, 0, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 6.5 (8)

            // NFW_LCDM
            dNetCurveV_km_s = 65;
            dDiskV_km_s = 12;
            dGasV_km_s = 20.5;
            dMass_kg = 1.13E+39; // guestimate
            // dHf_V_km_s 60.5
            // dVf_km_s 1279 (738 * 2^.5 = 1044)
            dHf_km_s_Mpc = GalaxyParams.GetHf_FromVelocities_km_s_Mpc(dNetCurveV_km_s, 0, dDiskV_km_s, dGasV_km_s, dMass_kg, out dHf_V_km_s, out dVf_km_s);
            // 4.6

            int n1 = 1;
        }

        private static void Calculate_Rf(string sBasePath)
        {
            // Only flattening curves are included here.

            // 3.0878137702 is the Hf that produces the MOND ao = 1.2E-010 m/s^2
            // An ao = 1.0E-010 m/s^2 gives an Hf = 2.573178142
            // An ao = 1.4E-010 m/s^2 gives an Hf = 3.602449399
            double Hf_ao_km_s_Mpc = GalaxyParams.Hf_ao_km_s_Mpc;

            // Ho = 70
            // ao = 1.2E-010 m/s^2
            double dV2frame_km_s;
            double dVframe_km_s = GetVframe_km_s(3.0878137702, 412.2, out dV2frame_km_s); // 583.3

            // Ho = 70
            dVframe_km_s = GetVframe_km_s(2.573178142, 286.4, out dV2frame_km_s); // 405.1

            dVframe_km_s = GetVframe_km_s(3.602449399, 561.4, out dV2frame_km_s); // 794.0

            // Ho = 74
            /*
            double dV2frame_km_s;
            double dVframe_km_s = GetVframe_km_s(3.0878137702, 369.1, out dV2frame_km_s); // 522

            dVframe_km_s = GetVframe_km_s(2.573178142, 256.3, out dV2frame_km_s); // 362.5

            dVframe_km_s = GetVframe_km_s(3.602449399, 502.4 , out dV2frame_km_s); // 710.5
            */

            // vf @ Ho = 74 (km/s)/Mpc (vf/2^.5), vf @ Ho = 70 (km/s)/Mpc (vf/2^.5)
            dVframe_km_s = GetVframe_km_s(1.0, 38.7, out dV2frame_km_s); // 54.7 (38.7), 61.2 (43.3)
            dVframe_km_s = GetVframe_km_s(1.5, 87.1, out dV2frame_km_s); // 123.2 (87.1), 137.7 (97.4)
            dVframe_km_s = GetVframe_km_s(2.0, 154.9, out dV2frame_km_s); // 219.0 (154.9), 244.7 (173.0)
            dVframe_km_s = GetVframe_km_s(2.5, 242.1, out dV2frame_km_s); // 342.2 (242.0), 382.4 (270.4)
            dVframe_km_s = GetVframe_km_s(3.0, 348.4, out dV2frame_km_s); // 492.7 (348.4), 550.6 (389.3)
            dVframe_km_s = GetVframe_km_s(3.5, 474.2, out dV2frame_km_s); // 670.6 (478.2), 749.5 (530.0)
            dVframe_km_s = GetVframe_km_s(4.0, 619.4, out dV2frame_km_s); // 876.0 (619.4), 979.0 (692.3)
            dVframe_km_s = GetVframe_km_s(4.5, 784.2, out dV2frame_km_s); // 1109 (784.2), 1238.9 (876.0)
            dVframe_km_s = GetVframe_km_s(5.0, 968.0, out dV2frame_km_s); // 1369 (968.0), 1530.0 (1082)


            double dRf2_Mpc = 30;

            // DDO 154 Burkert (639 km/s)
            double dMass_kg = 5.23E+38;
            double dVr_km_s = 50;
            double dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); //
            double dNetSpeed_km_s;
            double dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 0, 3, 15, out dNetSpeed_km_s); // 35.6, 40.2
            double dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 1.2
            double dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 35.6
            double dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 3.5); // Hf is from curve fit. 46.7
            double dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 13.3
            dVframe_km_s = GetVframe_km_s(3.5, 639, out dV2frame_km_s); // 749, 904

            // DDO 154 coreNFW_flat (639 km/s)
            dMass_kg = 4.75E+38;
            dVr_km_s = 50;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 9.85
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 0, 3, 15, out dNetSpeed_km_s); // 34.5, 37.7
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 1.15 (4.0)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 34.5
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 4.0); // Hf is from curve fit. 47.1
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 11.8
            dVframe_km_s = GetVframe_km_s(4.0, 639, out dV2frame_km_s); // 978, 904

            // ESO444_G084 Burkert (870 km/s)
            dMass_kg = 4.53E+38;
            dVr_km_s = 60;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 5.44
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 0, 6, 14, out dNetSpeed_km_s); // 34.0, 37.2
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 1.13 (10.0)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 34.0
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 10.0); // Hf is from curve fit. 58.5
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 5.86
            dVframe_km_s = GetVframe_km_s(10.0, 870, out dV2frame_km_s); // 6118, 1230
            dVframe_km_s = GetVframe_km_s(8.0, 870, out dV2frame_km_s); // 3915, 1230
            dVframe_km_s = GetVframe_km_s(6.0, 870, out dV2frame_km_s); // 2203, 1230
            dVframe_km_s = GetVframe_km_s(5.0, 870, out dV2frame_km_s); // 1530, 1230; with Ho = 74, Vf = 1369

            // M31 (582 km/s)
            dMass_kg = 1.80E+41;
            dVr_km_s = 250;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 29.9
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 45, 85, 45, out dNetSpeed_km_s); // 249.7, 271.3
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 8.32 (6.0)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 250.0
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 6.0); // Hf is from curve fit. 230.0
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 38.3
            dVframe_km_s = GetVframe_km_s(10.0, 870, out dV2frame_km_s); //

            // M33d
            dMass_kg = 1.29E+40;
            dVr_km_s = 120;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 19.4
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 0, 28, 28, out dNetSpeed_km_s); // 103.7, 111.0
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 3.55 (4.5)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 103.7
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 4.5); // Hf is from curve fit. 110.8
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 24.6

            // M33e (460 km/s)
            dMass_kg = 1.87E+40;
            dVr_km_s = 112;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 34.5
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 10, 31, 28, out dNetSpeed_km_s); // 117.4, 125.0
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 3.91 (2.3)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 117.4
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 2.3); // Hf is from curve fit. 102.8
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 44.7

            double dVflatFrom_ao_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, Hf_ao_km_s_Mpc); // Vflat at ao. 110.6
            double dVnet_km_s = Math.Sqrt(10 * 10 + 31 * 31 + 28 * 28 + 110.6 * 110.6);

            // NGC0055 (115 km/s)
            dMass_kg = 8.97E+39;
            dVr_km_s = 88;
            dRf_Mpc = GetRf_Mpc(dMass_kg, dVr_km_s); // 34.1
            dVr2_km_s = GetVr_From_Rf_km_s(dMass_kg, dRf2_Mpc, 0, 34, 34, out dNetSpeed_km_s); // 91.9, 103.7
            dHf_km_s_Mpc = dVr2_km_s / dRf2_Mpc; // 3.06 (1.3)
            dVflat_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, dHf_km_s_Mpc); // 91.9
            dVflatFromCurveFit_km_s = AstronomyHelpers.GetVflat_km_s(dMass_kg, 1.3); // Hf is from curve fit. 74.2
            dRfFromCurveFit_Mpc = GetRf_Mpc(dMass_kg, dVflatFromCurveFit_km_s); // 57.0

            dVnet_km_s = Math.Sqrt(34 * 34 + 34 * 34 + 28 * 28 + 74.2 * 74.2);

            int n1 = 1;
        }

        public static void CheckBTFR_InLocalGroup(string sBasePath)
        {
            // Data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "BTFR LOCAL GROUP 2021.pdf"

            double dBTFR_km_s_sm = .379;
            double dBTFR_km_s_kg = Math.Pow(dBTFR_km_s_sm, 4) / AstronomicalConversions.kg_per_sm;
            double d1 = 4 * AstronomicalConstants.c_km_s * AstronomicalConstants.G_km_kg_s;
            double Hf_km_s_Mpc = (dBTFR_km_s_kg / d1) * AstronomicalConversions.km_per_Mpc;
            // Hf_km_s_Mpc = 4.00

            ////////////////////////////////
            // Rotationally-supported Local Group Galaxies

            // M31
            double dM31_Ms_sm = 135E9;
            double dM31_Mg_sm = 5.46E9;
            double dM31_Mt_sm = dM31_Ms_sm + dM31_Mg_sm;
            double dM31_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dM31_Mt_sm, .25);
            // 232.0 -> 229.5 ± 2.2
            // Redshift (v. Local Group): -31 ± 16 km/s
            // Redshift (v. 3K CMB): -582 ± 20 km/s

            // MW
            double dMW_Ms_sm = 60.8E9;
            double dMW_Mg_sm = 12.2E9;
            double dMW_Mt_sm = dMW_Ms_sm + dMW_Mg_sm;
            double dMW_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dMW_Mt_sm, .25);
            // 197.0 -> 197.9 ± 1.9
            // Redshift (v. Local Group):
            // Redshift (v. 3K CMB): 552 ± 6 km/s

            // M33
            double dM33_Ms_sm = 5.5E9;
            double dM33_Mg_sm = 3.1E9;
            double dM33_Mt_sm = dM33_Ms_sm + dM33_Mg_sm;
            double dM33_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dM33_Mt_sm, .25);
            // 115.4 -> 118.0 ± 1.1
            // Redshift (v. Local Group): 37 ± 13 km/s
            // Redshift (v. 3K CMB): -460 ± 20 km/s

            // LMC
            double dLMC_Ms_sm = 2.0E9;
            double dLMC_Mg_sm = 0.60E9;
            double dLMC_Mt_sm = dLMC_Ms_sm + dLMC_Mg_sm;
            double dLMC_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dLMC_Mt_sm, .25);
            // 85.6 -> 78.9 ± 7.5
            // Redshift (v. Local Group): 27 ± 15 km/s
            // Redshift (v. 3K CMB): 327 ± 4 km / s
            // Distance to MW 48-52 kpc.
            //
            // It is roughly one-hundredth the mass of the Milky Way and is the fourth-largest 
            // galaxy in the Local Group, after the Andromeda Galaxy (M31), the Milky Way,
            // and the Triangulum Galaxy (M33).
            //
            // LMC—the LMC is clearly interacting with the Milky Way,
            // so one may not expect it to retain equilibrium kinematics.
            // Nevertheless, it falls close to the BTFR(for examples of other
            // perturbed systems; see Verheijen 2001).

            // SMC
            double dSMC_Ms_sm = 0.31E9;
            double dSMC_Mg_sm = 0.54E9;
            double dSMC_Mt_sm = dSMC_Ms_sm + dSMC_Mg_sm;
            double dSMC_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dSMC_Mt_sm, .25);
            // 64.7 -> 56 ±5
            // Redshift (v. Local Group): -35 ± 11 km/s
            // Redshift (v. 3K CMB): 90 ± 4 km/s
            // Distance to MW 59-63 kpc.

            // NGC6822
            double dNGC6822_Ms_sm = 0.234E9;
            double dNGC6822_Mg_sm = 0.20E9;
            double dNGC6822_Mt_sm = dNGC6822_Ms_sm + dNGC6822_Mg_sm;
            double dNGC6822_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dNGC6822_Mt_sm, .25);
            // 54.7 -> 55 ±3
            // Redshift (v. Local Group): 66 ± 8 km/s
            // Redshift (v. 3K CMB): -264 ± 15 km/s

            // WLM
            double dWLM_Ms_sm = 0.0163E9;
            double dWLM_Mg_sm = 0.077E9;
            double dWLM_Mt_sm = dWLM_Ms_sm + dWLM_Mg_sm;
            double dWLM_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dWLM_Mt_sm, .25);
            // 37.2 -> 38.7 ± 3.4
            // Redshift (v. Local Group): -24 ± 6 km/s
            // Redshift (v. 3K CMB): -465 ± 23 km/s

            // DDO216
            double dDDO216_Ms_sm = 0.0152E9;
            double dDDO216_Mg_sm = 0.00816E9;
            double dDDO216_Mt_sm = dDDO216_Ms_sm + dDDO216_Mg_sm;
            double dDDO216_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dDDO216_Mt_sm, .25);
            // 26.3 -> 13.6 ± 5.5
            // Redshift (v. Local Group): 60 ± 15 km/s
            // Redshift (v. 3K CMB): -550 ± 26 km/s

            // DDO210
            double dDDO210_Ms_sm = 0.00068E9;
            double dDDO210_Mg_sm = 0.00274E9;
            double dDDO210_Mt_sm = dDDO210_Ms_sm + dDDO210_Mg_sm;
            double dDDO210_Vo_km_s = dBTFR_km_s_sm * Math.Pow(dDDO210_Mt_sm, .25);
            // 16.3 -> 16.4 ± 9.5
            // Redshift (v. Local Group): 10 ± 9 km/s
            // Redshift (v. 3K CMB): -419 ± 20 km/s

            ////////////////////////////////
            // Local Group Dwarf Spheroidals

            // NGC 205, M110
            // Redshift (v. Local Group): 27 ± 16 km/s
            // Redshift (v. 3K CMB): -526 ± 20 km/s

            // NGC 205 is excluded from the fit as it is not entirely in the low-acceleration
            // regime of dark matter domination. Intriguingly, it is already in
            // good agreement with the BTFR for bc = 3, as we might
            // expect if the stars are important to the mass budget.

            // About half of the Andromeda's satellite galaxies are orbiting it along a
            // highly flattened plane, with 14 out of 16 following the same sense of rotation.
            // One theory proposes that these 16 once belonged to a subhalo surrounding M110,
            // then the group was broken up by tidal forces during a close encounter with Andromeda.

            int n1 = 1;
        }

        private static void Check_AU_Increase(string sBasePath)
        {
            // Check to see that the Earth travels around the sun in a year.
            double r_km = AstronomicalConstants.AU_km;
            double vo_sqd_km_s = (AstronomicalConstants.Sun_Mass_kg * AstronomicalConstants.G_km_kg_s) / r_km;
            double vo_km_s = Math.Sqrt(vo_sqd_km_s);
            double t_yr = (2 * Math.PI * r_km / vo_km_s) / Conversions.c_d_s_per_yr;
            Debug.Assert(DebugHelpers.ThreeDecimalSanityCheck(t_yr, 1.000));

            double dHf_s = 4 / AstronomicalConversions.km_per_Mpc; // 70/4 = 17.5
            double dMt_kg = AstronomicalConstants.Sun_Mass_kg + AstronomicalConstants.Earth_Mass_kg;
            double dEc_km = (dMt_kg * AstronomicalConstants.G_km_kg_s) / (AstronomicalConstants.c_km_s * dHf_s);
            double dEc_srt_km = Math.Sqrt(dEc_km);

            double d2 = (2 / r_km) * dEc_srt_km + 4;
            double v_km_s = r_km * AstronomicalConstants.Ho_km_s_km / d2;
            double v_cm_yr = v_km_s * Conversions.c_d_m_per_km * Conversions.c_d_cm_per_m * Conversions.c_d_s_per_yr;
        }

        private static void Calculate_Redshift(string sBasePath)
        {
            // Baryon density / critical density: 0.0463.
            // Current baryon density (proton masses / cm^3): 2.542E-7
            double dBaryonDensity_kg_cm = 2.542E-7 * Constants.ProtonMass_kg;
            double dBaryonDensity_kg_m = dBaryonDensity_kg_cm * 100 * 100 * 100;

            double dHubbleRadius_Gpc = AstronomicalConstants.HubbleRadius_Mpc / 1000;
            // dHubbleRadius_Gpc = 4.283

            double dHubbleRadius_m = AstronomicalConstants.HubbleRadius_Mpc * AstronomicalConversions.km_per_Mpc * 1000;
            
            double dHubbleVolume_m = MathHelpers.GetVolumeOfSphere(dHubbleRadius_m);
            
            double dUnviverseMass_kg = dBaryonDensity_kg_m * dHubbleVolume_m;

            double dUniverseVolume_m = 3.566E80;
            double dUniverseRadius_m = MathHelpers.GetSphereRadiusFromVolume(dUniverseVolume_m);
            double dUniverseRadius_Gpc = ((dUniverseRadius_m / 1000) / AstronomicalConversions.km_per_Mpc) / 1000;
            // dUniverseRadius_Gpc = 14.257
            // The current horizon distance is ~ 14.6 Gpc (Frpm: 24-Inflation.pdf)

            double dUnviverseMass2_kg = dUniverseVolume_m * dBaryonDensity_kg_m;

            double dRatio = dUnviverseMass2_kg / dUnviverseMass_kg;

            ////////////////////////////////
            // CMB.

            // From "24-Inflation.pdf":
            // At the time of the CMB, the horizon scale was about 0.25 Mpc.
            // The current horizon distance is ~14.6 Gpc, so the observable universe at a redshift
            // of z = 1100 was 14.6 Gpc / 1100 ~13.2 Mpc in size - much larger than the horizon.

            double dR_CMB_Mpc, dLBT_CMB_Gyr;
            double dCMB_v_over_c = AstronomyHelpers.Get_v_over_c_From_z_1(1100, out dR_CMB_Mpc, out dLBT_CMB_Gyr);
            double t_CMB_Gyr = AstronomicalConstants.HubbleTime_Gyr - dLBT_CMB_Gyr;
            // dCMB_v_over_c = .993
            // dR_CMB_Mpc = 4251
            // dLBT_CMB_Gyr = 13.8646
            // t_CMB_Gyr = .104 (= 104 Myr)

            ////////////////////////////////

            double dR_z_2p5_Mpc, dLBT_z_2p5_Gyr;
            double v_over_c_z_2p5 = AstronomyHelpers.Get_v_over_c_From_z_1(2.5, out dR_z_2p5_Mpc, out dLBT_z_2p5_Gyr);
            double t_z_2p5_Gyr = AstronomicalConstants.HubbleTime_Gyr - dLBT_z_2p5_Gyr;
            double H_z_2p5_km_s_Mpc = AstronomyHelpers.Get_H_from_t_km_s_Mpc(t_z_2p5_Gyr);
            // H_z_2p5_km_s_Mpc = 190

            // http://hyperphysics.phy-astr.gsu.edu/hbase/Astro/redshf.html#c3

            // Red Shift of Galaxy 8C1435 + 635
            // Reported in November 1994 in Monthly Notices of the Royal Astronomical Society
            // is a galaxy with a measured red shift of z = 4.25, a new record.This value for
            // the z parameter corresponds to a recession speed of .93c.The galaxy 8C1435 + 635
            // was observed in a systematic search for faint, radio - emitting galaxies carried
            // out by a team at Leiden Observatory led by George Miley. After the discovery, the
            // optical spectra was observed by the William Hershel Telescope in La Palma,
            // Canary Islands.Two emission lines of ionized carbon and hydrogen were measured
            // to obtain the red shift.This red shift corresponds to a distance of about 13
            // billion light years if one uses the current WMAP value of 71km / s / mpc for
            // the Hubble parameter is used.

            // The above scenario with z-relation #1.
            double r_Mpc, LBT_Gyr;
            double v_over_c = AstronomyHelpers.Get_v_over_c_From_z_1(4.25, out r_Mpc, out LBT_Gyr);
            // v_over_c = .724
            // LBT_Gyr = 10.11

            ////////////////////////////////
            // Limit z -> infinity.

            double dObjectDia_kpc = 10;
            double dTheta_rad = (dObjectDia_kpc / 1000) * AstronomicalConstants.Ho_km_s_Mpc / AstronomicalConstants.c_km_s;
            double dTheta_arcsec = dTheta_rad * AstronomicalConversions.arcsec_per_rad;

            ////////////////////////////////
            // The linear z relationship: z = r * Ho / c = v / c.

            ////////////////////////////////
            // z relation #1

            // PDF #1: 10 kpc angular-diameter data from "Cosmological_Model_Tests_with_JWST.pdf"
            // PDF #2: 4.5 kpc angular-diameter data from "jwst_data_suggest_new_cosmology.pdf"

            double dR_p1_Mpc;
            double dLBT_p1_Gyr;
            double z_p1 = AstronomyHelpers.Get_z_from_v_over_c_1(.1, out dR_p1_Mpc, out dLBT_p1_Gyr);
            double dTheta_p1_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p1_Mpc, 10);
            double H_p1_km_s_Mpc = AstronomyHelpers.Get_H_from_t_km_s_Mpc(AstronomicalConstants.HubbleTime_Gyr - dLBT_p1_Gyr);
            // z_p1_linear = .1
            // z_p1 = .117
            // dLBT_p1_Gyr = 1.40
            // dTheta_p1_arcsec = 4.82
            // H_p1_km_s_Mpc = 77.8

            double dR_p2_Mpc;
            double dLBT_p2_Gyr;
            double z_p2 = AstronomyHelpers.Get_z_from_v_over_c_1(.2, out dR_p2_Mpc, out dLBT_p2_Gyr);
            double dTheta_p2_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p2_Mpc, 10);
            // z_p2_linear = .2
            // z_p2 = .276
            // dLBT_p2_Gyr = 2.79
            // dTheta_p2_arcsec = 2.41

            double dR_p3_Mpc;
            double dLBT_p3_Gyr;
            double z_p3 = AstronomyHelpers.Get_z_from_v_over_c_1(.3, out dR_p3_Mpc, out dLBT_p3_Gyr);
            double dTheta_p3_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p3_Mpc, 10);
            // z_p3_linear = .3
            // z_p3 = .498
            // dLBT_p3_Gyr = 4.19
            // dTheta_p3_arcsec = 1.61

            double dR_p4_Mpc;
            double dLBT_p4_Gyr;
            double z_p4 = AstronomyHelpers.Get_z_from_v_over_c_1(.4, out dR_p4_Mpc, out dLBT_p4_Gyr);
            double dTheta_p4_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p4_Mpc, 10);
            // z_p4_linear = .4
            // z_p4 = .818
            // dLBT_p4_Gyr = 5.59
            // dTheta_p4_arcsec = 1.20

            v_over_c = AstronomyHelpers.Get_v_over_c_From_z_1(1, out r_Mpc, out LBT_Gyr);
            // v_over_c = .442

            double dR_z_1_Mpc;
            double dLBT_z_1_Gyr;
            double z_1 = AstronomyHelpers.Get_z_from_v_over_c_1(v_over_c, out dR_z_1_Mpc, out dLBT_z_1_Gyr);
            double dTheta_z1_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_1_Mpc, 10);
            // z_1_linear = .835
            // z_1 = 1
            // dLBT_1_Gyr = 6.18
            // dTheta_1_arcsec = 1.09 <- Good agreement with PDF #1.

            double dTheta_z_1_4p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_1_Mpc, 4.5);
            // dTheta_z_1_4p5_arcsec = .490 <- Good agreement with PDF #2.

            double dR_p5_Mpc;
            double dLBT_p5_Gyr;
            double z_p5 = AstronomyHelpers.Get_z_from_v_over_c_1(.5, out dR_p5_Mpc, out dLBT_p5_Gyr);
            double dTheta_p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p5_Mpc, 10);
            // z_p5_linear = .5
            // z_p5 = 1.31
            // dLBT_p5_Gyr = 6.98
            // dTheta_p5_arcsec = .963

            double dR_p6_Mpc;
            double dLBT_p6_Gyr;
            double z_p6 = AstronomyHelpers.Get_z_from_v_over_c_1(.6, out dR_p6_Mpc, out dLBT_p6_Gyr);
            double dTheta_p6_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p6_Mpc, 10);
            // z_p6_linear = .6
            // z_p6 = 2.13
            // dLBT_p6_Gyr = 8.38
            // dTheta_p6_arcsec = .803

            v_over_c = AstronomyHelpers.Get_v_over_c_From_z_1(3, out r_Mpc, out LBT_Gyr);
            // v_over_c = .665

            double dR_z_3_Mpc;
            double dLBT_z_3_Gyr;
            double z_3 = AstronomyHelpers.Get_z_from_v_over_c_1(v_over_c, out dR_z_3_Mpc, out dLBT_z_3_Gyr);
            double dTheta_z3_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_3_Mpc, 10);
            // z_3_linear = .665
            // z_3 = 3
            // dLBT_z_3_Gyr = 9.29
            // dTheta_z_3_arcsec = .724

            double dTheta_z_3_4p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_3_Mpc, 4.5);
            // dTheta_z_3_4p5_arcsec = .326 <- Good agreement with PDF #2.

            double dR_p7_Mpc;
            double dLBT_p7_Gyr;
            double z_p7 = AstronomyHelpers.Get_z_from_v_over_c_1(.7, out dR_p7_Mpc, out dLBT_p7_Gyr);
            double dTheta_p7_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p7_Mpc, 10);
            // z_p7_linear = .7
            // z_p7 = 3.67
            // dLBT_p7_Gyr = 9.78
            // dTheta_p7_arcsec = .688

            double dR_Mpc_p75;
            double dLBT_p75_Gyr;
            double z_p75 = AstronomyHelpers.Get_z_from_v_over_c_1(.75, out dR_Mpc_p75, out dLBT_p75_Gyr);
            double dTheta_p75_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_Mpc_p75, 10);
            // z_p75_linear = .75
            // z_p75 = 5.05
            // dLBT_p75_Gyr = 10.48
            // dTheta_p75_arcsec = .642

            double dR_p8_Mpc;
            double dLBT_p8_Gyr;
            double z_p8 = AstronomyHelpers.Get_z_from_v_over_c_1(.8, out dR_p8_Mpc, out dLBT_p8_Gyr);
            double dTheta_p8_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p8_Mpc, 10);
            // z_p8_linear = .8
            // z_p8 = 7.33
            // dLBT_p8_Gyr = 11.17
            // dTheta_p8_arcsec = .602

            v_over_c = AstronomyHelpers.Get_v_over_c_From_z_1(10, out r_Mpc, out LBT_Gyr);
            // v_over_c = .835

            double dR_z_10_Mpc;
            double dLBT_z_10_Gyr;
            double z_10 = AstronomyHelpers.Get_z_from_v_over_c_1(v_over_c, out dR_z_10_Mpc, out dLBT_z_10_Gyr);
            double dTheta_z_10_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_10_Mpc, 10);
            // z_10_linear = .835
            // z_10 = 10
            // dLBT_z_10_Gyr = 11.66
            // dTheta_z_10_arcsec = .577 <- Fair agreement with PDF #1.

            double dTheta_z_10_4p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_z_10_Mpc, 4.5);
            // dTheta_z_10_4p5_arcsec = .260 <- Good agreement with PDF #2.

            double dR_p9_Mpc;
            double dLBT_p9_Gyr;
            double z_p9 = AstronomyHelpers.Get_z_from_v_over_c_1(.9, out dR_p9_Mpc, out dLBT_p9_Gyr);
            double dTheta_p9_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p9_Mpc, 10);
            double H_p9_km_s_Mpc = AstronomyHelpers.Get_H_from_t_km_s_Mpc(AstronomicalConstants.HubbleTime_Gyr - dLBT_p9_Gyr);
            // z_p9_linear = .9
            // z_p9 = 21.9
            // dLBT_p9_Gyr = 12.57
            // dTheta_p9_arcsec = .535
            // H_p9_km_s_Mpc = 700
            // Hl acceleration is sqrt(10) = 3.16X than the case @ Ho = 70.

            ////////////////////////////////
            // z relation 0 (baseline).

            z_p1 = AstronomyHelpers.Get_z_from_v_over_c_0(.1, out dR_p1_Mpc, out dLBT_p1_Gyr);
            dTheta_p1_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p1_Mpc, 10);
            // z_p1_linear = .1
            // z_p1 = .111
            // dLBT_p1_Gyr = 1.40
            // dTheta_p1_arcsec = 4.82

            z_p2 = AstronomyHelpers.Get_z_from_v_over_c_0(.2, out dR_p2_Mpc, out dLBT_p2_Gyr);
            dTheta_p2_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p2_Mpc, 10);
            // z_p2_linear = .2
            // z_p2 = .25
            // dLBT_p2_Gyr = 2.80
            // dTheta_p2_arcsec = 2.41

            z_p3 = AstronomyHelpers.Get_z_from_v_over_c_0(.3, out dR_p3_Mpc, out dLBT_p3_Gyr);
            dTheta_p3_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p3_Mpc, 10);
            // z_p3_linear = .3
            // z_p3 = .429
            // dLBT_p3_Gyr = 4.19
            // dTheta_p3_arcsec = 1.61

            z_p4 = AstronomyHelpers.Get_z_from_v_over_c_0(.4, out dR_p4_Mpc, out dLBT_p4_Gyr);
            dTheta_p4_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p4_Mpc, 10);
            // z_p4_linear = .4
            // z_p4 = .667
            // dLBT_p4_Gyr = 5.59
            // dTheta_p4_arcsec = 1.20

            z_p5 = AstronomyHelpers.Get_z_from_v_over_c_0(.5, out dR_p5_Mpc, out dLBT_p5_Gyr);
            dTheta_p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p5_Mpc, 10);
            // z_p5_linear = .5
            // z_p5 = 1
            // dLBT_p5_Gyr = 6.98
            // dTheta_p5_arcsec = .963

            z_p6 = AstronomyHelpers.Get_z_from_v_over_c_0(.6, out dR_p6_Mpc, out dLBT_p6_Gyr);
            dTheta_p6_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p6_Mpc, 10);
            // z_p6_linear = .6
            // z_p6 = 1.5
            // dLBT_p6_Gyr = 8.38
            // dTheta_p6_arcsec = .803

            z_p7 = AstronomyHelpers.Get_z_from_v_over_c_0(.7, out dR_p7_Mpc, out dLBT_p7_Gyr);
            dTheta_p7_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p7_Mpc, 10);
            // z_p7_linear = .7
            // z_p7 = 2.33
            // dLBT_p7_Gyr = 9.78
            // dTheta_p7_arcsec = .688

            z_p75 = AstronomyHelpers.Get_z_from_v_over_c_0(.75, out dR_Mpc_p75, out dLBT_p75_Gyr);
            dTheta_p75_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_Mpc_p75, 10);
            // z_p75_linear = .75
            // z_p75 = 3
            // dLBT_p75_Gyr = 10.48
            // dTheta_p75_arcsec = .642

            z_p8 = AstronomyHelpers.Get_z_from_v_over_c_0(.8, out dR_p8_Mpc, out dLBT_p8_Gyr);
            dTheta_p8_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p8_Mpc, 10);
            // z_p8_linear = .8
            // z_p8 = 4
            // dLBT_p8_Gyr = 11.17
            // dTheta_p8_arcsec = .602

            z_p9 = AstronomyHelpers.Get_z_from_v_over_c_0(.9, out dR_p9_Mpc, out dLBT_p9_Gyr);
            dTheta_p9_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p9_Mpc, 10);
            // z_p9_linear = .9
            // z_p9 = 9
            // dLBT_p9_Gyr = 12.57
            // dTheta_p9_arcsec = .535

            ////////////////////////////////
            // z relation #3

            z_p1 = AstronomyHelpers.Get_z_from_v_over_c_3(.1, out dR_p1_Mpc, out dLBT_p1_Gyr);
            dTheta_p1_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p1_Mpc, 10);
            // z_p1_linear = .1
            // z_p1 = .228
            // dLBT_p1_Gyr = 1.40
            // dTheta_p1_arcsec = 4.82

            z_p2 = AstronomyHelpers.Get_z_from_v_over_c_3(.2, out dR_p2_Mpc, out dLBT_p2_Gyr);
            dTheta_p2_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p2_Mpc, 10);
            // z_p2_linear = .2
            // z_p2 = .531
            // dLBT_p2_Gyr = 2.79
            // dTheta_p2_arcsec = 2.41

            z_p3 = AstronomyHelpers.Get_z_from_v_over_c_3(.3, out dR_p3_Mpc, out dLBT_p3_Gyr);
            dTheta_p3_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p3_Mpc, 10);
            // z_p3_linear = .3
            // z_p3 = .947
            // dLBT_p3_Gyr = 4.19
            // dTheta_p3_arcsec = 1.61

            z_p4 = AstronomyHelpers.Get_z_from_v_over_c_3(.4, out dR_p4_Mpc, out dLBT_p4_Gyr);
            dTheta_p4_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p4_Mpc, 10);
            // z_p4_linear = .4
            // z_p4 = 1.55
            // dLBT_p4_Gyr = 5.59
            // dTheta_p4_arcsec = 1.20

            z_p5 = AstronomyHelpers.Get_z_from_v_over_c_3(.5, out dR_p5_Mpc, out dLBT_p5_Gyr);
            dTheta_p5_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p5_Mpc, 10);
            // z_p5_linear = .5
            // z_p5 = 2.46
            // dLBT_p5_Gyr = 6.98
            // dTheta_p5_arcsec = .963

            z_p6 = AstronomyHelpers.Get_z_from_v_over_c_3(.6, out dR_p6_Mpc, out dLBT_p6_Gyr);
            dTheta_p6_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p6_Mpc, 10);
            // z_p6_linear = .6
            // z_p6 = 4
            // dLBT_p6_Gyr = 8.38
            // dTheta_p6_arcsec = .803

            z_p7 = AstronomyHelpers.Get_z_from_v_over_c_3(.7, out dR_p7_Mpc, out dLBT_p7_Gyr);
            dTheta_p7_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p7_Mpc, 10);
            // z_p7_linear = .7
            // z_p7 = 6.93
            // dLBT_p7_Gyr = 9.78
            // dTheta_p7_arcsec = .688

            z_p75 = AstronomyHelpers.Get_z_from_v_over_c_3(.75, out dR_Mpc_p75, out dLBT_p75_Gyr);
            dTheta_p75_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_Mpc_p75, 10);
            // z_p75_linear = .75
            // z_p75 = 9.58
            // dLBT_p75_Gyr = 10.48
            // dTheta_p75_arcsec = .642

            z_p8 = AstronomyHelpers.Get_z_from_v_over_c_3(.8, out dR_p8_Mpc, out dLBT_p8_Gyr);
            dTheta_p8_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p8_Mpc, 10);
            // z_p8_linear = .8
            // z_p8 = 14
            // dLBT_p8_Gyr = 11.17
            // dTheta_p8_arcsec = .602

            z_p9 = AstronomyHelpers.Get_z_from_v_over_c_3(.9, out dR_p9_Mpc, out dLBT_p9_Gyr);
            dTheta_p9_arcsec = AstronomyHelpers.GetAngularDiameterFromDistance_arcsec(dR_p9_Mpc, 10);
            // z_p9_linear = .9
            // z_p9 = 42.6
            // dLBT_p9_Gyr = 12.57
            // dTheta_p9_arcsec = .535

            int n1 = 0;
        }

        private static void DrawCurves_Hfx(string sBasePath)
        {
            // At HF = 3 and dTotalMass_kg = 1.67E41:
            // point 1: log(R kpc): 2 -> 100 kpc, V (km/s): 200
            // point 2: log(R kpc): 3.45 -> 2800 kpc, V (km/s): 180
            // slope: (180 - 200) / (2800 - 100) = -7.4 km/s/Mpc

            double x_Mpc = AstronomyHelpers.GetZeroHfxDistance_Mpc(3);
            // x_Mpc = 7.87

            double Hfx1 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 1000); 
            double Hfx2 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 2000);
            double Hfx3 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 3000);
            double Hfx4 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 4000);
            double Hfx5 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 5000);
            double Hfx6 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 6000);
            double Hfx7 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 7000);
            double Hfx7p87 = AstronomyHelpers.GetHfxFromHf_km_s_Mpc(3, 7870);
            //   1,    2,    3,    4,    5,    6,   7, 7.87
            // 2.8, 2.59, 2.36, 2.10, 1.81, 1.46, .96,    0

            Hfx cHfx = new Hfx()
            {
                Name = "Hfx",
                R_DataMax_kpc = 10000,
                R_Max_kpc = 10000,
                V_Max_km_s = 300,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 108
                /*
                R_Max_px = 500,
                V_Max_px = 300
                */
            };

            cHfx.Path = Path.Combine(sBasePath, cHfx.Name);
            cHfx.SetDXF();

            //double[] Hf_array_km_s_Mpc = new double[] { 1, 2, 3 };
            double[] Hf_array_km_s_Mpc = new double[] { 3 };

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cHfx);
            GalaxyRotationInput cSphere = cCurveBuilder.GetTestInputObject();
            cSphere.ComputeHfx = true;
            LayeredBody cSphereBody = cSphere.CreateBody(LayeredBodyType.Shell, 100, 0, 20, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null, 148, 50);

            double dTotalMass_kg = cHfx.Draw(Hf_array_km_s_Mpc, cSphere);
            // dTotalMass_kg = 1.67E41
            // v_flat_km_s = 190 km/s

            int n1 = 1;
        }

        private static void DrawCurves_SphereTest_ConstantDensity(string sBasePath)
        {
            GalaxyParams cSphereTest = new GalaxyParams()
            {
                Name = "SphereTest_ConstantDensity",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = 1,
                R_DataMin_kpc = 0,
                R_DataMax_kpc = 190,
                R_Max_kpc = 200,
                V_Max_km_s = 150,
                R_LongTick_kpc = 5,
                V_LongTick_km_s = 40,
                R_Max_px = 500,
                V_Max_px = 150
            };

            cSphereTest.Path = Path.Combine(sBasePath, cSphereTest.Name);
            cSphereTest.SetDXF();

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cSphereTest);

            GalaxyRotationInput cSphere = cCurveBuilder.GetTestInputObject();

            LayeredBody cSphereBody = cSphere.CreateBody(LayeredBodyType.Shell, 1000, 0, 10, 0, 0, 0, DensityDistribution.Constant, null, 80, 4);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cSphereTest, cSphere, null, null);
            // dTotalMass_kg = 2.96E+40

            int n1 = 1;
        }

        private static void DrawCurves_SphereTest_WaterSlideDensity(string sBasePath)
        {
            GalaxyParams cSphereTest = new GalaxyParams()
            {
                Name = "SphereTest_WaterSlideDensity",
                Hf_km_s_Mpc = 3,
                R_Increment_kpc = 1,
                R_DataMin_kpc = 1,
                R_DataMax_kpc = 220,
                R_Max_kpc = 250,
                V_Max_km_s = 250,
                R_LongTick_kpc = 50,
                V_LongTick_km_s = 50,
                R_Max_px = 179.2,
                V_Max_px = 108
                /*
                R_Max_px = 500,
                V_Max_px = 250
                */
            };

            cSphereTest.Path = Path.Combine(sBasePath, cSphereTest.Name);
            cSphereTest.SetDXF();

            GalacticRotationCurveBuilder cCurveBuilder = new GalacticRotationCurveBuilder(cSphereTest);
            GalaxyRotationInput cSphere = cCurveBuilder.GetTestInputObject();
            //cSphere.ComputeHfx = true;

            LayeredBody cSphereBody = cSphere.CreateBody(LayeredBodyType.Shell, 100, 0, 20, 0, 0, 0, DensityDistribution.HalfParabola_WaterSlide, null, 148, 50);

            double dTotalMass_kg = cCurveBuilder.DrawCurves(cSphereTest, cSphere, null, null);
            // dTotalMass_kg = 1.67E+41

            int n1 = 1;
        }

        private static void DrawHf_v_Vel(string sBasePath)
        {
            double dVframeComponent_km_s;
            double v_km_s = GalaxyParams.GetFrameVelocityFromHf_km_s(3, out dVframeComponent_km_s);
            // v_km_s = 550.64 (km/s), dVframeComponent_km_s = 389.36 (km/s)

            v_km_s = GalaxyParams.GetFrameVelocityFromHf_km_s(GalaxyParams.Hf_ao_km_s_Mpc, out dVframeComponent_km_s);
            // Hf_ao_km_s_Mpc = 3.087813770151616
            // v_km_s = 583.35 (km/s), dVframeComponent_km_s = 412.49 (km/s)

            ////////////////////////////////

            Hf_v_Vel cHf_v_Vel = new Hf_v_Vel()
            {
                Name = "Hf_v_Vel",
                Width_px = 179.2,
                Height_px = 179.2
            };

            DXF_ChartTicks cTicks = new DXF_ChartTicks()
            {
                LongTickLength = 6.4,
                ShortTicksPerLongTickX = 0,
                ShortTicksPerLongTickY = 0
            };

            ////////////////////////////////
            // Add the galaxy points.

            HfPoint[] acPoints = new HfPoint[32];

            // Set fit to 0 to ignore.

            // SPARC galaxies
            acPoints[0] = new HfPoint(.07, 23, 1.68, 1); // CamB NFW_flat (< 2 kpc) (poor fit!)
            acPoints[1] = new HfPoint(4.0, 784, 3.03, 4); // DDO064 Burkert
            acPoints[2] = new HfPoint(3.8, 639, 5.97, 4.5); // DDO154 coreNFW_flat
            acPoints[3] = new HfPoint(2.5, 380, 4.07, 3.5); // DDO168 Lucky13_LCDM
            acPoints[4] = new HfPoint(4.0, 870, 4.42, 5); // ESO444-G084 NFW_LCDM
            acPoints[5] = new HfPoint(1.8, 148, 10.2, 5); // IC2574 DC14_flat
            acPoints[6] = new HfPoint(1.3, 115, 13.36, 5); // NGC0055 Einasto_flat (Local Group)
            acPoints[7] = new HfPoint(1.5, 143, 14.5, 3.5); // NGC0247 ML1p0
            acPoints[8] = new HfPoint(.9, 91, 12.04, 5); // NGC0300 Lucky13_LCDM
            acPoints[9] = new HfPoint(1.3, 134, 6.02, 2); // NGC2366 Burkert (poor fit!)
            acPoints[10] = new HfPoint(3.1, 182, 19.6, 4); // NGC2403 Burkert (outlier!) M81 Group, 3.010-3.930 Mpc
            acPoints[11] = new HfPoint(12.0, 588, 9.9, 2, HfPoint_Force.Exclude); // NGC2915 Burkert (poor fit!)(outlier!)
            acPoints[12] = new HfPoint(1.9, 90, 2.25, 5); // NGC2976 Burkert, (DC14_LCDM has Hf = .2 with a fit of 4.6)
            acPoints[13] = new HfPoint(3.6, 738, 7.16, 5); // NGC3109 NFW_LCDM (Local Group)
            acPoints[14] = new HfPoint(5.0, 456, 7.13, 4); // NGC3741 Burkert (high Hf!)
            acPoints[15] = new HfPoint(2.5, 388, 2.28, 4.5); // NGC4068 coreNFW_LCDM
            acPoints[16] = new HfPoint(3.0, 550, 5.63, 2.5); // NGC4214 NFW_LCDM
            acPoints[17] = new HfPoint(3.0, 275, .73, 3.5); // NGC6789 ML1p0 (< 2 kpc)
            acPoints[18] = new HfPoint(1.1, 133, 18.9, 5); // NGC6946 Burkert
            acPoints[19] = new HfPoint(.35, 53, 7.97, 4); // NGC7793 Burkert
            acPoints[20] = new HfPoint(2.0, 203, 5.45, 2); // UGC04305 Burkert (poor fit!)
            acPoints[21] = new HfPoint(1.8, 213, 1.21, 4); // UGC04483 Burkert (< 2 kpc)
            acPoints[22] = new HfPoint(4.0, 486, .82, 3.5); // UGC07232 (< 2 kpc) (outlier!) M94 Group, 2.950 Mpc
            acPoints[23] = new HfPoint(3.0, 585, 10.5, 1); // UGC07524 DC14_flat (poor fit!)
            acPoints[24] = new HfPoint(1.5, 470, 2.5, 4); // UGC07559 coreNFW_LCDM (outlier!)(above curve!) M94 Group, 4.970 Mpc
            acPoints[25] = new HfPoint(.7, 417, 1.5, 3.5 /*, HfPoint_Force.Exclude*/); // UGC07577 coreNFW_LCDM (< 2 kpc)(outlier!)(above curve!)
            acPoints[26] = new HfPoint(3.0, 592, 2.29, 3.5); // UGC07866 coreNFW_LCDM
            acPoints[27] = new HfPoint(2.2, 322, 12.5, 4.5); // UGC08490 DC14_LCDM
            acPoints[28] = new HfPoint(3.2, 457, 2.6, 4); // UGCA444 Burkert (SPARC galaxy)

            //  non-SPARC galaxies, Local Group

            acPoints[29] = new HfPoint(3.0, 419, .307, 4, HfPoint_Force.Exclude); // DDO 210 (< 2 kpc)
            acPoints[30] = new HfPoint(3.5, 582, 38, 3.5, HfPoint_Force.Exclude); // M31
            acPoints[31] = new HfPoint(3.3, 460, 18.7, 4, HfPoint_Force.Exclude); // M33 (version f)
            
            // Milky Way speed relative to CMB: 552.2 ± 5.5 km/s
            // For now, only SPARC galaxies are included in this chart. MW 6/18/25

            ////////////////////////////////
            // Draw the chart.

            cHf_v_Vel.DXF_ChartTicks = cTicks;
            cHf_v_Vel.Path = Path.Combine(sBasePath, cHf_v_Vel.Name);

            int NumPlottedGalaxies;
            double Hf_Ave_km_s_Mpc, V_Ave_km_s;
            double Rframe_Mpc = cHf_v_Vel.Draw(acPoints, 0/*2*/, 2.5, 6, 1000, 100, out NumPlottedGalaxies, out Hf_Ave_km_s_Mpc, out V_Ave_km_s);
            // The data should follow a trend plotted by two lines---v and v2, which are defined as:
            // v = c(Hf / Ho)^2, v2 = v / 2^.5 -> v2^2 + v2^2 = v^2
            // v vs. Hf -> red line, v2 vs. Hf -> magenta line

            // Also plotted in this chart is a vertical line at the Hf that corresponds to ao.

            // dMinR_DataMax_kpc = 0
            // NumPlottedGalaxies = 28
            // Hf_Ave_km_s_Mpc = 2.36
            // V_Ave_km_s = 355
            // Data as of 7/18/25

            // dMinR_DataMax_kpc = 2
            // NumPlottedGalaxies = 23
            // Hf_Ave_km_s_Mpc = 2.46
            // V_Ave_km_s = 371
            // Data as of 6/21/25

            int n1 = 1;
        }

        private static void DrawRAR(string sBasePath)
        {
            // Data are from:
            // C:\PROJECTS\PHYSICS\Galaxy Rotation Papers -> "weak lensing radial acceleration relation.pdf"
            // https://www.aanda.org/articles/aa/full_html/2021/06/aa40108-20/aa40108-20.html
            // https://www.aanda.org/about-aa/copyright

            RAR cRAR = new RAR()
            {
                Name = "RAR",
                ax1_log_m_s = -15,
                ax2_log_m_s = -8.69898, // -9 + this: (19.658 px / 65.305 px) * 1 = 0.30102
                ay1_log_m_s = -13,
                ay2_log_m_s = -8.69898, // -9 + this: (21.009 px / 34.896 px) * .5 = 0.30102
                Width_px = 411.491,
                Height_px = 300.18
            };

            DXF_ChartTicks cTicks = new DXF_ChartTicks()
            {
                LongTickLength = 6.4,
                ShortTicksPerLongTickX = 0,
                ShortTicksPerLongTickY = 0,
                DistanceBetweenTicksX = 65.305, // From the source figure.
                DistanceBetweenTicksY = 34.896 // From the source figure.
            };

            cRAR.DXF_ChartTicks = cTicks;
            cRAR.Path = Path.Combine(sBasePath, cRAR.Name);

            double[] Hf_array_km_s_Mpc = new double[] { 1, 5 };
            //double[] Hf_array_km_s_Mpc = new double[] { 1, 5 };
            double dReferenceMass_kg = 2.45E+41;
            cRAR.Draw(Hf_array_km_s_Mpc, dReferenceMass_kg);
        }

        public static double GetDistanceFromHf_kpc(double dHf_km_s_kpc, double dMass_kg, out double d_vf_km_s, out double d_vp_km_s)
        {
            double d1 = Math.Sqrt(dMass_kg * AstronomicalConstants.G_km_kg_s);

            double d2 = AstronomicalConstants.c_km_s * dHf_km_s_kpc;
            d_vf_km_s = d2 / AstronomicalConstants.Ho_km_s_kpc;
            d2 = Math.Sqrt(d2);

            double r_kpc = d_vf_km_s / AstronomicalConstants.Ho_km_s_kpc;
            double dR_sqrt_kpc = Math.Sqrt(r_kpc);
            double dR_sqrt_km = Math.Sqrt(r_kpc * AstronomicalConversions.km_per_kpc);

            d_vp_km_s = d1 / dR_sqrt_km + d2 * dR_sqrt_kpc;

            return r_kpc;
        }

        public static double GetEffectiveRadiusFromRingMass_kpc(double dRingElementMass_kg, int nNumRingElements, double dRingRadius_kpc, double dHf_km_s_kpc, double r_kpc)
        {
            ////////////////////////////////
            // Acceleration constants

            double d_a3_km_kg_s = AstronomicalConstants.c_km_s * dHf_km_s_kpc / AstronomicalConversions.km_per_kpc;
            double d_mu_km_kg_s = dRingElementMass_kg * AstronomicalConstants.G_km_kg_s;

            double d1_km = (r_kpc * r_kpc + dRingRadius_kpc * dRingRadius_kpc) * AstronomicalConversions.km_per_kpc_sqd;
            double d2_km = 2 * r_kpc * dRingRadius_kpc * AstronomicalConversions.km_per_kpc_sqd;

            ////////////////////////////////
            // Sim constants

            double dElementAngle_rad = 2 * Math.PI / nNumRingElements;
            double dTheta_rad = dElementAngle_rad;

            double d_ax_km_kg_s = 0;

            for (int nElement = 1; nElement < nNumRingElements; nElement++)
            {
                double d_r_km = Math.Sqrt(d1_km + 2 * d2_km * Math.Cos(dTheta_rad));
                double d_an_km_kg_s = GetNetAcceleration_km_kg_s(d_mu_km_kg_s, d_a3_km_kg_s, d_r_km);
                d_ax_km_kg_s += d_an_km_kg_s * Math.Cos(dTheta_rad);

                dTheta_rad += dElementAngle_rad;
            }

            d_mu_km_kg_s = (nNumRingElements - 1) * dRingElementMass_kg * AstronomicalConstants.G_km_kg_s;
            double dEffectiveRadius_km = GetRadiusFromNetAcceleration_km(d_ax_km_kg_s, d_mu_km_kg_s, d_a3_km_kg_s);

            return dEffectiveRadius_km;
        }

        public static double GetNetAcceleration_km_kg_s(double d_mu_km_kg_s, double d_a3_km_kg_s, double d_r_km)
        {
            double d_an_km_kg_s = d_mu_km_kg_s / (d_r_km * d_r_km) + 2 * Math.Sqrt(d_a3_km_kg_s * d_mu_km_kg_s) / d_r_km + d_a3_km_kg_s;

            return d_an_km_kg_s;
        }

        public static double GetOrbitingVelocityFromNetAcceleration_km_s(double d_an_km_kg_s, double d_r_km)
        {
            double d_v_km_s = Math.Sqrt(d_r_km * d_an_km_kg_s);

            return d_v_km_s;
        }

        public static double GetRadiusFromNetAcceleration_km(double d_an_km_kg_s, double d_mu_km_kg_s, double d_a3_km_kg_s)
        {
            double d_r_km_old = (10 * AstronomicalConversions.km_per_kpc);
            double d_r_km;

            for (; ; )
            {
                d_r_km = (1 / d_an_km_kg_s) * ((d_r_km_old / d_r_km_old) + 2 * Math.Sqrt(d_r_km_old * d_a3_km_kg_s) + d_a3_km_kg_s * d_r_km_old);

                if (Math.Abs(d_r_km - d_r_km_old) < Constants.c_dMaxError)
                {
                    return d_r_km;
                }

                d_r_km_old = d_r_km;
            }
        }

        private static double GetRf_Mpc(double dMass_kg, double dVr_km_s)
        {
            double dVr_m_s = dVr_km_s * 1000;
            double dRf_Mpc = (4 * AstronomicalConstants.c_m_s * dMass_kg * AstronomicalConstants.G_m_kg_s / (dVr_m_s * dVr_m_s * dVr_m_s)) / (AstronomicalConversions.km_per_Mpc * 1000);

            return dRf_Mpc;
        }

        private static double GetVframe_km_s(double dH_km_s_Mpc, double dV_rel_CMB_km_s, out double dVframe2_km_s)
        {
            double dVframe_km_s = AstronomicalConstants.c_km_s * dH_km_s_Mpc * dH_km_s_Mpc / (AstronomicalConstants.Ho_km_s_Mpc * AstronomicalConstants.Ho_km_s_Mpc);
            //double dVframe_km_s = AstronomicalConstants.c_km_s * dH_km_s_Mpc * dH_km_s_Mpc / (74 * 74);
            dVframe2_km_s = Math.Sqrt(2) * dV_rel_CMB_km_s;

            return dVframe_km_s;
        }

        private static double GetVr_From_Rf_km_s(double dMass_kg, double dRf_Mpc, double dBulgeSpeed_km_s, double dDiskSpeed_km_s, double dGasSpeed_km_s, out double dNetSpeed_km_s)
        {
            double dRf_m = dRf_Mpc * AstronomicalConversions.km_per_Mpc * 1000;
            double dVr_km_s = Math.Pow(4 * AstronomicalConstants.c_m_s * dMass_kg * AstronomicalConstants.G_m_kg_s / dRf_m, 1.0/3.0) / 1000;
            dNetSpeed_km_s = Math.Sqrt(dBulgeSpeed_km_s * dBulgeSpeed_km_s + dDiskSpeed_km_s * dDiskSpeed_km_s + dGasSpeed_km_s * dGasSpeed_km_s + dVr_km_s * dVr_km_s);

            return dVr_km_s;
        }

        public static void Test1()
        {
            double d_a3_km_kg_s = AstronomicalConstants.c_km_s * AstronomicalConstants.MilkyWay_Hf_km_s_kpc / AstronomicalConversions.km_per_kpc;
            //d_a3_km_kg_s = 0;
            double d_mu_km_kg_s = .055 * AstronomicalConstants.MilkyWay_Mass_kg * AstronomicalConstants.G_km_kg_s;
            double d_r_km = 60 * AstronomicalConversions.km_per_kpc;

            double d_an_km_kg_s = GetNetAcceleration_km_kg_s(d_mu_km_kg_s, d_a3_km_kg_s, d_r_km);
            double d_v_km_s = GetOrbitingVelocityFromNetAcceleration_km_s(d_an_km_kg_s, d_r_km);
            Debug.Assert(DebugHelpers.ZeroDecimalSanityCheck(d_v_km_s, 196));
        }
    }
}

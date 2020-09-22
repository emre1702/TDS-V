using NeoSmart.Hashing.XXHash;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using TDS_Shared.Data.Enums;

namespace TDS_Shared.Data.Utility
{
    public static class SharedUtils
    {
        #region Public Fields

        public static readonly Random Rnd = new Random();

        #endregion Public Fields

        #region Public Properties

        public static bool IsServersided { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Color? GetColorFromHtmlRgba(string rgba)
        {
            if (string.IsNullOrEmpty(rgba))
                return null;

            int left = rgba.IndexOf('(');
            int right = rgba.IndexOf(')');

            if (left < 0 || right < 0)
                return Color.White;
            string noBrackets = rgba.Substring(left + 1, right - left - 1);

            string[] parts = noBrackets.Split(',');

            int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
            int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
            int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

            if (parts.Length == 3)
            {
                return Color.FromArgb(r, g, b);
            }
            else if (parts.Length == 4)
            {
                float a = float.Parse(parts[3], CultureInfo.InvariantCulture);
                return Color.FromArgb((int)(a * 255), r, g, b);
            }

            return Color.White;
        }

        public static PedBodyPart GetPedBodyPart(int boneIndex)
        {
            switch (boneIndex)
            {
                case 0:
                    return PedBodyPart.GenitalRegion;

                case 1:
                case 2:
                case 4:
                case 5:
                    return PedBodyPart.Leg;

                case 3:
                case 6:
                    return PedBodyPart.Foot;

                case 7:
                case 8:
                case 9:
                case 10:
                    return PedBodyPart.Torso;

                case 11:
                case 12:
                case 13:
                case 15:
                case 16:
                case 17:
                    return PedBodyPart.Arm;

                case 14:
                case 18:
                    return PedBodyPart.Hand;

                case 19:
                    return PedBodyPart.Neck;

                case 20:
                    return PedBodyPart.Head;
            }

            return PedBodyPart.Head;
        }

        public static PedBodyPart GetPedBodyPart(PedBone pedBone)
        {
            switch (pedBone)
            {
                case PedBone.FACIAL_facialRoot:
                case PedBone.FB_Brow_Centre_000:
                case PedBone.FB_Jaw_000:
                case PedBone.FB_LowerLipRoot_000:
                case PedBone.FB_LowerLip_000:
                case PedBone.FB_L_Brow_Out_000:
                case PedBone.FB_L_CheekBone_000:
                case PedBone.FB_L_Eye_000:
                case PedBone.FB_L_Lid_Upper_000:
                case PedBone.FB_L_Lip_Bot_000:
                case PedBone.FB_L_Lip_Corner_000:
                case PedBone.FB_L_Lip_Top_000:
                case PedBone.FB_R_Brow_Out_000:
                case PedBone.FB_R_CheekBone_000:
                case PedBone.FB_R_Eye_000:
                case PedBone.FB_R_Lid_Upper_000:
                case PedBone.FB_R_Lip_Bot_000:
                case PedBone.FB_R_Lip_Corner_000:
                case PedBone.FB_R_Lip_Top_000:
                case PedBone.FB_Tongue_000:
                case PedBone.FB_UpperLipRoot_000:
                case PedBone.FB_UpperLip_000:
                case PedBone.IK_Head:
                case PedBone.SKEL_Head:
                    return PedBodyPart.Head;

                case PedBone.IK_L_Foot:
                case PedBone.IK_R_Foot:
                case PedBone.PH_L_Foot:
                case PedBone.PH_R_Foot:
                case PedBone.SKEL_L_Foot:
                case PedBone.SKEL_L_Toe0:
                case PedBone.SKEL_R_Foot:
                case PedBone.SKEL_R_Toe0:
                    return PedBodyPart.Foot;

                case PedBone.IK_L_Hand:
                case PedBone.IK_R_Hand:
                case PedBone.PH_L_Hand:
                case PedBone.PH_R_Hand:
                case PedBone.SKEL_L_Finger00:
                case PedBone.SKEL_L_Finger01:
                case PedBone.SKEL_L_Finger02:
                case PedBone.SKEL_L_Finger10:
                case PedBone.SKEL_L_Finger11:
                case PedBone.SKEL_L_Finger12:
                case PedBone.SKEL_L_Finger20:
                case PedBone.SKEL_L_Finger21:
                case PedBone.SKEL_L_Finger22:
                case PedBone.SKEL_L_Finger30:
                case PedBone.SKEL_L_Finger31:
                case PedBone.SKEL_L_Finger32:
                case PedBone.SKEL_L_Finger40:
                case PedBone.SKEL_L_Finger41:
                case PedBone.SKEL_L_Finger42:
                case PedBone.SKEL_R_Finger00:
                case PedBone.SKEL_R_Finger01:
                case PedBone.SKEL_R_Finger02:
                case PedBone.SKEL_R_Finger10:
                case PedBone.SKEL_R_Finger11:
                case PedBone.SKEL_R_Finger12:
                case PedBone.SKEL_R_Finger20:
                case PedBone.SKEL_R_Finger21:
                case PedBone.SKEL_R_Finger22:
                case PedBone.SKEL_R_Finger30:
                case PedBone.SKEL_R_Finger31:
                case PedBone.SKEL_R_Finger32:
                case PedBone.SKEL_R_Finger40:
                case PedBone.SKEL_R_Finger41:
                case PedBone.SKEL_R_Finger42:
                case PedBone.SKEL_L_Hand:
                case PedBone.SKEL_R_Hand:
                    return PedBodyPart.Hand;

                case PedBone.MH_L_Elbow:
                case PedBone.MH_R_Elbow:
                case PedBone.RB_L_ArmRoll:
                case PedBone.RB_L_ForeArmRoll:
                case PedBone.RB_R_ArmRoll:
                case PedBone.RB_R_ForeArmRoll:
                case PedBone.SKEL_L_Forearm:
                case PedBone.SKEL_L_UpperArm:
                case PedBone.SKEL_R_Forearm:
                case PedBone.SKEL_R_UpperArm:
                    return PedBodyPart.Arm;

                case PedBone.SKEL_L_Clavicle:
                case PedBone.SKEL_R_Clavicle:
                case PedBone.SKEL_ROOT:
                case PedBone.IK_Root:
                case PedBone.RB_L_ThighRoll:
                case PedBone.RB_R_ThighRoll:
                case PedBone.SKEL_Spine0:
                case PedBone.SKEL_Spine1:
                case PedBone.SKEL_Spine2:
                case PedBone.SKEL_Spine3:
                case PedBone.SKEL_Spine_Root:
                    return PedBodyPart.Torso;

                case PedBone.SKEL_Pelvis:
                    return PedBodyPart.GenitalRegion;

                case PedBone.RB_Neck_1:
                case PedBone.SKEL_Neck_1:
                    return PedBodyPart.Neck;

                case PedBone.SKEL_L_Calf:
                case PedBone.SKEL_L_Thigh:
                case PedBone.SKEL_R_Calf:
                case PedBone.SKEL_R_Thigh:
                case PedBone.MH_L_Knee:
                case PedBone.MH_R_Knee:
                    return PedBodyPart.Leg;
            }

            return PedBodyPart.Torso;
        }

        public static T GetRandom<T>(params T[] elements)
        {
            var rndIndex = Rnd.Next(0, elements.Length);
            return elements[rndIndex];
        }

        public static T GetRandom<T>(List<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection[rndIndex];
        }

        public static T GetRandom<T>(IEnumerable<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count());
            return collection.ElementAt(rndIndex);
        }

        public static T GetRandom<T>(HashSet<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection.ElementAt(rndIndex);
        }

        public static T GetRandom<T>(ICollection<T> collection)
        {
            var rndIndex = Rnd.Next(collection.Count);
            return collection.ElementAt(rndIndex);
        }

        public static string HashPWClient(string pw)
        {
            return XXHash64.Hash(Encoding.Default.GetBytes(pw)).ToString();
        }

        #endregion Public Methods
    }
}

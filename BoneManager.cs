using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;

namespace KHR_MayFes
{
    /*
     * Kinectでは関節の①情報を取得できるので、
     * それら関節の間をつなぐ、骨を定義する
     */ 
    public enum BoneTag {
        SPINE_BASE_MID,
        SPINE_MID_TOP,
        SHOULDER_RIGHT_LEFT,
        HIP_RIGHT_LEFT,
        SPINE_NECK,
        NECK_HEAD,
        RIGHT_SPINE_SHOULDER,
        RIGHT_SHOULDER_ELBOW,
        RIGHT_ELBOW_WRIST,
        RIGHT_BASE_HIP,
        RIGHT_HIP_KNEE,
        RIGHT_KNEE_ANKLE,
        RIGHT_ANKLE_FOOT,
        LEFT_SPINE_SHOULDER,
        LEFT_SHOULDER_ELBOW,
        LEFT_ELBOW_WRIST,
        LEFT_BASE_HIP,
        LEFT_HIP_KNEE,
        LEFT_KNEE_ANKLE,
        LEFT_ANKLE_FOOT,
        NUM_OF_BONES
    }

    /*
     * Bone管理用クラス
     */ 
    public class BoneManager
    {
        //Boneを格納する連想配列
        private Dictionary<BoneTag, Vector3D> boneDict;
        private IReadOnlyDictionary<JointType, Joint> mJoints;

        //コンストラクタ
        public BoneManager()
        {
            boneDict = new Dictionary<BoneTag,Vector3D>();
            for (int i = 0; i < (int)BoneTag.NUM_OF_BONES; i++)
            {
                boneDict.Add((BoneTag)i, new Vector3D(0.0, 0.0, 0.0));
            }
        }

        //joint二つをもとにBoneTagに対応するBoneをboneDictに登録する
        private void jointsToBone(JointType joint_a, JointType joint_b, BoneTag destBone)
        {
            boneDict[destBone] = MathUtil.JointsToVector3D(mJoints[joint_a], mJoints[joint_b]);
        }

        //boneDictを作成する
        public void setBones(IReadOnlyDictionary<JointType, Joint> joints){
            mJoints = joints;
            jointsToBone(JointType.SpineShoulder, JointType.Neck, BoneTag.SPINE_NECK);
            jointsToBone(JointType.Neck, JointType.Head, BoneTag.NECK_HEAD);
            jointsToBone(JointType.ElbowRight, JointType.WristRight, BoneTag.RIGHT_ELBOW_WRIST);
            jointsToBone(JointType.ShoulderRight, JointType.ElbowRight, BoneTag.RIGHT_SHOULDER_ELBOW);
            jointsToBone(JointType.SpineShoulder, JointType.ShoulderRight, BoneTag.RIGHT_SPINE_SHOULDER);
            jointsToBone(JointType.ElbowLeft, JointType.WristLeft, BoneTag.LEFT_ELBOW_WRIST);
            jointsToBone(JointType.ShoulderLeft, JointType.ElbowLeft, BoneTag.LEFT_SHOULDER_ELBOW);
            jointsToBone(JointType.SpineShoulder, JointType.ShoulderLeft, BoneTag.LEFT_SPINE_SHOULDER);
            jointsToBone(JointType.ShoulderRight, JointType.ShoulderLeft, BoneTag.SHOULDER_RIGHT_LEFT);
            jointsToBone(JointType.HipRight, JointType.HipLeft, BoneTag.HIP_RIGHT_LEFT);
            jointsToBone(JointType.SpineMid, JointType.SpineShoulder, BoneTag.SPINE_MID_TOP);
            jointsToBone(JointType.SpineBase, JointType.SpineMid, BoneTag.SPINE_BASE_MID);
            jointsToBone(JointType.HipRight, JointType.KneeRight, BoneTag.RIGHT_HIP_KNEE);
            jointsToBone(JointType.KneeRight, JointType.AnkleRight, BoneTag.RIGHT_KNEE_ANKLE);
            jointsToBone(JointType.HipLeft, JointType.KneeLeft, BoneTag.LEFT_HIP_KNEE);
            jointsToBone(JointType.KneeLeft, JointType.AnkleLeft, BoneTag.LEFT_KNEE_ANKLE);
            jointsToBone(JointType.AnkleRight, JointType.FootRight, BoneTag.RIGHT_ANKLE_FOOT);
            jointsToBone(JointType.AnkleLeft, JointType.FootLeft, BoneTag.LEFT_ANKLE_FOOT);
        }

        //BoneTagからBoneに対応するベクトルを取得する
        public Vector3D getBoneWithTag(BoneTag tag)
        {
            return boneDict[tag];
        }

        //boneDictを取得する
        public Dictionary<BoneTag, Vector3D> getBones()
        {
            return boneDict;
        }
    }
}
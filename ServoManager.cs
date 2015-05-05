using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Diagnostics;

namespace KHR_MayFes
{
    //各サーボの名称
    public enum ServoTag{
        NECK = 0, //首(壊れている?)
        WAIST = 1, //腰(回転方向不明)
        LEFT_SHOULDER_PITCH = 2, //左肩ピッチ方向回転
        RIGHT_SHOULDER_PITCH = 3, //右肩ピッチ方向回転
        LEFT_SHOULDER_ROLL = 4, //左肩ロール回転(外側に開く)
        RIGHT_SHOULDER_ROLL = 5, //右肩ロール回転(内側に閉じる(鏡面対称ではない))
        LEFT_SHOULDER_YAW = 6, //左肩ヨー方向回転
        RIGHT_SHOULDER_YAW = 7, //右肩ヨー方向回転
        LEFT_ELBOW = 8, //左肘
        RIGHT_ELBOW = 9, //右肘
        LEFT_HIP_YAW = 10, //左股間節ヨー方向回転
        RIGHT_HIP_YAW = 11, //右股間節ヨー方向回転
        LEFT_HIP_ROLL = 12, //左股関節ロール方向回転
        RIGHT_HIP_ROLL = 13, //右股関節ロール方向回転
        LEFT_HIP_PITCH = 14, //左股関節ピッチ方向回転
        RIGHT_HIP_PITCH = 15, //右股関節ピッチ方向回転
        LEFT_KNEE = 16, //左膝
        RIGHT_KNEE = 17, //右膝
        LEFT_ANKLE_PITCH = 18, //左足首ピッチ方向回転
        RIGHT_ANKLE_PITCH = 19, //右足首ピッチ方向回転
        LEFT_ANKLE_ROLL = 20, //左足首ロール方向回転
        RIGHT_ANKLE_ROLL = 21, //右足首ロール方向回転
        NUM_OF_SERVO = 22
    }

    /*
     * サーボの情報を格納するためのクラス
     * 方向、トリム、最下限、最上限を設定できる
     */ 
    public class ServoData
    {
        public ServoData(bool dir, int t, int llim, int ulim)
        {
            destAngle = 0;
            direction = dir;
            trim = t;
            upperLimit = ulim;
            lowerLimit = llim;
            offset = 0;
        }

        public ServoData(bool dir, int t, int llim, int ulim, int ofs)
        {
            destAngle = 0;
            direction = dir;
            trim = t;
            upperLimit = ulim;
            lowerLimit = llim;
            offset = ofs;
        }


        public ServoData(bool dir, int t)
        {
            destAngle = 0;
            direction = dir;
            trim = t;
            upperLimit = 11500;
            lowerLimit = 3500;
        }

        //弧度法角度によって目標値を設定
        public void SetDestWithDegree(double d)
        {
            int dest;
            if (direction)
            {
                dest = toServoAngle(d);
            }
            else
            {
                dest = toServoAngle(-d);
            }
            destAngle = dest;
        }

        //サーボ角度によって直接目標値を設定
        public void SetDestWithServoAngle(int d)
        {
            //下限より小さいときは下限にする
            if (d < lowerLimit)
            {
                d = lowerLimit;
            }
            //上限より大きいときは上限にする
            if (d > upperLimit)
            {
                d = upperLimit;
            }
            destAngle = d;
        }

        //degree指定
        public int toServoAngle(double angle)
        {
            //7500のとき0deg            
            int neutral = 7500;       
            //±135deg =&gt;±4000 
            int servoAngle = neutral + (int)((angle / 135.0) * 4000);
            //サーボモーター最大角、最小角の設定
            //下限より小さいときは下限にする
            if (servoAngle < lowerLimit)
            {
                servoAngle = lowerLimit;
            }
            //上限より大きいときは上限にする
            if (servoAngle > upperLimit)
            {
                servoAngle = upperLimit;
            }
            //Debug.WriteLine("servoangle : {0}", servoAngle);

            return (int)servoAngle; 
        }

        public bool direction; //方向の正負
        public int destAngle; //目標角度 [サーボ角度]
        public int trim; //トリム
        public int upperLimit; //上限
        public int lowerLimit; //下限
        public int offset; //オフセット
    }

    class ServoManager
    {
        private Dictionary<ServoTag, ServoData> servoDict;
        private Dictionary<BoneTag, Vector3D> mBoneDict;
        public ServoManager()
        {
            servoDict = new Dictionary<ServoTag, ServoData>();
            servoDict.Add(ServoTag.NECK, new ServoData(true, 0, 6970, 7950));
            servoDict.Add(ServoTag.WAIST, new ServoData(true, 0, 7030, 8070));
            servoDict.Add(ServoTag.LEFT_SHOULDER_PITCH, new ServoData(true, -1350, 7200, 10200));
            servoDict.Add(ServoTag.RIGHT_SHOULDER_PITCH, new ServoData(false, 900, 4800, 7800));
            servoDict.Add(ServoTag.LEFT_SHOULDER_ROLL, new ServoData(true, -2650, 6820, 10200, -10));
            servoDict.Add(ServoTag.RIGHT_SHOULDER_ROLL, new ServoData(false, 2650, 4800, 8180, -10));
            servoDict.Add(ServoTag.LEFT_SHOULDER_YAW, new ServoData(false, 0, 6170, 8170));
            servoDict.Add(ServoTag.RIGHT_SHOULDER_YAW, new ServoData(true, 0, 8830, 6830));
            servoDict.Add(ServoTag.LEFT_ELBOW, new ServoData(false, 2650, 4900, 7545));
            servoDict.Add(ServoTag.RIGHT_ELBOW, new ServoData(true, -2650, 7545, 10100));
            servoDict.Add(ServoTag.LEFT_HIP_PITCH, new ServoData(true, 0, 7250, 8700));
            servoDict.Add(ServoTag.RIGHT_HIP_PITCH, new ServoData(true, 0, 6300, 7750));
            servoDict.Add(ServoTag.LEFT_HIP_YAW, new ServoData(true, 0, 7650, 8100));
            servoDict.Add(ServoTag.RIGHT_HIP_YAW, new ServoData(true, 0, 6900, 7350));
            servoDict.Add(ServoTag.LEFT_HIP_ROLL, new ServoData(true, 30, 7250, 7900));
            servoDict.Add(ServoTag.RIGHT_HIP_ROLL, new ServoData(true, -30, 7100, 7750));
            servoDict.Add(ServoTag.LEFT_KNEE, new ServoData(true, 0, 8200, 9800));
            servoDict.Add(ServoTag.RIGHT_KNEE, new ServoData(true, 0, 5200, 6800));
            servoDict.Add(ServoTag.LEFT_ANKLE_PITCH, new ServoData(true, 0, 6200, 7650));
            servoDict.Add(ServoTag.RIGHT_ANKLE_PITCH, new ServoData(true, 0, 7350, 8800));
            servoDict.Add(ServoTag.LEFT_ANKLE_ROLL, new ServoData(true, 20, 7200, 7950));
            servoDict.Add(ServoTag.RIGHT_ANKLE_ROLL, new ServoData(true, -20, 7050, 7800));
        }

        //二つのBoneの角度として設定できるサーボはこいつで設定する
        private void bonesToServo(BoneTag bone_a, BoneTag bone_b, ServoTag destServo){
            servoDict[destServo].SetDestWithDegree(Vector3D.AngleBetween(mBoneDict[bone_a], mBoneDict[bone_b]));
        }

        /*
         * 上半身をKinectによって設定する
         * 度数表記
         */ 
        public void SetUpperBody(Dictionary<BoneTag, Vector3D> boneDict){
            mBoneDict = boneDict;

            //首
            Vector3D neckHeadNorm = mBoneDict[BoneTag.NECK_HEAD] - Vector3D.DotProduct(mBoneDict[BoneTag.NECK_HEAD], mBoneDict[BoneTag.SPINE_NECK]) * mBoneDict[BoneTag.SPINE_NECK];
            neckHeadNorm.Normalize();
            servoDict[ServoTag.NECK].SetDestWithDegree(MathUtil.getAxisAngle(mBoneDict[BoneTag.NECK_HEAD], mBoneDict[BoneTag.SHOULDER_RIGHT_LEFT], mBoneDict[BoneTag.SPINE_NECK]));
            
            //右肘
            bonesToServo(BoneTag.RIGHT_ELBOW_WRIST, BoneTag.RIGHT_SHOULDER_ELBOW, ServoTag.RIGHT_ELBOW);
            //左肘
            bonesToServo(BoneTag.LEFT_ELBOW_WRIST, BoneTag.LEFT_SHOULDER_ELBOW, ServoTag.LEFT_ELBOW);

            //右肩ロール
            servoDict[ServoTag.RIGHT_SHOULDER_ROLL].SetDestWithDegree(Vector3D.AngleBetween(-mBoneDict[BoneTag.RIGHT_SPINE_SHOULDER], mBoneDict[BoneTag.RIGHT_SHOULDER_ELBOW]) - 90 + servoDict[ServoTag.RIGHT_SHOULDER_ROLL].offset);
            //左肩ロール
            servoDict[ServoTag.LEFT_SHOULDER_ROLL].SetDestWithDegree(Vector3D.AngleBetween(-mBoneDict[BoneTag.LEFT_SPINE_SHOULDER], mBoneDict[BoneTag.LEFT_SHOULDER_ELBOW]) - 90 + servoDict[ServoTag.LEFT_SHOULDER_ROLL].offset);

            //右肩ピッチ
            servoDict[ServoTag.RIGHT_SHOULDER_PITCH].SetDestWithDegree(-MathUtil.getAxisAngle(-mBoneDict[BoneTag.RIGHT_SHOULDER_ELBOW], mBoneDict[BoneTag.SPINE_MID_TOP], mBoneDict[BoneTag.RIGHT_SPINE_SHOULDER]));
            //左肩ピッチ
            servoDict[ServoTag.LEFT_SHOULDER_PITCH].SetDestWithDegree(MathUtil.getAxisAngle(-mBoneDict[BoneTag.LEFT_SHOULDER_ELBOW], mBoneDict[BoneTag.SPINE_MID_TOP], mBoneDict[BoneTag.LEFT_SPINE_SHOULDER]));

            //右肩ヨー
            Vector3D spineShoulderRightNormalized = mBoneDict[BoneTag.RIGHT_SPINE_SHOULDER];
            spineShoulderRightNormalized.Normalize();
            Vector3D shoulderElbowRightNormalized = mBoneDict[BoneTag.RIGHT_SHOULDER_ELBOW];
            shoulderElbowRightNormalized.Normalize();
            Vector3D normRight = spineShoulderRightNormalized - Vector3D.DotProduct(spineShoulderRightNormalized, shoulderElbowRightNormalized) * shoulderElbowRightNormalized;
            Vector3D elbowWristRightNorm = mBoneDict[BoneTag.RIGHT_ELBOW_WRIST];
            elbowWristRightNorm.Normalize();
            Vector3D normWristRight = elbowWristRightNorm - Vector3D.DotProduct(elbowWristRightNorm, shoulderElbowRightNormalized) * shoulderElbowRightNormalized;
            servoDict[ServoTag.RIGHT_SHOULDER_YAW].SetDestWithDegree(Vector3D.AngleBetween(normWristRight, normRight) - 90);

            //左肩ヨー
            Vector3D spineShoulderLeftNormalized = mBoneDict[BoneTag.LEFT_SPINE_SHOULDER];
            spineShoulderLeftNormalized.Normalize();
            Vector3D shoulderElbowLeftNormalized = mBoneDict[BoneTag.LEFT_SHOULDER_ELBOW];
            shoulderElbowLeftNormalized.Normalize();
            Vector3D normLeft = spineShoulderLeftNormalized - Vector3D.DotProduct(spineShoulderLeftNormalized, shoulderElbowLeftNormalized) * shoulderElbowLeftNormalized;
            Vector3D elbowWristLeftNorm = mBoneDict[BoneTag.LEFT_ELBOW_WRIST];
            elbowWristLeftNorm.Normalize();
            Vector3D normWristLeft = elbowWristLeftNorm - Vector3D.DotProduct(elbowWristLeftNorm, shoulderElbowLeftNormalized) * shoulderElbowLeftNormalized;
            servoDict[ServoTag.LEFT_SHOULDER_YAW].SetDestWithDegree(Vector3D.AngleBetween(normWristLeft, normLeft) - 90);


            servoDict[ServoTag.LEFT_ELBOW].destAngle -= 500;
            servoDict[ServoTag.RIGHT_ELBOW].destAngle += 500;
            servoDict[ServoTag.LEFT_SHOULDER_ROLL].destAngle -= 500;
            servoDict[ServoTag.RIGHT_SHOULDER_ROLL].destAngle += 500;
        }

        /*
         * 下半身の歩行モーションを設定
         * 必ずID10以降のすべてのサーボのdestを指定すること
         * \param lowerServoDests ID10以降のサーボの目標値を入れた配列 (サイズは13)
         */
        public void SetLowerBody(int[] lowerServoDests)
        {
            /*for (int i = 0; i < 13; i++)
            {
                Debug.WriteLine("lower servo {0} {1}", i, lowerServoDests[i]);
            }*/
            servoDict[ServoTag.WAIST].SetDestWithServoAngle(lowerServoDests[0]);
            for(int i = 1; i < 13; i++){ //マジックナンバー 13 : 下半身のサーボの数
                servoDict[(ServoTag)i+9].SetDestWithServoAngle(lowerServoDests[i]);
            }

            /*servoDict[ServoTag.WAIST].SetDestWithServoAngle(7500);
            servoDict[ServoTag.LEFT_HIP_YAW].SetDestWithServoAngle(7500);
            servoDict[ServoTag.RIGHT_HIP_YAW].SetDestWithServoAngle(7500);
            servoDict[ServoTag.LEFT_HIP_ROLL].SetDestWithServoAngle(7530);
            servoDict[ServoTag.RIGHT_HIP_ROLL].SetDestWithServoAngle(7470);
            servoDict[ServoTag.LEFT_HIP_PITCH].SetDestWithServoAngle(8000);
            servoDict[ServoTag.RIGHT_HIP_PITCH].SetDestWithServoAngle(7000);
            servoDict[ServoTag.LEFT_KNEE].SetDestWithServoAngle(8500);
            servoDict[ServoTag.RIGHT_KNEE].SetDestWithServoAngle(6500);
            servoDict[ServoTag.LEFT_ANKLE_PITCH].SetDestWithServoAngle(6900);
            servoDict[ServoTag.RIGHT_ANKLE_PITCH].SetDestWithServoAngle(8100);
            servoDict[ServoTag.LEFT_ANKLE_ROLL].SetDestWithServoAngle(7520);
            servoDict[ServoTag.RIGHT_ANKLE_ROLL].SetDestWithServoAngle(7480);*/
        }

        //servoDictを返す関数
        public Dictionary<ServoTag, ServoData> getServo()
        {
            return servoDict;
        }

        //サーボ角度表記へ変換
        private Dictionary<int, int> servoToDest(Dictionary<ServoTag, ServoData> d){
            Dictionary<int, int> ret = new Dictionary<int, int>(d.Count);
            foreach (ServoTag s in servoDict.Keys)
            {
                //Debug.WriteLine("servo ID {0} : {1}", (int)s, d[s].destAngle);
                ret.Add((int)s, d[s].destAngle);
            }
            return ret;
        }

        public byte[] generateCommand()
        {
            byte[] cmd = CommandUtil.SeriesServoMove(servoToDest(servoDict),15 );
            return cmd;
        }
    }
}

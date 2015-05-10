using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KHR_MayFes
{
    public partial class MotionManager
    {
        private int[] GetMotionDests(MotionStatus motionStatus)
        {
            int[] ret;
            switch (motionStatus)
            {
                case MotionStatus.STOP:
                    ret = GetSTOPDests();
                    break;
                case MotionStatus.WALK_FORWARD:
                    ret = GetWALK_FORWARDDests();
                    break;
                case MotionStatus.WALK_BACKWARD:
                    ret = GetWALK_BACKWARDDests();
                    break;
                case MotionStatus.WALK_RIGHT:
                    ret = GetWALK_RIGHTDests();
                    break;
                case MotionStatus.WALK_LEFT:
                    ret = GetWALK_LEFTDests();
                    break;
                case MotionStatus.TURN_RIGHT:
                    ret = GetTURN_RIGHTDests();
                    break;
                case MotionStatus.TURN_LEFT:
                    ret = GetTURN_LEFTDests();
                    break;
                default :
                    ret = GetSTOPDests();
                    break;
            }

            return AddTrim(ret);
        }

        int[] AddTrim(int[] posVals)
        {
            var ret = new int[13];
            ret[0] = posVals[0] + 7500;
            ret[1] = posVals[1] + 7500;
            ret[2] = posVals[2] + 7500;
            ret[3] = posVals[3] + 7500;
//            ret[3] = posVals[3] + 7530;
            ret[4] = posVals[4] + 7470;
            //ret[5] = posVals[5] + 8000;
            //ret[6] = posVals[6] + 7000;
            //ret[5] = posVals[5] + 7700;
            //ret[6] = posVals[6] + 7300;
            ret[5] = posVals[5] + 7600;
            ret[6] = posVals[6] + 7400;
            ret[7] = posVals[7] + 8500;
            ret[8] = posVals[8] + 6500;
            ret[9] = posVals[9] + 6900;
            ret[10] = posVals[10] + 8100;
            ret[11] = posVals[11] + 7520;
            ret[12] = posVals[12] + 7480;

            return ret;
        }

        /*
         * posA と posBの間をratioの値を使って補完する
         * ratioは0~1の割合の値 (現在のフレーム数/登録フレーム数)
         */ 
        int[] InterPolatePositions(int[][] dests, int[] frames, int nowID, int nextID)
        {
            int[] ret = new int[13]; //マジックナンバー 下半身サーボ数
            double ratio = (double)frameCount / (double)frames[nextID];
            for (int i = 0; i < 13; i++)
            {
                //Debug.WriteLine("dest now next : {0} {1}", dests[nowID][i], dests[nextID][i]);
                ret[i] = (int)((double)dests[nowID][i] + ratio * (double)(dests[nextID][i] - dests[nowID][i]));
                //Debug.WriteLine("dest now next : {0} {1} {2}", dests[nowID][i], dests[nextID][i], ret[i]);
            }
            return ret;
        }

        /*
         * 分岐のない遷移はこれで書ける
         */
        private int[] NormalTransition(int[][] dests, int[] frames, int nowID, int nextID)
        {
            if(frameCount == frames[nextID]){
                frameCount = 0;
                positionID = nextID;
                return dests[nextID];
            }
            //Debug.WriteLine("frame ratio : {0} / {1}", frameCount, frames[nextID]);
            return InterPolatePositions(dests, frames, nowID, nextID);
        }

        //frameCount(経過フレーム数)から計算する感じです
        private int[] GetSTOPDests()
        {
            //これはおそらくこれで確定
            int[] ret ={0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            return ret;
        }

        private int[] GetWALK_LEFTDests()
        {
            int[] ret = { 7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480 };
            return ret;
        }

        private int[] GetWALK_RIGHTDests()
        {
            int[] ret = { 7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480 };
            return ret;
        }
    }
}


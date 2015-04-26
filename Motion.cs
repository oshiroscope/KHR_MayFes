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
            ret[3] = posVals[3] + 7530;
            ret[4] = posVals[4] + 7470;
            ret[5] = posVals[5] + 8000;
            ret[6] = posVals[6] + 7000;
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

        private int[] WALK_FORWARD_FRAMES = {
                                                /*//posFirst : 0
                                                0,
                                                //pos13 : 1
                                                20,
                                                //pos45 : 2
                                                12,
                                                //pos7 : 3
                                                17,
                                                //pos9 : 4
                                                20,
                                                //pos33 : 5
                                                20,
                                                //pos0 : 6
                                                12,
                                                //pos8 : 7
                                                17,
                                                //pos22 : 8 
                                                20,
                                                //pos4 : 9
                                                20,
                                                //pos1 : 10
                                                12,
                                                //pos43 : 11
                                                15,
                                                //pos44 : 12
                                                15,
                                                //pos1Fin : 13
                                                30//posFirst : 0
                                                0,
                                                //pos13 : 1
                                                20,
                                                //pos45 : 2
                                                12,
                                                //pos7 : 3
                                                17,
                                                //pos9 : 4
                                                20,
                                                //pos33 : 5
                                                20,
                                                //pos0 : 6
                                                12,
                                                //pos8 : 7
                                                17,
                                                //pos22 : 8 
                                                20,
                                                //pos4 : 9
                                                20,
                                                //pos1 : 10
                                                12,
                                                //pos43 : 11
                                                15,
                                                //pos44 : 12
                                                15,
                                                //pos1Fin : 13
                                                30*/
                                                //posFirst : 0
                                                0,
                                                //pos13 : 1
                                                5,
                                                //pos45 : 2
                                                3,
                                                //pos7 : 3
                                                4,
                                                //pos9 : 4
                                                5,
                                                //pos33 : 5
                                                5,
                                                //pos0 : 6
                                                3,
                                                //pos8 : 7
                                                4,
                                                //pos22 : 8 
                                                5,
                                                //pos4 : 9
                                                5,
                                                //pos1 : 10
                                                3,
                                                //pos43 : 11
                                                4,
                                                //pos44 : 12
                                                4,
                                                //pos1Fin : 13
                                                7
                                            };

        private static readonly int[][] WALK_FORWARD_DESTS = {
                                           //posFirst : 0
                                           new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
                                           //pos13 : 1
                                           new int[]{0, 0, 0, 150, 150, 500, -500, 1000, -1000, -650, 650, 150, 150},
                                           //pos45 : 2
                                           new int[]{0, 0, 0, 400, 250, 1100, -600, 2200, -1200, -1100, 750, 450, 250},
                                           //pos7 : 3
                                           new int[]{0, 0, 0, 0, 0, 900, -500, 1200, -1200, -450, 900, 0, -100},
                                           //pos9 : 4
                                           new int[]{0, 0, 0, -100, -50, 500, 250, 1200, -700, -800, 900, -200, -200},
                                           //pos33 : 5
                                           new int[]{0, 0, 0, -250, -250, 600, -300, 1200, -1700, -700, 1000, -300, -300},
                                           //pos0 : 6
                                           new int[]{0, 0, 0, -250, -400, 600, -1100, 1200, -2200, -700, 1100, -250, -450},
                                           //pos8 : 7
                                           new int[]{0, 0, 0, 0, 0, 500, -900, 1200, -1200, -900, 450, 100, 0},
                                           //pos22 : 8 
                                           new int[]{0, 0, 0, 50, 100, -250, -500, 700, -1200, -900, 800, 200, 200},
                                           //pos4 : 9
                                           new int[]{0, 0, 0, 250, 250, 300, -600, 1700, -1200, -1000, 700, 300, 300},
                                           //pos1 : 10
                                           new int[]{0, 0, 0, 400, 250, 1100, -600, 2200, -1200, -1100, 700, 450, 250},
                                           //pos43 : 11
                                           new int[]{0, 0, 0, -250, -250, 600, -900, 1200, -2100, -700, 1300, -300, 300},
                                           //pos44 : 12
                                           new int[]{0, 0, 0, 250, 250, 900, -600, 2100, -1200, -1300, 700, 300, 300},
                                           //pos1Fin : 13
                                           new int[]{0, 0, 0, 0, 0, 500, -500, 1000, -1000, -600, 600, 0, 0},
                                       };
            
        private int[] GetWALK_FORWARDDests()
        {
            Debug.WriteLine("postion ID {0}", positionID);
            //frameCountが現在のpositionIDのframe数に到達していない場合は、線形補完した値を使う
            switch (positionID)
            {
                case 0:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 0, 1);
                case 1:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 1, 2);
                case 2:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 2, 3);
                case 3:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 3, 4);
                case 4:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 4, 5);
                case 5:
                    Debug.WriteLine("current {0} next {1}", currentStatus, nextStatus);
                    if(frameCount == 0 && currentStatus != nextStatus){
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 5, 11);
                    }
                    else{
                        return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 5, 6);
                    }
                case 6:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 6, 7);
                case 7:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 7, 8);
                case 8:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 8, 9);
                case 9:
                    Debug.WriteLine("current {0} next {1} framecount {2}", currentStatus, nextStatus, frameCount);
                    if(frameCount == 1 && currentStatus != nextStatus){
                        Debug.WriteLine("change the flag");
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 9, 12);
                    }
                    else{
                        return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 9, 10);
                    }
                case 10:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 10, 3);
                case 11:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 11, 13);
                case 12:
                    return NormalTransition(WALK_FORWARD_DESTS, WALK_FORWARD_FRAMES, 12, 13);
                case 13:
                    finishFlag = true;
                    positionID = 0;
                    return WALK_FORWARD_DESTS[13];
            }

            //positionID
            int[] ret = WALK_FORWARD_DESTS[0];
            return ret;
        }

        private int[] GetWALK_BACKWARDDests()
        {
            int[] ret ={7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480};
            return ret;
        }
        
        private int[] GetWALK_RIGHTDests()
        {
            int[] ret ={7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480};
            return ret;
        }
        
        private int[] GetWALK_LEFTDests()
        {
            int[] ret ={7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480};
            return ret;
        }
        
        private int[] GetTURN_RIGHTDests()
        {
            int[] ret ={7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480};
            return ret;
        }

        private int[] GetTURN_LEFTDests()
        {
            int[] ret ={7500, 7500, 7500, 7530, 7470, 8000, 7000, 8500, 6500, 6900, 8100, 7520, 7480};
            return ret;
        }
    }
}


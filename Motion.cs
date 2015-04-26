using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            posVals[0] += 7500;
            posVals[1] += 7500;
            posVals[2] += 7500;
            posVals[3] += 7530;
            posVals[4] += 7470;
            posVals[5] += 8000;
            posVals[6] += 7000;
            posVals[7] += 8500;
            posVals[8] += 6500;
            posVals[9] += 6900;
            posVals[10] += 8100;
            posVals[11] += 7520;
            posVals[12] += 7480;

            return posVals;
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
                ret[i] = (int)((double)dests[nowID][i] + ratio * (double)(dests[nextID][i] - dests[nowID][i]));
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
            return InterPolatePositions(dests, frames, 0, 1);
        }

        //frameCount(経過フレーム数)から計算する感じです
        private int[] GetSTOPDests()
        {
            //これはおそらくこれで確定
            int[] ret ={0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            return ret;
        }

        private int[] WALK_FORWARD_FRAMES = {
                                                //posFirst : 0
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
                                                30
                                            };

        private int[][] WALK_FORWARD_DESTS = {
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
                    if(frameCount == 0 && currentStatus != nextStatus){
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
        //okapon
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

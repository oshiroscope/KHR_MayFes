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

        private int[] TURN_LEFT_FRAMES = {
                                                   //posFirst : 0
                                                   0,
                                                   //pos12 : 1
                                                   6,
                                                   //pos1 : 2
                                                   8,
                                                   //pos2 : 3
                                                   6,
                                                   //pos3 : 4
                                                   8,
                                                   //pos4 : 5
                                                   8
                                              };

        private static readonly int[][] TURN_LEFT_DESTS = {
                                                                //posfirst : 0
                                                                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                                                //pos12 : 1
                                                                new int[]{0, 0, 0, 100, 50, 500, -500, 1000, -1000, -650, 650, 100, 50},
                                                                //pos1 : 2
                                                                new int[]{0, 450, -150, 150, 0, 1000, -500, 2000, -1000, -1000, 650, 250, 0},
                                                                //pos2 : 3
                                                                new int[]{0, 600, -450, -100, -100, 500, -500, 1000, -1000, -650, 650, -100, -100},
                                                                //pos3 : 4
                                                                new int[]{0, 0, 0, -100, -100, 550, -1000, 1100, -2000, -700, 1000, -100, -150},
                                                                //pos4 : 5
                                                                new int[]{0, 0, 0, 0, 0, 500, -500, 1000, -1000, -600, 600, 0, 0},
                                                            };
        private int[] GetTURN_LEFTDests()
        {
            switch (positionID)
            {
                case 0:
                    return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 0, 1);
                case 1:
                    return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 1, 2);
                case 2:
                    return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 2, 3);
                case 3:
                    return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 3, 4);
                case 4:
                    if (frameCount == 0 && currentStatus != nextStatus)
                    {
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 4, 1);
                    }
                    else
                    {
                        return NormalTransition(TURN_LEFT_DESTS, TURN_LEFT_FRAMES, 4, 5);
                    }
                case 5:
                    finishFlag = true;
                    return TURN_LEFT_DESTS[5];
            }

            //positionID
            int[] ret = TURN_LEFT_DESTS[0];
            return ret;
        }
    }
}

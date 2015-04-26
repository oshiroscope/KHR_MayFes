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

        private int[] TURN_RIGHT_FRAMES = {
                                                   //posFirst : 0
                                                   0,
                                                   //pos12 : 1
                                                   3,
                                                   //pos1 : 2
                                                   4,
                                                   //pos2 : 3
                                                   4,
                                                   //pos3 : 4
                                                   4,
                                                   //pos4 : 5
                                                   4
                                              };

        private static readonly int[][] TURN_RIGHT_DESTS = {
                                                                //posfirst : 0
                                                                new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                                                //pos12 : 1
                                                                new int[]{0, 0, 0, -50, -100, 500, -500, 1000, -1000, -650, 650, -50,-100},
                                                                //pos1 : 2
                                                                new int[]{0, 150, -450, 0, -150, 500, -1000, 1000, -2000, -650, 1000, 0, -250},
                                                                //pos2 : 3
                                                                new int[]{0, 450, -600, 100, 100, 500, -500, 1000, -1000, -650, 650, 100, 100},
                                                                //pos3 : 4
                                                                new int[]{0, 0, 0, 100, 100, 1000, -550, 2000, -1100, -1000, 700, 100, 100},
                                                                //pos4 : 5
                                                                new int[]{0, 0, 0, 0, 0, 500, -500, 1000, -1000, -600, 600, 0, 0},
                                                            };
        private int[] GetTURN_RIGHTDests()
        {
            switch (positionID)
            {
                case 0:
                    return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 0, 1);
                case 1:
                    return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 1, 2);
                case 2:
                    return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 2, 3);
                case 3:
                    return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 3, 4);
                case 4:
                    if (frameCount == 1 && currentStatus != nextStatus)
                    {
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 4, 5);
                    }
                    else
                    {
                        return NormalTransition(TURN_RIGHT_DESTS, TURN_RIGHT_FRAMES, 4, 1);
                    }
                case 5:
                    positionID = 0;
                    finishFlag = true;
                    return TURN_RIGHT_DESTS[5];
            }

            //positionID
            int[] ret = TURN_RIGHT_DESTS[0];
            return ret;
        }
    }
}

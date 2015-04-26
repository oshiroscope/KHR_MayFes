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
        private int[] WALK_BACKWARD_FRAMES = {
                                                //posFirst : 0
                                                0,
                                                //pos13 : 1
                                                5,
                                                //pos43 : 2
                                                2,
                                                //pos45 : 3
                                                3,
                                                //pos3 : 4
                                                2,
                                                //pos7 : 5
                                                4,
                                                //pos9 : 6
                                                5,
                                                //pos33 : 7
                                                5,
                                                //pos0 : 8 
                                                3,
                                                //pos5 : 9
                                                2,
                                                //pos8 : 10
                                                4,
                                                //pos22 : 11
                                                5,
                                                //pos4 : 12
                                                5,
                                                //pos1 : 13
                                                3,
                                                //pos43 : 14
                                                4,
                                                //pos44 : 15
                                                4,
                                                //pos1Fin : 16
                                                7
                                            };

        private static readonly int[][] WALK_BACKWARD_DESTS = {
                                           //posFirst : 0
                                           new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
                                           //pos13 : 1
                                           new int[]{0, 0, 0, 150, 150, 500, -500, 1000, -1000, -500, 500, 150, 150},
                                           //pos43 : 2
                                           new int[]{0, 0, 0, 250, 250, 1000, -500, 2000, -1000, -800, 550, 350, 300},
                                           //pos45 : 3
                                           new int[]{0, 0, 0, 400, 250, 800, -600, 2300, -1200, -1200, 650, 450, 250},
                                           //pos3 : 4
                                           new int[]{0, 0, 0, 400, 250, 0, -600, 1300, -1200, -1100, 650, 450, 250},
                                           //pos7 : 5
                                           new int[]{0, 0, 0, 0, 0, 200, -900, 1200, -1200, -1000, 400, 0, -100},
                                           //pos9 : 6
                                           new int[]{0, 0, 0, -100, -50, 500, -900, 1200, -700, -700, -150, -200, -200},
                                           //pos33 : 7
                                           new int[]{0, 0, 0, -250, -250, 500, -1200, 1200, -1700, -700, 0, -300, -300},
                                           //pos0 : 8 
                                           new int[]{0, 0, 0, -250, -400, 600, -800, 1200, -2300, -650, 1200, -250, -450},
                                           //pos5 : 9
                                           new int[]{0, 0, 0, -250, -400, 600, 0, 1200, -1300, -650,1100, -250, -450},
                                           //pos8 : 10
                                           new int[]{0, 0, 0, 0, 0, 900, -200, 1200, -1200, -400, 1000, 100, 0},
                                           //pos22 : 11
                                           new int[]{0, 0, 0, 50, 100, 900, -500, 700, -1200, 150, 700, 200, 200},
                                           //pos4 : 12
                                           new int[]{0, 0, 0, 250, 250, 1200, -500, 1700, -1200, 0, 700, 300, 300},
                                           //pos1 : 13
                                           new int[]{0, 0, 0, 400, 250, 800, -600, 2300, -1200, -1200, 650, 450, 250},
                                           //pos43 : 14
                                           new int[]{0, 0, 0, -250, -250, 600, -900, 1200, -2100, -700, 1300, -300, -300},
                                           //pos44 : 15
                                           new int[]{0, 0, 0, 250, 250, 900, -600, 2100, -1200, -1300, 700, 300, 300},
                                           //pos1Fin : 16
                                           new int[]{0, 0, 0, 0, 0, 500, -500, 1000, -1000, -600, 600, 0, 0},
                                       };

        private int[] GetWALK_BACKWARDDests()
        {
            switch (positionID)
            {
                case 0:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 0, 1);
                case 1:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 1, 2);
                case 2:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 2, 3);
                case 3:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 3, 4);
                case 4:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 4, 5);
                case 5:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 5, 6);
                case 6:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 6, 7);
                case 7:
                    if (frameCount == 1 && currentStatus != nextStatus)
                    {
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 5, 14);
                    }
                    else
                    {
                        return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 7, 8);
                    }
                case 8:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 8, 9);
                case 9:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 9, 10);
                case 10:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 10, 11);
                case 11:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 11, 12);
                case 12:
                    if (frameCount == 1 && currentStatus != nextStatus)
                    {
                        changeFlag = true;
                    }

                    if (changeFlag)
                    {
                        return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 12, 15);
                    }
                    else
                    {
                        return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 12, 13);
                    }
                case 13:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 13, 4);
                case 14:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 14, 16);
                case 15:
                    return NormalTransition(WALK_BACKWARD_DESTS, WALK_BACKWARD_FRAMES, 15, 16);
                case 16:
                    positionID = 0;
                    finishFlag = true;
                    return WALK_BACKWARD_DESTS[16];
            }

            //positionID
            int[] ret = WALK_BACKWARD_DESTS[0];
            return ret;
        }
    }
}

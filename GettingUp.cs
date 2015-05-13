using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KHR_MayFes
{
    class GettingUp
    {
        private int frame = 0;
        private int id = 0;
        private bool _isGettingUp = false;

        public GettingUp()
        {
        }

        public bool isGettingUp()
        {
            return _isGettingUp;
        }

        public void start()
        {
            _isGettingUp = true;
        }

        private int[] AddNeutral(int[] dests)
        {
            int[] ret = new int[22];
            for (int i = 0; i < 22; i++)
            {
                ret[i] = dests[i] + 7500;
            }
            return ret;
        }


        public int[] FromOnFace()
        {
            Debug.WriteLine("frame : {0} ID : {1}", frame, id);
            frame++;
            if (frame == GETTING_UP_FROM_ON_FACE_FRAMES[id])
            {
                if (id == 8)//finish
                {
                    _isGettingUp = false;
                    id = 0;
                    frame = 0;
                    return AddNeutral(GETTING_UP_FROM_ON_FACE_DESTS[8]);
                }
                else
                {
                    id++;
                    frame = 0;
                }
            }
            return AddNeutral(GETTING_UP_FROM_ON_FACE_DESTS[id]);
        }

        public int[] FromOnBack()
        {
            frame++;
            if (frame == GETTING_UP_FROM_ON_BACK_FRAMES[id])
            {
                if (id == 8)
                {
                    _isGettingUp = false;
                    id = 0;
                    frame = 0;
                    return AddNeutral(GETTING_UP_FROM_ON_BACK_DESTS[8]);
                }
                else
                {
                    id++;
                    frame = 0;
                }
            }
            return AddNeutral(GETTING_UP_FROM_ON_BACK_DESTS[id]);
        }

        private int[] GETTING_UP_FROM_ON_FACE_FRAMES = {
                                                           //pos19
                                                           15,
                                                           //pos21
                                                           50,
                                                           //pos20
                                                           20,
                                                           //pos9
                                                           40,
                                                           //pos21
                                                           25,
                                                           //pos20
                                                           5,
                                                           //pos2
                                                           20,
                                                           //pos13
                                                           35,
                                                           //pos23
                                                           50
                                                       };

        private static readonly int[][] GETTING_UP_FROM_ON_FACE_DESTS = {
                                       
                                                                            //pos19
                                                                            new int[]{0,0,-2000,2000,1000,-1000,0,0,-3800,3800,0,0,0,0,500,-500,1000,-1000,-600,600,0,0},
                                                                            //pos21
                                                                            new int[]{0,0,1500,-1500,1000,-1000,0,0,-3800,3800,0,0,0,0,-800,800,1000,-1000,-600,600,0,0},
                                                                            //pos20
                                                                            new int[]{0,0,1500,-1500,0,0,0,0,-3500,3500,0,0,0,0,-800,800,1000,-1000,-600,600,0,0},
                                                                            //pos9
                                                                            new int[]{0,0,2700,-2700,0,0,0,0,0,0,0,0,0,0,1200,-1200,0,0,-1200,1200,0,0},
                                                                            //pos21
                                                                            new int[]{0,0,2700,-2700,200,-200,0,0,0,0,0,0,0,0,3200,-3200,0,0,300,-300,0,0},
                                                                            //pos20
                                                                            new int[]{0,0,2700,-2700,200,-200,0,0,0,0,0,0,0,0,3200,-3200,0,0,300,-300,0,0},
                                                                            //pos2
                                                                            new int[]{0,0,2700,-2700,200,-200,0,0,0,0,0,0,0,0,3250,-3250,0,0,1200,-1200,0,0},
                                                                            //pos13
                                                                            new int[]{0,0,1100,-1100,200,-200,0,0,0,0,0,0,0,0,2100,-2100,0,0,1000,-1000,0,0},
                                                                            //pos23
                                                                            new int[]{0,0,-800,800,200,-200,0,0,-2000,2000,0,0,0,0,500,-500,1000,-1000,-600,600,0,0},
                                                                        };

        private int[] GETTING_UP_FROM_ON_BACK_FRAMES = {

                                                       };

        private static readonly int[][] GETTING_UP_FROM_ON_BACK_DESTS = {
                                                                        };
    }
}

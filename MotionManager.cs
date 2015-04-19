using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHR_MayFes
{
    public enum MotionStatus
    {
        STOP = 0,
        WALK_FORWARD,
        WALK_BACKWARD,
        WALK_RIGHT,
        WALK_LEFT,
        TURN_RIGHT,
        TURN_LEFT
    }

    class MotionManager
    {
        public MotionManager()
        {
            //コンストラクタで必要そうなもの
        }

        /*
         * Wiiバランスボードから重心位置を取得し、
         * x,yの座標位置からどのモーションを操縦者が期待しているのかを特定する
         * ちなみに、今考えているのは
         * 前に進む：体を前に倒す
         * 後ろに進む：体を後ろに倒す
         * 右にかに歩き：体を右に倒す
         * 左にかに歩き：体を左に倒す
         * 右に旋回：体を右後ろに倒す
         * 左に旋回：体を左後ろに倒す
         */ 
        private MotionStatus GetMotionState(){
            //ここを実装してね☆
            return MotionStatus.STOP;
        }
    }
}

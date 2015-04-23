using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using WiimoteLib;//wimote用

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

    public partial class MotionManager
    {
        private MotionStatus currentStatus;
        private MotionStatus oldStatus;
        private MotionStatus nextStatus;
        private Coord vertex;
        private bool changeFlag;
        private bool finishFlag;
        private int frameCount;
        private int positionID;
        private int wiiBBFrameCount;
        private float weight; 
        
        //wiimoteのインスタンス
        private Wiimote wm;

        public MotionManager()
        {
            //コンストラクタで必要そうなもの

            //とりあえず最初は止まってるでしょう
            currentStatus = MotionStatus.STOP;
            oldStatus = MotionStatus.STOP;
            nextStatus = MotionStatus.STOP;
            wiiBBFrameCount = 0;
            changeFlag = false;
            finishFlag = true;

            frameCount = 0;
            positionID = 0;
            weight = 0;
            wm = new Wiimote();
            //Wiimoteの接続
            this.wm.Connect();
            //イベント関数の登録
            this.wm.WiimoteChanged += wm_WiimoteChanged;
            
        }

        /*
         * 今の周期のサーボ指定角度を返す関数
         * サーボの並びは
         * WAIST = 0, //腰(回転方向不明)
         * LEFT_HIP_YAW = 1, //左股間節ヨー方向回転
         * RIGHT_HIP_YAW = 2, //右股間節ヨー方向回転
         * LEFT_HIP_ROLL = 3, //左股関節ロール方向回転
         * RIGHT_HIP_ROLL = 4, //右股関節ロール方向回転
         * LEFT_HIP_PITCH = 5, //左股関節ピッチ方向回転
         * RIGHT_HIP_PITCH = 6, //右股関節ピッチ方向回転
         * LEFT_KNEE = 7, //左膝
         * RIGHT_KNEE = 8, //右膝
         * LEFT_ANKLE_PITCH = 9, //左足首ピッチ方向回転
         * RIGHT_ANKLE_PITCH = 10, //右足首ピッチ方向回転
         * LEFT_ANKLE_ROLL = 11, //左足首ロール方向回転
         * RIGHT_ANKLE_ROLL = 12, //右足首ロール方向回転
         */
        public int[] GetLowerServoDests()
        {
            oldStatus = nextStatus;
            nextStatus = GetMotionState();
            Debug.WriteLine("vartex : {0} {1}", vertex.x, vertex.y);
            Debug.WriteLine("MotionStatus : {0}", nextStatus);
            /*if (!changeFlag && currentStatus != nextStatus)
            {
                changeFlag = true;
            }*/

            if (finishFlag == true)
            {
                finishFlag = false;
                changeFlag = false;
                frameCount = 0;
                currentStatus = nextStatus;
            }

            frameCount++;

            return GetMotionDests(currentStatus);
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
            // Wiiバランスボードから重心のX Y座標を取得する関数
            //TODO ここでモーションに変換
            if (weight < 10) return MotionStatus.STOP;
            if (vertex.x > 10) {
                return MotionStatus.TURN_RIGHT;
            }
            else if (vertex.x < -10) {
                return MotionStatus.TURN_LEFT;
            }
            else{
                if (vertex.y > 5){
                    return MotionStatus.WALK_BACKWARD;
                }
                else if (vertex.y < -5){
                    return MotionStatus.WALK_FORWARD;
                }
                else{
                    return MotionStatus.STOP;
                }
            }         
        }

        /*
         * 二次元座標を表すためだけのアレ
         * 中身のアレはWiiBB(バランスボード)の返し方のアレにアレ
         */
        struct Coord{
            public double x;
            public double y;
        }

        void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            //WiimoteStateの値を取得
            if (wiiBBFrameCount%10 == 0){
                  WiimoteState ws = args.WiimoteState;

            //ここでWiiからアレがアレ
                  vertex.x = ws.BalanceBoardState.CenterOfGravity.X;
                  vertex.y = ws.BalanceBoardState.CenterOfGravity.Y;
                  weight = ws.BalanceBoardState.WeightKg;
                 // Debug.WriteLine("vartex : {0} {1}", vartex.x, vartex.y);
            }
            wiiBBFrameCount++;
        }
      
    }
}

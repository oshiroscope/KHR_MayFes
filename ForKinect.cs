/* 
 * Visual C#でWindowクラスを外に出すやり方がよくわからなかったので、
 * ソースを分けるためにMainWindowを部分クラスにして、Kinect関連の動作を
 * ここに書くこととする
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace KHR_MayFes
{
    public partial class MainWindow : Window
    {
        //Kinect関連のコンポーネント
        private KinectSensor kinect;
        private BodyFrameReader bodyFrameReader;
        private Body[] bodies;


        /*
         * Kinect初期化関数
         * MainWindow.csのコンストラクタで呼び出す
         */
        private void InitializeKinect()
        {
            try
            {
                kinect = KinectSensor.GetDefault();
                if (kinect == null)
                {
                    throw new Exception("Kinectを開けません");
                }

                kinect.Open();

                // Bodyを入れる配列を作る
                bodies = new Body[kinect.BodyFrameSource.BodyCount];

                // ボディーリーダーを開く
                bodyFrameReader = kinect.BodyFrameSource.OpenReader();
                bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;
            }
            catch (Exception ex)
            {
                StatusText.Text = ex.Message;
                Close();
            }
        }

        
        private int loop_cnt = 0;
        /*
         * Kinectから、アバウト30Hzでボディ情報が飛んでくる。(イベント)
         * これはそのイベントが起こった時に呼び出される関数。(イベントハンドラ)
         * 内部では、BoneManagerにBody情報を投げるのと、MainWindowへの描画を行う。
         * 今回はこのイベントをすべてのプログラムのトリガーにする
         */
        private void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            //bodiesの更新
            UpdateBodyFrame(e);

            if(loop_cnt % 1 == 0){
                //Bodyを用いて実際に命令を送る
                foreach (var body in bodies.Where(b => b.IsTracked))
                {
                    SetBody(body);
                    break;
                }
            }
            //bodyを描画
            DrawBodyFrame();

            loop_cnt++;
        }

        /*
         * Kinectから飛んできたBody情報をもとに、手元のbodiesを更新するための関数。
         * なんか更新してるんだなーくらいの理解で大丈夫
         */ 
        private void UpdateBodyFrame(BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame == null)
                {
                    return;
                }

                // ボディデータを取得する
                bodyFrame.GetAndRefreshBodyData(bodies);
            }
        }

        /*
         * Kinectから飛んできたBody情報をMainWindowに描画するための関数。
         * イベントハンドラ内部で呼び出す
         * Canvasというクラスを使って円、楕円を描画する
         */
        private void DrawBodyFrame()
        {
            //CanvasBodyはMainWindowのメンバCanvasオブジェクト
            CanvasBody.Children.Clear();

            foreach (var body in bodies.Where(b => b.IsTracked))
            {
                foreach (var joint in body.Joints)
                {
                    DrawEllipse(joint.Value, 10, Brushes.Blue);
                }
                break; //インデックスが一番若いもののみをトレース
            }
        }


        /*
         * 円を描画する関数
         * \param joint Kinectから送られてくるBodyの中に格納された関節のオブジェクト
         * \param R 円の直径(ピクセル)
         * \param brush Brush,主に色の指定に使う
         */
        private void DrawEllipse(Joint joint, int R, Brush brush)
        {
            //楕円のオブジェクトを置く
            var ellipse = new Ellipse()
            {
                Width = R,
                Height = R,
                Fill = brush,
            };

            //関節の位置情報から、2次元のWindow上の位置に変換
            var point = kinect.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
            if ((point.X < 0) || (point.Y < 0))
            {
                return;
            }

            //Depth座標系で円を配置する
            // R / 2 ってなんで引くんだっけ
            Canvas.SetLeft(ellipse, point.X - (R / 2));
            Canvas.SetTop(ellipse, point.Y - (R / 2));

            //描画
            CanvasBody.Children.Add(ellipse);
        }
    }
}

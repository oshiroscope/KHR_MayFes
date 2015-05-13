using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Kinect;

namespace KHR_MayFes
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoneManager boneManager;
        private ServoManager servoManager;
        private SerialPortManager serialPortManager;
        private MotionManager motionManager;
        private GettingUp gettingUp;

        public MainWindow()
        {
            InitializeComponent();
            boneManager = new BoneManager();
            servoManager = new ServoManager();
            serialPortManager = new SerialPortManager();
            motionManager = new MotionManager(ref serialPortManager);
            gettingUp = new GettingUp();
        }

        private FallingStatus fallingStatus = FallingStatus.STANDING;

        private void SetBody(Body body)
        {
            if (!gettingUp.isGettingUp())
            {
                if (serialPortManager.isOpen()) {
                    fallingStatus = getFallingStatus();
                    switch (fallingStatus)
                    {
                        case FallingStatus.ONBACK:
                            gettingUp.start();
                            break;
                        case FallingStatus.ONFACE:
                            gettingUp.start();
                            break;
                        case FallingStatus.STANDING:
                        //boneManagerにセット
                        boneManager.setBones(body.Joints);
                        //上半身をkinectの情報をもとに設定
                        servoManager.SetUpperBody(boneManager.getBones());
                        //下半身をモーションから設定
                        servoManager.SetLowerBody(motionManager.GetLowerServoDests());
                        //サーボコマンド列を生成
                        byte[] cmd = servoManager.generateCommand();
                        serialPortManager.sendMessage(cmd);
                        break;
                    }
                }
                else
                {
                    //boneManagerにセット
                    boneManager.setBones(body.Joints);
                    //上半身をkinectの情報をもとに設定
                    servoManager.SetUpperBody(boneManager.getBones());
                    //下半身をモーションから設定
                    servoManager.SetLowerBody(motionManager.GetLowerServoDests());
                    //サーボコマンド列を生成
                    byte[] cmd = servoManager.generateCommand();
                    serialPortManager.sendMessage(cmd);
                }
            }
            else
            {
                switch (fallingStatus)
                {
                    case FallingStatus.ONFACE:
                        servoManager.SetWholeBody(gettingUp.FromOnFace());
                        byte[] cmd = servoManager.generateCommand();
                        serialPortManager.sendMessage(cmd);
                        break;
                    case FallingStatus.ONBACK:
                    default:
                        break;
                }
            }
        }

        private enum FallingStatus{
            STANDING,
            ONBACK,//仰向け
            ONFACE //うつぶせ
        };

        private FallingStatus getFallingStatus()
        {
            byte[] recv = serialPortManager.readADC();
            int xAxis = ((int)(recv[3]) * 256 + (int)(recv[2]));
            XAxis.Text = "XAxis :" + xAxis.ToString();
            if (xAxis > 380)
            {
                return FallingStatus.ONFACE;
            }
            else if (xAxis < 180)
            {
                return FallingStatus.ONBACK;
            }
            else
            {
                return FallingStatus.STANDING;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ForKinect.cs内で定義されたKinect初期化関数を呼び出し
            InitializeKinect();
        }


        private void ConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "connecting...";
            StatusText.Text = serialPortManager.Open();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

    }
}

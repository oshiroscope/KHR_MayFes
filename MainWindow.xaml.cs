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

        public MainWindow()
        {
            InitializeComponent();
            boneManager = new BoneManager();
            servoManager = new ServoManager();
        }

        private void SetBody(Body body)
        {
            //boneManagerにセット
            boneManager.setBones(body.Joints);
            //上半身をkinectの情報をもとに設定
            servoManager.SetUpperBody(boneManager.getBones());
            //下半身をモーションから設定
            servoManager.SetLowerBody();
            //サーボコマンド列を生成
            byte[] cmd = servoManager.generateCommand();
            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ForKinect.cs内で定義されたKinect初期化関数を呼び出し
            InitializeKinect();
        }


        private void ConnectButtonClicked(object sender, RoutedEventArgs e)
        {
             
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    }
}

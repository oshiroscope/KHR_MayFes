using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Rcb4;


namespace KHR_MayFes
{
    /*
     * シリアルポートの設定、入出力を請負うクラス
     */ 
    public class SerialPortManager
    {
        public SerialPort MyPort;

        //城のPCではいつもCOM5なので暫定的に決め打ち
        public SerialPortManager()
        {
            MyPort = new SerialPort("COM5", 115200, Parity.Even, 8, StopBits.One);
            MyPort.ReadTimeout = 1000;
        }

        //シリアルポートを開く
        public string Open(){
            try{
                if(!MyPort.IsOpen){
                    MyPort.Open();
                }
            }
            catch(Exception e){
                return e.Message;
            }
            return "connected!";
        }

        //シリアルポートを閉じる
        public string Close(){
            try{
                if (MyPort.IsOpen){
                    MyPort.Close();
                }
            }
            catch (Exception e){
                return e.Message; 
            }
            return "disconnected";
        }

        //コマンドを送る
        public void sendMessage(byte[] msg)
        {
            if (MyPort.IsOpen)
            {
                /*
                 * 返答は4バイト
                 * RCB-4 コマンドリファレンス p18,19
                 */ 
                byte[] rx = new byte[4];
                Command.Synchronize(MyPort, msg, ref rx);   
            }
        }
    }
}

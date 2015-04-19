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
using System.Windows.Media.Media3D;
using System.Diagnostics;
using Microsoft.Kinect;


namespace KHR_MayFes
{
    /*
     * 数学関連の使える関数をまとめたもの
     * static宣言してあるので使用する際に実体はいらない
     */
    class MathUtil
    {
        /*
         * Kinectがくれる関節情報2つからそれらをつなぐ3次元のベクトルを生成する関数
         * b - a を返す
         */ 
        public static Vector3D JointsToVector3D(Joint a, Joint b)
        {
            Vector3D ret = new Vector3D(
                b.Position.X - a.Position.X,
                b.Position.Y - a.Position.Y,
                b.Position.Z - a.Position.Z);
            return ret;
        }

        /*
         * ベクトルを射影する関数
         * destにsrcを射影したベクトルを返す
         */ 
        public static Vector3D ProjectVector3D(Vector3D src, Vector3D dest)
        {
            dest.Normalize();
            return (Vector3D.DotProduct(src, dest) * dest);
        }

        /*
         * aとbの間の角度をdirectionの方向に応じて返す関数
         * 正直どういう式か覚えてない
         */ 
        public static double getDirectionalAngle(Vector3D a, Vector3D b, Vector3D direction)
        {
            Vector3D crossProduct = Vector3D.CrossProduct(a,b);
            Debug.WriteLine("angle : {0}", Vector3D.AngleBetween(crossProduct, direction));
            if (Vector3D.AngleBetween(crossProduct, direction) <= 180)
            {
                return Math.Atan2(Vector3D.CrossProduct(a,b).Length, Vector3D.DotProduct(a, b));
            }
            else
            {
                return - Math.Atan2(Vector3D.CrossProduct(a,b).Length, Vector3D.DotProduct(a, b));
            }
        }

        //radianをdegreeに変換する関数 ↓で使う
        private static double RadToDegree(double angleRad)
        {
            return angleRad * (180.0 / Math.PI);
        }

        //なんか外積から角度計算するやつだった気がする
        public static double getAxisAngle(Vector3D fromDirection, Vector3D toDirection, Vector3D axis)
        {
            axis.Normalize();
            Vector3D fromDirectionProjected = fromDirection - axis * Vector3D.DotProduct(axis, fromDirection);
            fromDirectionProjected.Normalize();
            Vector3D toDirectionProjected = toDirection - axis * Vector3D.DotProduct(axis, toDirection);
            toDirectionProjected.Normalize();
            return RadToDegree(Math.Acos(Vector3D.DotProduct(fromDirectionProjected, toDirectionProjected)) *
                (Vector3D.DotProduct(Vector3D.CrossProduct(axis, fromDirectionProjected), toDirectionProjected) < 0.0 ? -1 : 1));
        }
    }
}
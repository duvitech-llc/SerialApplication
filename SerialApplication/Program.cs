using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialApplication
{
    class Program
    {
        public static bool bRunnable;

        static void Main(string[] args)
        {

            Console.CancelKeyPress += Console_CancelKeyPress;
            System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort("COM8", 9600, System.IO.Ports.Parity.None);
            sp.ReadTimeout = 500;
            sp.WriteTimeout = 500;
                        
            sp.Open();
            bRunnable = true;
            string toSend = "Hi George!";
            byte[] arr = new byte[toSend.Length +1];
            arr[0] = (byte)toSend.Length;
            int j = 1;
            for (int x = 0; x < toSend.Length; x++ )
            {
                arr[j] = (byte)toSend[x];
                j++;
            }

            // buffer ready to send

            while (bRunnable)
            {
                try
                {
                    int b = sp.ReadByte();
                    Console.Write(b + " ");
                }
                catch (Exception ex) { }

                System.Threading.Thread.Sleep(10);

                sp.Write(arr, 0, toSend.Length + 1);
            }

            sp.Close();
            
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            bRunnable = false;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}

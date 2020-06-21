using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace Ontcijferen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Create a UnicodeEncoder to convert between byte array and string.
        UnicodeEncoding ByteConverter = new UnicodeEncoding();

        //Create byte array to hold decrypted data.
        byte[] decryptedData;

        //Create byte array to hold encrypted data.
        byte[] encryptedData;

        //Open button
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                encryptedData = File.ReadAllBytes(openFileDialog.FileName);

                //Show encrypted text in textBox
                encryptedText.Text = ByteConverter.GetString(encryptedData);
            }

            //Enable Decrypt button when lines array is poplulated
            if (encryptedData != null)
            {
                decrypt.IsEnabled = true;
            }
        }

        //Decrypt button
        private void decrypt_Click(object sender, RoutedEventArgs e)
        {
            //Pass the data to DECRYPT and a boolean flag specifying no OAEP padding.
            decryptedData = RSADecrypt(encryptedData, false);

            //Display decrypted text in textBox
            decryptedText.Text = ByteConverter.GetString(decryptedData);
        }

        //Decrypt function
        public static byte[] RSADecrypt(byte[] DataToDecrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Creates an RSA object containing the public & private key from the XML string.
                    RSA.FromXmlString(MyPrivateKey());

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        private static string MyPrivateKey()
        {
            return "<RSAKeyValue><Modulus>xncZR7YHbH+Q22EefnVkArlIfGYvtSp6BtUSTjTuAuksOn+gACxcZHfEQGqp/qczHW67zpuNxNNrOzV4Ga2m617rtxuqvURELG9RK3SujmnZBkafMwfHc1X2Ft2sz5S2DnrxtHbQl1GVOHYu/avvjUC5UR5XNloDEKQ5Pr8r/aE=</Modulus><Exponent>AQAB</Exponent><P>/1eq1B3HNru/Xde7BESxD+yzmg+1wahpRnehExzwC+96YGM4cPCfjht9NWJD3LwpwZ2oDqTDNydUKvWNhOKlew==</P><Q>xvnvf/pZODXJewzWK3ME/iIgHKe+qSvXPj5NMMFMZF9DNKdpWCCwYV4jlnH0a3rZEt/h+V8+d9cP+inDE1dokw==</Q><DP>Ye0PONZKxnTuiWDo+lQVy9OtdZI81I5wAXRzs87PSOSI/FpRFQ9TQb7NICIVgJwxL231O9h4fbh5kRgnNCVv7Q==</DP><DQ>aGoJ41elcBjRpQ/kZ6KREScpQMBKDg5iglhBO3+ELfvLkZr0bfmhdUboV+9uuuQZe40e0TTI3CxwRu+ZXdH1nw==</DQ><InverseQ>eI0GLNQjEuASpXeCy4ZugSvIM+5SOyxRg9Q0Kk0jaHPfzb63JZ9ZmBQVzrtLdSwlSnjCDm9YUq93UdYW5LibLA==</InverseQ><D>lk/10Z5IGSC0sbLoyZZXXMi/4ZfzYh79tNcDuj/UVFgNq3Cl2fu/LkiXVsJyZfB0gkIv89dUlFIb0Qg6IhinZhyx/EIbBLV8lXGEZ4L8AfnjNlBPkMqGV2KsWRPEpSTZKyg0zqzRRltD0qavgPh5+DzpOWvrMOSl8Y+CMCFG89E=</D></RSAKeyValue>";
        }


    }
}

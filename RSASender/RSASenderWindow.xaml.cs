using RSASender.Service;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RSASender
{
    /// <summary>
    /// Interaction logic for RSASenderWindow.xaml
    /// </summary>
    public partial class RSASenderWindow : Window
    {
        RSAEncryptionService encryptionService;

        public RSASenderWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                RSAParameters rSAParameters = new RSAParameters
                {
                    Modulus = Convert.FromBase64String(ModulusTextBox.Text),
                    Exponent = Convert.FromBase64String(ExponentTextBox.Text)
                };

                encryptionService = new RSAEncryptionService(rSAParameters);
                MessageBox.Show("The Public key have been set, you can now encrypt messages");
            }
            catch(CryptographicException)
            {
                MessageBox.Show("There is something wrong with the provided values");
            }
        }

        private void EncryptMessage_Click(object sender, RoutedEventArgs e)
        {
            EncryptedMessage.Text = Convert.ToBase64String(encryptionService.Encrypt(Encoding.UTF8.GetBytes(PlainTextMessage.Text)));
        }
    }
}

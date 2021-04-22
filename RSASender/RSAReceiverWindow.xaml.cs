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
    /// Interaction logic for RSASender.xaml
    /// </summary>
    public partial class RSAReceiverWindow : Window
    {
        RSAEncryptionService encryptionService;


        public RSAReceiverWindow()
        {
            InitializeComponent();
        }

        private void GenerateKeyPair_Click(object sender, RoutedEventArgs e)
        {
            encryptionService = new RSAEncryptionService();

            MessageBox.Show("Created a new Encryption Service");

            ModulusTextBox.Text = Convert.ToBase64String(encryptionService.PublicKey.Modulus);
            ExponentTextBox.Text = Convert.ToBase64String(encryptionService.PublicKey.Exponent);
            DTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.D);
            DPTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.DP);
            DQTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.DQ);
            InverseQTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.InverseQ);
            PTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.P);
            QTextBox.Text = Convert.ToBase64String(encryptionService.PrivateKey.Q);
        }

        private void EncryptMessage_Click(object sender, RoutedEventArgs e)
        {
            EncryptedMessage.Text = Convert.ToBase64String(encryptionService.Encrypt(Encoding.UTF8.GetBytes(PlainTextMessage.Text)));

            MessageBox.Show(EncryptedMessage.Text);


        }

        private void DecryptMessage_Click(object sender, RoutedEventArgs e)
        {
            PlainTextMessage.Text = Encoding.UTF8.GetString(encryptionService.Decrypt(Convert.FromBase64String(EncryptedMessage.Text)));

            MessageBox.Show(PlainTextMessage.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RSAParameters rSAParameters = new RSAParameters
                {
                    Modulus = Convert.FromBase64String(ModulusTextBox.Text),
                    Exponent = Convert.FromBase64String(ExponentTextBox.Text),
                    D  = Convert.FromBase64String(DTextBox.Text),
                    DP = Convert.FromBase64String(DPTextBox.Text),
                    DQ = Convert.FromBase64String(DQTextBox.Text),
                    InverseQ = Convert.FromBase64String(InverseQTextBox.Text),
                    P = Convert.FromBase64String(PTextBox.Text),
                    Q = Convert.FromBase64String(QTextBox.Text)
                };

                encryptionService = new RSAEncryptionService(rSAParameters);

                MessageBox.Show("Your private key have been set.");
            }
            catch(FormatException)
            {
                MessageBox.Show("The Values provided did not uphold the Base64 encoding");
            }
            catch(CryptographicException)
            {
                MessageBox.Show("The private Key that was provided was not good try again");
            }

             

        }
    }
}

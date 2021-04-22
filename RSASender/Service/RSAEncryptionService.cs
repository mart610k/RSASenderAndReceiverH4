using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using RSASender.Exceptions;

namespace RSASender.Service
{
    public class RSAEncryptionService
    {
        RSACryptoServiceProvider cryptoServiceProvider;

        public RSAParameters PublicKey { 
            get 
            {
                return cryptoServiceProvider.ExportParameters(false);
            }
        }

        public RSAParameters PrivateKey
        {
            get
            {
                return cryptoServiceProvider.ExportParameters(true);
            }
        }

        public RSAEncryptionService(int keySize = 2048)
        {
            cryptoServiceProvider = new RSACryptoServiceProvider(keySize);
        }

        public RSAEncryptionService(RSAParameters key)
        {
            cryptoServiceProvider = new RSACryptoServiceProvider();
            cryptoServiceProvider.ImportParameters(key);
        }

        public byte[] Encrypt(byte[] dataToEncrypt)
        {
            return cryptoServiceProvider.Encrypt(dataToEncrypt, false);
        }

        public byte[] Decrypt(byte[] dataToDecrypt)
        {
            if (PrivateKeyPresent())
            {
                return cryptoServiceProvider.Decrypt(dataToDecrypt,false);

            }
            else
            {
                throw new PrivateKeyNotPresentException();
            }
        }

        public bool PrivateKeyPresent()
        {
            try
            {
                RSAParameters privatekey = PrivateKey;
                return true;

            }
            catch(CryptographicException CE)
            {
                return false;
            }
            
        }

        public static RSAParameters GenerateKeys(int keysize = 2048)
        {
            using (RSA rsa = RSA.Create(keysize))
            {
                return rsa.ExportParameters(true);
            }
        }
        
    }
}

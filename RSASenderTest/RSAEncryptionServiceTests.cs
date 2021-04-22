using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using RSASender.Service;
using System.Security.Cryptography;
using RSASender.Exceptions;

namespace RSASenderTest
{
    class RSAEncryptionServiceTests
    {
        [Test]
        public void TestEncryptDontReturnNull()
        {
            RSAEncryptionService service = new RSAEncryptionService();

            string testString = "Hello world encrypt me";

            Assert.NotNull(service.Encrypt(Encoding.UTF8.GetBytes(testString)));
        }

        [Test]
        public void TestDecryptDontReturnNull()
        {
            RSAEncryptionService service = new RSAEncryptionService();

            string testString = "Hello world encrypt me";

            byte[] encrypted = service.Encrypt(Encoding.UTF8.GetBytes(testString));

            Assert.NotNull(service.Decrypt(encrypted));
        }

        [Test]
        public void TestEncryptedStringCanBeDecrypted()
        {
            RSAEncryptionService service = new RSAEncryptionService();

            string testString = "Hello world encrypt me";

            byte[] encrypted = service.Encrypt(Encoding.UTF8.GetBytes(testString));

            Assert.AreEqual(testString, Encoding.UTF8.GetString(service.Decrypt(encrypted)));
        }

        [Test]
        public void TestGetPublicKeyDontReturnNull()
        {
            RSAEncryptionService service = new RSAEncryptionService();

            Assert.NotNull(service.PublicKey);
        }

        [Test]
        public void TestGetPrivateKeyDontReturnNull()
        {
            RSAEncryptionService service = new RSAEncryptionService();

            Assert.NotNull(service.PrivateKey);
        }


        [Test]
        public void TestCreatingRSAKeyCanBeSetInConstructor()
        {
            RSAParameters rSAParameters = RSAEncryptionService.GenerateKeys();

            RSAEncryptionService service = new RSAEncryptionService(rSAParameters);

            //Check Public key
            Assert.AreEqual(rSAParameters.Modulus, service.PublicKey.Modulus);
            Assert.AreEqual(rSAParameters.Exponent, service.PublicKey.Exponent);

            //Check Private Key
            Assert.AreEqual(rSAParameters.Modulus, service.PrivateKey.Modulus);
            Assert.AreEqual(rSAParameters.Exponent, service.PrivateKey.Exponent);
            Assert.AreEqual(rSAParameters.P, service.PrivateKey.P);
            Assert.AreEqual(rSAParameters.Q, service.PrivateKey.Q);
            Assert.AreEqual(rSAParameters.DP, service.PrivateKey.DP);
            Assert.AreEqual(rSAParameters.DQ, service.PrivateKey.DQ);
            Assert.AreEqual(rSAParameters.D, service.PrivateKey.D);
            Assert.AreEqual(rSAParameters.InverseQ, service.PrivateKey.InverseQ);


        }

        [Test]
        public void TestGeneratedKeysAreNotEqual()
        {
            RSAParameters rSAParameters = RSAEncryptionService.GenerateKeys();
            
            RSAParameters rSAParameters2 = RSAEncryptionService.GenerateKeys();

            Assert.AreNotEqual(rSAParameters.Modulus, rSAParameters2.Modulus);
        }

        [Test]
        public void TestEncryptWorksWhenOnlyPublicKeyIsPresent()
        {
            RSAParameters privateAndPublicKey = RSAEncryptionService.GenerateKeys();

            RSAEncryptionService baseLineService = new RSAEncryptionService(privateAndPublicKey);
            string textToEncrypt = "Testing this encrypts";

            byte[] baselineEncrypted = baseLineService.Encrypt(Encoding.UTF8.GetBytes(textToEncrypt));

            RSAParameters publicKey = new RSAParameters
            {
                Modulus = privateAndPublicKey.Modulus,
                Exponent = privateAndPublicKey.Exponent
            };
            RSAEncryptionService encryptionService = new RSAEncryptionService(publicKey);

            byte[] encryptedTest = encryptionService.Encrypt(Encoding.UTF8.GetBytes(textToEncrypt));

            //Assert That the encrypted text comes back as equals
            Assert.AreEqual(baseLineService.Decrypt(baselineEncrypted), baseLineService.Decrypt(encryptedTest));
        }

        [Test]
        public void TestDecryptingWithOnlyPublicKeyThrowsException()
        {
            RSAParameters privateAndPublicKey = RSAEncryptionService.GenerateKeys();

            RSAEncryptionService baseLineService = new RSAEncryptionService(privateAndPublicKey);
            string textToEncrypt = "Testing this encrypts";

            byte[] baselineEncrypted = baseLineService.Encrypt(Encoding.UTF8.GetBytes(textToEncrypt));

            RSAParameters publicKey = new RSAParameters
            {
                Modulus = privateAndPublicKey.Modulus,
                Exponent = privateAndPublicKey.Exponent
            };
            RSAEncryptionService encryptionService = new RSAEncryptionService(publicKey);

            Assert.Throws<PrivateKeyNotPresentException>(() => encryptionService.Decrypt(baselineEncrypted));
        }

        [Test]
        public void TestPrivateKeyReturnsFalseWhenPrivateKeyNotPresent()
        {
            RSAParameters privateAndPublicKey = RSAEncryptionService.GenerateKeys();


            RSAParameters publicKey = new RSAParameters
            {
                Modulus = privateAndPublicKey.Modulus,
                Exponent = privateAndPublicKey.Exponent
            };
            RSAEncryptionService baseLineService = new RSAEncryptionService(publicKey);

            Assert.False(baseLineService.PrivateKeyPresent());
        }

        [Test]
        public void TestPrivateKeyReturnsFalseWhenPrivateKeyPresent()
        {
            RSAParameters privateAndPublicKey = RSAEncryptionService.GenerateKeys();

            RSAEncryptionService baseLineService = new RSAEncryptionService(privateAndPublicKey);

            Assert.True(baseLineService.PrivateKeyPresent());
        }

    }
}

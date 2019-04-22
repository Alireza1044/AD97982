using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exam1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1.Tests
{
    [TestClass()]
    public class EncryptionTests
    {
        [TestMethod]
        public void PassphraseEncryptTest()
        {
            string msg = "sjlkajdlsafkjsadlfksdlfkja";
            Encryption e = new Encryption("kill", 'a', 'z', false);
            string enc = e.Encrypt(msg);
            string dec = e.Decrypt(enc);
            Assert.AreEqual(msg, dec);
        }

        [TestMethod]
        public void PassphraseBigVocabEncryptTest()
        {
            string msg = "abcd  Tas., a";
            Encryption e = new Encryption("kill", ' ', 'z', false);
            string enc = e.Encrypt(msg);
            string dec = e.Decrypt(enc);
            Assert.AreEqual(msg, dec);
        }

        [TestMethod()]
        public void CharEncryptTest()
        {
            TestCharEncryption('A', 'Z');
            TestCharEncryption('A', 'z');
            TestCharEncryption('0', 'z');
        }

        private static void TestCharEncryption(char minChar, char maxChar)
        {
            char key = (char) ((minChar + maxChar) / 2);
            Encryption e = new Encryption(key.ToString(), minChar, maxChar, false);
            foreach (char c in Enumerable.Range(minChar, maxChar - minChar + 1))
            {
                char enc = e.Encrypt(c, key);
                Assert.AreNotEqual(enc, c);
                char dec = e.Decrypt(enc, key);
                Assert.AreEqual(c, dec);
            }
        }


        [TestMethod()]
        public void EncryptNonLetterTest()
        {
            Encryption e = new Encryption("A", 'A', 'Z', true);
            var enc = e.Encrypt(' ', 'R');
            Assert.AreEqual(enc, ' ');

            enc = e.Encrypt('5', 'R');
            Assert.AreEqual(enc, '5');

            enc = e.Encrypt(',', 'R');
            Assert.AreEqual(enc, ',');

        }

    }
}
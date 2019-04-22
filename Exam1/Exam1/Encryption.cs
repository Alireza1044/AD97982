using System;
using System.Linq;
using System.IO;

namespace Exam1
{
    public class Encryption
    {
        private string passphrase;
        private char MinChar;
        private char MaxChar;
        private bool IgnoreOOV;

        public Encryption(string passphrase, char minChar, char maxChar, bool ignoreOOV)
        {
            this.MinChar = minChar;
            this.MaxChar = maxChar;
            this.VocabSize = MaxChar - MinChar + 1;
            this.passphrase = passphrase;
            this.IgnoreOOV = ignoreOOV;

            if (passphrase.Min() < MinChar || passphrase.Max() > maxChar)
                throw new InvalidDataException(
                    $"All chars in pass phrase must be in range {minChar}-{maxChar}");
        }

        public string Encrypt(string msg, int maxLen=int.MaxValue)
        {
            char[] chars = msg.ToCharArray();
            char[] eChars = new char[chars.Length];
            for(int i=0; i<chars.Length && i<maxLen; i++)
            {
                eChars[i] = Encrypt(chars[i], passphrase[i % passphrase.Length]);
            }
            return new string(eChars);
        }

        public readonly int VocabSize;

        public string Decrypt(string msg, int maxLen=int.MaxValue)
        {
            char[] eChars = msg.ToCharArray();
            char[] chars = new char[Math.Min(eChars.Length,maxLen)];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = Decrypt(eChars[i], passphrase[i % passphrase.Length]);
            }
            return new string(chars);
        }

        public char Encrypt(char c, char key)
        {
            // Only encrypt letters
            if (c < MinChar || c > MaxChar)
                if (IgnoreOOV)
                    return c;
                else
                    throw new ArgumentOutOfRangeException($"{c} not in {MinChar}-{MaxChar}");

            int zeroBaseACEncrypted = (ZeroBase(c) + ZeroBase(key)) % VocabSize;
            int encryptedAscii = VocabBase(zeroBaseACEncrypted);
            return (char)encryptedAscii;
        }

        public char Decrypt(char c, char key)
        {
            // Only encrypt/decrypt letters in range
            if (c < MinChar || c > MaxChar)
                if (IgnoreOOV)
                    return c;
                else
                    throw new ArgumentOutOfRangeException($"{c} not in {MinChar}-{MaxChar}");

            int zeroBaseDecrypted = (ZeroBase(c) - ZeroBase(key) + VocabSize) % VocabSize;
            return (char) VocabBase(zeroBaseDecrypted);
        }

        private int ZeroBase(int c) => c - MinChar;
        private int VocabBase(int c) => c + MinChar;
    }
}
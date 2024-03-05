using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUGI_Cipher
{
    internal class MUGIAlgorithm
    {
        ulong[] Sbox = {
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5,
            0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
            0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0,
            0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc,
            0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
            0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a,
            0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
            0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0,
            0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
            0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b,
            0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
            0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85,
            0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5,
            0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
            0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17,
            0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
            0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88,
            0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
            0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c,
            0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
            0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9,
            0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
            0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6,
            0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
            0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e,
            0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94,
            0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
            0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68,
            0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16};

        public ulong K1 = 0x1234567890123456;
        public ulong K2 = 0xABCDEABCDEABCDEA;
        public ulong I = 0x0987654321098765;
        const ulong C0 = 0x6A09E667F3BCC908;
        const ulong C1 = 0xBB67AE8584CAA73B;
        const ulong C2 = 0x3C6EF372FE94F82B;
        ulong[] MUGI(int n)
        {
            ulong[] a = new ulong[3];
            ulong[] b = new ulong[16];
            ulong[] emptyB = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            a[0] = K1;
            a[1] = K2;
            a[2] = (a[0] << 7) ^ (a[1] >> 7) ^ C0;

            ulong[] aK = new ulong[3];
            int i;
            for (i = 0; i < 16; i++)
            {
                aK = (p(new ulong[] { a[0], 0, 0 }, emptyB));
                b[15 - i] = p(a, emptyB)[0];
            }

            ulong[] newA = new ulong[3];
            newA[0] = (ulong)(aK[0] ^ I);
            newA[1] = (ulong)(aK[1] ^ 0);
            newA[2] = (ulong)(aK[2] ^ (I << 7) ^ 0 ^ C0);
            aK = newA;


            for (i = 0; i < 15; i++)
                aK = p(aK, emptyB);

            a = p(aK, emptyB);

            //Инициализируем b ключом.
            for (i = 0; i < 3; i++)
            {
                b[i] = K1;
                b[i + 1] = K2;
                b[i + 2] = (b[i] << 7) ^ (b[i + 1] >> 7) ^ C0;
            }
            b[15] = K1;

            ulong[] tempB = b;
            ulong[] tempA = a;
            for (i = 0; i < 15; i++)
            {
                tempA = p(tempA, tempB);
                tempB = lamb(tempA, tempB);
            }
            a = tempA;
            b = tempB;
            //Сейчас мы генерим слова, количество которых указано в методе.
            ulong[] result = new ulong[n];
            for (i = 0; i < n; i++)
            {
                result[i] = a[2];
                a = p(a, b);
                b = lamb(a, b);
            }
            return result;
        }
        ulong[] p(ulong[] a, ulong[] b)
        {
            ulong[] newA =
                [a[1],
                    a[2] ^ F(a[1], b[4]) ^ C1,
                    a[0] ^ F(a[1], b[10] << 17) ^ C2];
            return newA;
        }

        ulong[] lamb(ulong[] a, ulong[] b)
        {
            ulong[] newB = new ulong[16];
            for (int i = 0; i < 16; i++)
            {
                if (i == 0)
                {
                    newB[0] = b[15] ^ a[0];
                    continue;
                }
                if (i == 4)
                {
                    newB[4] = b[3] ^ b[7];
                    continue;
                }
                if (i == 10)
                {
                    newB[10] = b[9] ^ (b[13] << 32);
                    continue;
                }
                newB[i] = b[i - 1];
            }
            return newB;
        }

        ulong F(ulong unitA, ulong unitB)
        {
            ulong[] masks = {
            0b0000000000000000000000000000000000000000000000000000000011111111,
            0b0000000000000000000000000000000000000000000000001111111100000000,
            0b0000000000000000000000000000000000000000111111110000000000000000,
            0b0000000000000000000000000000000011111111000000000000000000000000,
            0b0000000000000000000000001111111100000000000000000000000000000000,
            0b0000000000000000111111110000000000000000000000000000000000000000,
            0b0000000011111111000000000000000000000000000000000000000000000000,
            0b1111111100000000000000000000000000000000000000000000000000000000};
            ulong[] P = new ulong[8];
            for (int i = 0; i < 8; i++)
            {
                ulong temp1 = (unitA & masks[i]) >>> (8 * i);
                ulong temp2 = (unitB & masks[i]) >>> (8 * i);

                P[i] = Sbox[temp1 ^ temp2];
            }
            ulong QH = 0, QL = 0;
            for (int i = 0; i < 4; i++)
            {
                QL = QL | (P[i] << (8 * i));
                QH = QH | (P[i] << (8 * (i + 4)));
            }
            QL = M(QL);
            QH = M(QH);
            return (QH & masks[0]) | (QH & masks[1]) | (QL & masks[2]) | (QL & masks[3]) | (QL & masks[0]) | (QL & masks[1]) | (QH & masks[2]) | (QH & masks[3]);
        }

        ulong M(ulong unit)
        {
            uint mask0 = 0b00000000000000000000000011111111;
            uint mask1 = 0b00000000000000001111111100000000;
            uint mask2 = 0b00000000111111110000000000000000;
            uint mask3 = 0b11111111000000000000000000000000;

            uint[] x = { (uint)(unit & mask0), (uint)(unit & mask1), (uint)unit & mask2, (uint)unit & mask3 };
            uint[][] matrix = [[0x02, 0x03, 0x01, 0x01],
                [0x01, 0x02, 0x03, 0x01],
                [0x01, 0x01, 0x02, 0x03],
                [0x03, 0x01, 0x01, 0x02]];
            uint[] newX = new uint[4];
            for (int i = 0; i < 4; i++)
            {
                newX[i] = 0;
                for (int j = 0; j < 4; j++)
                    newX[i] += matrix[i][j] * x[j];
            }
            for (int i = 1; i < 4; i++)
                x[0] = x[0] | (x[i] << (i * 8));
            return (ulong)x[0];
        }
        public ulong[] Encrypt(string plainText)
        {
            List<ulong> result = new List<ulong>();
            ulong temp = 0;
            int counter = 0;
            foreach (char letter in plainText)
            {
                temp +=
                    ((ulong)BitConverter.GetBytes(letter)[0] << (counter * 8)) +
                    ((ulong)BitConverter.GetBytes(letter)[1] << ((counter + 1) * 8));
                counter += 2;
                if (counter == 4)
                {
                    counter = 0;
                    result.Add(temp);
                    temp = 0;
                }
            }
            if (temp != 0)
                result.Add(temp);
            ulong[] cipher = MUGI(result.Count);
            for (int i = 0; i < result.Count; i++)
                result[i] = result[i] ^ cipher[i];
            return result.ToArray();
        }
        public string Decrypt(ulong[] cipherText)
        {
            ulong[] cipher = MUGI(cipherText.Length);
            StringBuilder answer = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i++)
            {
                cipherText[i] ^= cipher[i];
                answer.Append(Encoding.ASCII.GetString(BitConverter.GetBytes(cipherText[i])));
            }

            return answer.ToString();
        }
    }
}

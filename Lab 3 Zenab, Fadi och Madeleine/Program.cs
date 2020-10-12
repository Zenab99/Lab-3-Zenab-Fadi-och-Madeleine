using System;
using System.IO;
using System.Text;

namespace Lab_3_Zenab__Fadi_och_Madeleine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Found one path with argument.");
                return;
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found.");
                return;
            }
            byte[] fileWithBytes = File.ReadAllBytes(args[0]);
            Console.WriteLine(CheckFil(fileWithBytes));
        }

        static string CheckFil(byte[] bytes)
        {
            if (CheckBMP(bytes))
            {
                int w = BitConverter.ToUInt16(bytes, 18);
                int h = BitConverter.ToUInt16(bytes, 22);
                string bmpId = "";
                for (int i = 0; i < 2; i++)
                {
                    bmpId += bytes[i].ToString("X2");
                }

                if (bmpId == "424D")
                {
                    return $"This is a .bmp image. Resolution: : {w} x {h} pixels.";
                }
                else
                {
                    return "Not a bmp image";
                }

            }
            else if (CheckPNG(bytes))
            {
                StringBuilder pngstring = new StringBuilder();

                byte[] bytesSize = bytes[16..24].AsSpan().ToArray();
                Array.Reverse(bytesSize);

                uint w = BitConverter.ToUInt32(bytesSize, 4);
                uint h = BitConverter.ToUInt32(bytesSize, 0);

                string pngId = "";
                for (int i = 0; i < 8; i++)
                {
                    pngId += bytes[i].ToString("X2");
                }

                if (pngId == "89504E470D0A1A0A")
                {
                    pngstring.Append($"This is a .png image. Resolution: : {w} x {h} pixels.");
                    return pngstring.ToString();              
                }
                else
                {
                    return "Not a png image";
                }
            }
            else
            {
                return "This is not a valid .bmp or .png file!";
            }
        }

        static bool CheckPNG(byte[] bytes)
        {
            byte[] checkValues = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

            for (int i = 0; i < 8; i++)
            {
                if (bytes[i] != checkValues[i])
                {
                    return false;
                }
            }
            return true;
        }
        static bool CheckBMP(byte[] bytes)
        {
            byte[] cv = { 0x42, 0x4D };
            for (int i = 0; i < 2; i++)
            {
                if (bytes[i] != cv[i])
                {
                    return false;
                }
            }
            return true;
        }            
    }
}


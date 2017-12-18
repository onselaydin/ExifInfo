using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ExifInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Image image = Image.FromFile(Path.Combine(Environment.CurrentDirectory
                , "20171212_143604.jpg"));
            Console.WriteLine(image.GetExif().ToString());
            Console.ReadLine();
        }
    }


    static class ImageExtensions
    {
        enum Exif
        {
            ExposureTime=0x829A,
            ISOSpeedRatings=0x8827,
            Model=0x110,
            FocalLength=0x920A,
            MeteringMode=0x9207
        }

        public static ImageExif GetExif(this Image source)
        {
            PropertyItem evItem = source.GetPropertyItem((int)Exif.ExposureTime);
            PropertyItem isoItem = source.GetPropertyItem((int)Exif.ISOSpeedRatings);
            PropertyItem modelItem = source.GetPropertyItem((int)Exif.Model);
            PropertyItem focalLenghtItem = source.GetPropertyItem((int)Exif.FocalLength);
            return new ImageExif
            {
                ExposureTime = new ExposureTime
                {
                    X = BitConverter.ToInt32(evItem.Value, 0),
                    Y = BitConverter.ToInt32(evItem.Value, 4)
                },
                ISO = BitConverter.ToInt16(isoItem.Value, 0),
                Model = Encoding.ASCII.GetString(modelItem.Value),
                FocalLength = BitConverter.ToInt32(focalLenghtItem.Value, 0)
            };
        }
    }

    class ImageExif
    {
        public ExposureTime ExposureTime { get; set; }
        public short ISO { get; set; }
        public string Model { get; set; }
        public int FocalLength { get; set; }
        public override string ToString()
        {
            return string.Format("ISO\t\t{0}\nEV\t\t{1}\nFocal Length\t{2}\nModel\t\t{3}\n", ISO, ExposureTime.ToString(), FocalLength, Model);

        }
    }

    class ExposureTime
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            return string.Format("{0}/{1}", X, Y);
        }

     }
}

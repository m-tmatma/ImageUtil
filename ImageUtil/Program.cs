using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                return;
            }
            var input = args[0];
            var outdir = args[1];
            var testout = input + ".png";

            if (!Directory.Exists(outdir))
            {
                Directory.CreateDirectory(outdir);
            }

            var bmp = (Bitmap)Image.FromFile(input);
            bmp.Save(testout, System.Drawing.Imaging.ImageFormat.Png);

            var width = bmp.Size.Width;
            var height = bmp.Size.Height;
            var sx = 16;
            var sy = 16;

            var index = 1;
            for (var y = 0; y < height; y += sy)
            {
                for (var x = 0; x < width; x += sx)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}", index, x, y, x + sx, y + sy);
                    var cloneRect = new RectangleF(x, y, sx, sy);
                    using (var cloneBitmap = bmp.Clone(cloneRect, System.Drawing.Imaging.PixelFormat.Format4bppIndexed))
                    {
                        var outfile = Path.Combine(outdir, index.ToString() + ".bmp");
                        cloneBitmap.Save(outfile, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    index++;
                }
            }


            index = 1;
            //var firstpath = Path.Combine(outdir, index.ToString() + ".bmp");
            //var firstimg  = (Bitmap)Image.FromFile(firstpath);
            //Bitmap imgMerge = new Bitmap(width, height);
            var imgMerge = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(imgMerge);
            for (var y = 0; y < height; y += sy)
            {
                for (var x = 0; x < width; x += sx)
                {
                    var outfile = Path.Combine(outdir, index.ToString() + ".bmp");
                    using (var bmpTmp = (Bitmap)Image.FromFile(outfile))
                    {
                        var cloneRect = new RectangleF(x, y, sx, sy);
                        g.DrawImage(bmpTmp, cloneRect);
                    }
                    index++;
                }
            }
            var outRect = new RectangleF(0, 0, width, height);
            var imgMerge2 = imgMerge.Clone(outRect, System.Drawing.Imaging.PixelFormat.Format4bppIndexed);
            var mergedbmp = input + ".merged.bmp";
            imgMerge2.Save(mergedbmp, System.Drawing.Imaging.ImageFormat.Bmp);
         }
    }
}

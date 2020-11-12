using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGraphicsLab06
{
    class ZBuffer
    {
        private static int ProjMode = 0;
        public static Bitmap Z_buffer(int width, int heigh, List<Polyhedron> scene, List<Color> colors, int projMode = 0)
        {
            ProjMode = projMode;

            Bitmap newImg = new Bitmap(width, heigh);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    newImg.SetPixel(i, j, Color.White);

            float[,] zbuff = new float[width, heigh];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    zbuff[i, j] = float.MinValue;

            List<List<List<Point3D>>> rasterizedScene = new List<List<List<Point3D>>>();
            for (int i = 0; i < scene.Count; i++)
            {
                rasterizedScene.Add(RasterizingPolyhedron.Rasterize(scene[i], false));
            }

            var centerX = width / 2;
            var centerY = heigh / 2;

            int ind = 0;
            for (int i = 0; i < rasterizedScene.Count; i++)
            {
                //Смещение по центру фигуры
                var figureLeftX = rasterizedScene[i].Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.X));
                var figureLeftY = rasterizedScene[i].Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.Y));
                var figureRightX = rasterizedScene[i].Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.X));
                var figureRightY = rasterizedScene[i].Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.Y));
                var figureCenterX = (figureRightX - figureLeftX) / 2;
                var figureCenterY = (figureRightY - figureLeftY) / 2;

                Random r = new Random();

                for (int j = 0; j < rasterizedScene[i].Count; j++)
                {
                    List<Point3D> curr = rasterizedScene[i][j];
                    foreach (Point3D point in curr)
                    {
                        int x = (int)(point.X + centerX - figureCenterX);
                        int y = (int)(point.Y + centerY - figureCenterY);
                        if (x < width && y < heigh && x > 0 && y > 0)
                        {
                            if (point.Z > zbuff[x, y])
                            {
                                zbuff[x, y] = point.Z;
                                newImg.SetPixel(x, y, colors[ind % colors.Count]);
                            }
                        }
                    }
                    ind++;
                }
            }
            return newImg;
        }
    }
}


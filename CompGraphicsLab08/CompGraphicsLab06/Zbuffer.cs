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

        public static Bitmap Z_buffer(int width, int heigh, List<Polyhedron> scene, List<Color> colors)
        {
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
                rasterizedScene.Add(Rasterize(scene[i]));
            }

            var centerX = width / 2;
            var centerY = heigh / 2;

            int ind = 0;
            for (int i = 0; i < rasterizedScene.Count; i++)
            {
                //Смещение по центру фигуры
                //Тоже, конечно, так себе решение, но лучше, чем было
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
                                newImg.SetPixel(x, y, colors[ind]);
                            }
                        }
                    }
                    ind++;
                }
            }
            return newImg;
        }

        private static List<List<Point3D>> Rasterize(Polyhedron polyhedron)
        {
            List<List<Point3D>> rasterized = new List<List<Point3D>>();
            foreach (var facet in polyhedron.Faces)
            {
                List<Point3D> currentFac = new List<Point3D>();
                List<Point3D> facetPoint3Ds = new List<Point3D>();
                for (int i = 0; i < facet.Count; i++)
                {
                    facetPoint3Ds.Add(polyhedron.Vertexes[facet[i]]);
                }
                currentFac.AddRange(RasterizeShape(facetPoint3Ds));
                rasterized.Add(currentFac);
            }
            return rasterized;
        }

        private static List<Point3D> RasterizeShape(List<Point3D> points)
        {
            List<Point3D> res = new List<Point3D>();
            List<List<Point3D>> triangles = Triangulate(points);
            foreach (var triangle in triangles)
            {
                res.AddRange(RasterizeTriangle(PrepareCoords(triangle)));
            }
            return res;
        }

        private static List<Point3D> RasterizeTriangle(List<Point3D> points)
        {
            List<Point3D> res = new List<Point3D>();

            points.Sort((point1, point2) => point1.Y.CompareTo(point2.Y));
            var rpoints = points.Select(point => (X: (int)Math.Round(point.X), Y: (int)Math.Round(point.Y), Z: (int)Math.Round(point.Z))).ToList();

            var x01 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[1].Y, rpoints[1].X);
            var x12 = Interpolate(rpoints[1].Y, rpoints[1].X, rpoints[2].Y, rpoints[2].X);
            var x02 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[2].Y, rpoints[2].X);

            var z01 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[1].Y, rpoints[1].Z);
            var z12 = Interpolate(rpoints[1].Y, rpoints[1].Z, rpoints[2].Y, rpoints[2].Z);
            var z02 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[2].Y, rpoints[2].Z);

            x01.RemoveAt(x01.Count - 1);
            List<int> x012 = x01.Concat(x12).ToList();

            z01.RemoveAt(z01.Count - 1);
            List<int> z012 = z01.Concat(z12).ToList();

            int middle = x012.Count / 2;
            List<int> leftX, rightX, leftZ, rightZ;
            if (x02[middle] < x012[middle])
            {
                leftX = x02;
                leftZ = z02;
                rightX = x012;
                rightZ = z012;
            }
            else
            {
                leftX = x012;
                leftZ = z012;
                rightX = x02;
                rightZ = z02;
            }

            int y0 = rpoints[0].Y;
            int y2 = rpoints[2].Y;

            for (int ind = 0; ind <= y2 - y0; ind++)
            {

                int XL = leftX[ind];
                int XR = rightX[ind];

                List<int> intCurrZ = Interpolate(XL, leftZ[ind], XR, rightZ[ind]);

                for (int x = XL; x < XR; x++)
                {
                    res.Add(new Point3D(x, y0 + ind, intCurrZ[x - XL]));
                }
            }

            return res;
        }

        private static List<List<Point3D>> Triangulate(List<Point3D> points)
        {
            List<List<Point3D>> res = new List<List<Point3D>>();
            if (points.Count == 3)
                return new List<List<Point3D>> { points };

            for (int i = 2; i < points.Count; i++)
            {
                res.Add(new List<Point3D> { points[0], points[i - 1], points[i] });
            }

            return res;
        }

        private static List<int> Interpolate(int i0, int d0, int i1, int d1)
        {
            if (i0 == i1)
            {
                return new List<int> { d0 };
            }
            List<int> res = new List<int>();

            float step = (d1 - d0) * 1.0f / (i1 - i0);
            float value = d0;
            for (int i = i0; i <= i1; i++)
            {
                res.Add((int)value);
                value += step;
            }

            return res;
        }

        public static List<Point3D> PrepareCoords(List<Point3D> init)
        {
            var projection = new Projection();
            return projection.Project3(init, 0);
        }

    }
}


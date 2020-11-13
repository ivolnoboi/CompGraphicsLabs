using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    class GouraudShading
    {
        private static double ModelLambert(Point3D vertex, Point3D normal, Point3D light)
        {
            Point3D rayLight = new Point3D(vertex.X - light.X, vertex.Y - light.Y, vertex.Z - light.Z);
            double cos = Vectors.cosBetweenVectors(rayLight, normal);
            return cosToBrightness(cos);
        }

        private static double cosToBrightness(double cos)
        {
            return (cos + 1) / 2;
        }
        private static void calculate_shading(Polyhedron polyhedron, Point3D light)
        {
            // Добавление нормали к каждой вершине
            Dictionary<int, Point3D> pointNormal = new Dictionary<int, Point3D>(); // индекс точки - нормаль
            for (int i = 0; i < polyhedron.Vertexes.Count; i++)
            {
                List<List<int>> neededFaces = polyhedron.Faces.Where(x => x.Contains(i)).ToList();
                pointNormal.Add(i, Vectors.CalculateNormal(neededFaces, polyhedron));
            }

            // Вычисление цвета по модели Ламберта в каждой вершине
            for (int i = 0; i < polyhedron.Vertexes.Count; i++)
            {
                polyhedron.Vertexes[i].illumination = ModelLambert(polyhedron.Vertexes[i], pointNormal[i], light);
            }
        }

        public static Bitmap Gouraud(int width, int heigh, Polyhedron pl, Color color, Point3D ligth, int projMode = 0)
        {
            calculate_shading(pl, ligth); // вычисление яркостей
            RasterizingPolyhedron.ProjMode = projMode;

            Bitmap newImg = new Bitmap(width, heigh);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    newImg.SetPixel(i, j, Color.White);

            float[,] zbuff = new float[width, heigh];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    zbuff[i, j] = float.MinValue;

            // Многогранник <-- Грань <-- Точка - освещенность 
            List<List<Point3D>> rasterizedPolyhedron = RasterizingPolyhedron.Rasterize(pl, true);

            var centerX = width / 2;
            var centerY = heigh / 2;

            //Смещение по центру фигуры
            var figureLeftX = rasterizedPolyhedron.Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.X));
            var figureLeftY = rasterizedPolyhedron.Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.Y));
            var figureRightX = rasterizedPolyhedron.Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.X));
            var figureRightY = rasterizedPolyhedron.Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.Y));
            var figureCenterX = (figureRightX - figureLeftX) / 2;
            var figureCenterY = (figureRightY - figureLeftY) / 2;

            for (int i = 0; i < rasterizedPolyhedron.Count; i++)
            {
                List<Point3D> curr = rasterizedPolyhedron[i]; // текущая грань
                foreach (Point3D point in curr)
                {
                    int x = (int)(point.X + centerX - figureCenterX);
                    int y = (int)(point.Y + centerY - figureCenterY);
                    if (x < width && y < heigh && x > 0 && y > 0)
                    {
                        if (point.Z > zbuff[x, y])
                        {
                            zbuff[x, y] = point.Z;
                            newImg.SetPixel(x, y, Color.FromArgb((int)(color.R * point.illumination), (int)(color.G * point.illumination), (int)(color.B * point.illumination)));
                        }
                    }
                }
            }
            return newImg;
        }
    }
}

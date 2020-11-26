using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CompGraphicsInd02_Savelev
{
    enum ElemType
    {
        Circle = 1,
        Cube,
        Plain
    };
    enum MaterialType
    {
        Matte = 1,
        Mirror,
        Transparent
    }

    class Elem
    {
        public Elem()
        {

        }
    }

    class Sphere : Elem
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
        public Sphere(Point c, double r)
        {
            Center = c;
            Radius = r;
        }
    }

    class Cube : Elem
    {
        public Point MinPointValue { get; set; }
        public Point MaxPointValue { get; set; }
        public Cube(Point min, Point max)
        {
            MinPointValue = min;
            MaxPointValue = max;
        }
    }

    class Plain : Elem
    {
        public Point MinPoint { get; set; }
        public Point MaxPoint { get; set; }
        public Point Normal { get; set; }
        public Plain(Point _MinPoint, Point _MaxPoint, Point _Normal)
        {
            MinPoint = _MinPoint;
            MaxPoint = _MaxPoint;
            Normal = _Normal;
        }
    }

    class SceneElement
    {
        public ElemType Type { get; set; }
        public Elem Element { get; set; }
        public Color AmbientColor { get; set; }
        public MaterialType Material { get; set; }
        public SceneElement(ElemType _Type, Elem _Elem, Color _Color, MaterialType _Material)
        {
            Type = _Type;
            Element = _Elem;
            AmbientColor = _Color;
            Material = _Material;
        }
    }

    class LightSource
    {
        public Point Position { get; set; }
        public double Intens { get; set; }
        public LightSource(Point _Position, double _Intens)
        {
            Position = _Position;
            Intens = _Intens;
        }
    }

    class RayTracing
    {

        public static Bitmap rayTracing(int width, int heigh, List<SceneElement> scene, List<LightSource> lightSources)
        {
            Point lookup = new Point(0, 0, -20);
            Bitmap newImg = new Bitmap(width, heigh);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigh; j++)
                {
                    Point point = get3DCoords(i, j, width, heigh);
                    Color color = Ray(lookup, Camera.normalize(point), scene, lightSources, 3);
                    newImg.SetPixel(i, j, color);
                }
            }
            return newImg;
        }

        private static Color Ray(Point lookup, Point ray, List<SceneElement> scene, List<LightSource> lights, int recursion)
        {
            SceneElement closest = null;
            double closestT = double.NaN;
            closestElem(lookup, ray, scene, out closest, out closestT);
            if (closest == null)
            {
                return Color.Black;
            }

            Point closestPoint = new Point(lookup.x + closestT * ray.x, lookup.y + closestT * ray.y, lookup.z + closestT * ray.z);

            Point normal = null;

            if (closest.Type == ElemType.Circle)
            {
                Sphere sphere = (Sphere)closest.Element;
                normal = Camera.normalize(new Point(closestPoint.x - sphere.Center.x, closestPoint.y - sphere.Center.y, closestPoint.z - sphere.Center.z));
            }
            else
            {
                Plain plain = (Plain)closest.Element;
                normal = plain.Normal;
            }
            Color initial = closest.AmbientColor;
            double intens = recalcLight(closestPoint, normal, lights, scene);
            Color calculated = Color.FromArgb((int)(initial.R * intens), (int)(initial.G * intens), (int)(initial.B * intens));
            if (recursion == 0 || closest.Material == MaterialType.Matte)
            {
                return calculated;
            }
            else
            {
                Point reflectedRay = null;
                if (closest.Material == MaterialType.Mirror)
                {
                    reflectedRay = Camera.normalize(reflect(new Point(-ray.x, -ray.y, -ray.z), normal));
                }
                else if (closest.Material == MaterialType.Transparent)
                {
                    reflectedRay = Camera.normalize(refract(ray, normal));
                }
                Color reflectedColor = Ray(closestPoint, reflectedRay, scene, lights, recursion - 1);

                Color res = Color.FromArgb((int)(calculated.R * 0.3 + reflectedColor.R * 0.7),
                    (int)(calculated.G * 0.3 + reflectedColor.G * 0.7),
                    (int)(calculated.B * 0.3 + reflectedColor.B * 0.7));
                //Color res = reflectedColor;
                return res;
            }
        }

        private static void closestElem(Point lookup, Point ray, List<SceneElement> scene, out SceneElement closest, out double closestT)
        {
            closest = null;
            closestT = double.NaN;
            foreach (SceneElement sceneElement in scene)
            {
                double intersection = intersects(lookup, ray, sceneElement);
                if (!double.IsNaN(intersection))
                {
                    if (closest == null)
                    {
                        closest = sceneElement;
                        closestT = intersection;
                    }
                    else
                    {
                        if (intersection < closestT)
                        {
                            closestT = intersection;
                            closest = sceneElement;
                        }
                    }
                }
            }
        }

        private static double recalcLight(Point point, Point normal, List<LightSource> lights, List<SceneElement> scene)
        {
            double i = 0;
            foreach (var light in lights)
            {
                Point l = new Point(light.Position.x - point.x, light.Position.y - point.y, light.Position.z - point.z);

                SceneElement shadowElem = null;
                double shadowT = 0;
                closestElem(point, l, scene, out shadowElem, out shadowT);

                if (shadowElem != null && shadowElem.Type != ElemType.Plain)
                {
                    continue;
                }

                double normalMult = Camera.scalar(normal, l);
                if (normalMult > 0)
                {
                    i += light.Intens * normalMult / (mod(normal) * mod(l));
                }
            }
            return i;
        }

        private static double intersects(Point lookup, Point ray, SceneElement element)
        {
            switch (element.Type)
            {
                case (ElemType.Circle):
                    {
                        Sphere sphere = (Sphere)element.Element;
                        Point OC = new Point(lookup.x - sphere.Center.x,
                            lookup.y - sphere.Center.y,
                            lookup.z - sphere.Center.z);

                        double k1 = Camera.scalar(ray, ray);
                        double k2 = 2 * Camera.scalar(OC, ray);
                        double k3 = Camera.scalar(OC, OC) - sphere.Radius * sphere.Radius;

                        double D = k2 * k2 - 4 * k1 * k3;
                        if (D < 0.000001)
                        {
                            return Double.NaN;
                        }

                        double t1 = (-k2 + Math.Sqrt(D)) / (2 * k1);
                        double t2 = (-k2 - Math.Sqrt(D)) / (2 * k1);

                        if (t1 <= 0.000001)
                        {
                            return double.NaN;
                        }

                        return t2 > 0.000001 ? t2 : t1;
                    }
                case (ElemType.Cube):
                case (ElemType.Plain):
                    {
                        Plain plain = (Plain)element.Element;

                        Point normal = Camera.normalize(plain.Normal);

                        Point diff = new Point(lookup.x - plain.MaxPoint.x, lookup.y - plain.MaxPoint.y, lookup.z - plain.MaxPoint.z);

                        double prod1 = Camera.scalar(diff, normal);
                        double prod2 = Camera.scalar(ray, normal);
                        double prod3 = -prod1 / prod2;

                        if (prod3 < 0.000001)
                        {
                            return double.NaN;
                        }

                        Point interPoint = new Point(lookup.x + prod3 * ray.x, lookup.y + prod3 * ray.y, lookup.z + prod3 * ray.z);
                        if (pointInPlane(plain, interPoint))
                        {
                            return prod3;
                        }
                        else
                        {
                            return double.NaN;
                        }
                    }
                default:
                    return double.NaN;
            }
        }

        private static Point reflect(Point ray, Point normal)
        {
            double t = 2 * Camera.scalar(ray, normal);
            return new Point(t * normal.x - ray.x, t * normal.y - ray.y, t * normal.z - ray.z);
        }

        private static Point refract(Point ray, Point normal)
        {
            const double n1 = 1.1;
            const double n2 = 1;
            Point sn = Camera.normalize(Camera.scalar(ray, normal) < 0 ? normal : new Point(-normal.x, -normal.y, -normal.z));
            Point rd = Camera.normalize(ray);

            double inC1 = -Camera.scalar(sn, rd);
            double inN = inC1 > 0 ? n1 / n2 : n2 / n1;
            double inC2 = Math.Sqrt(Math.Max(1 - inN * inN * (1 - inC1 * inC1), 0));

            return new Point(ray.x * inN + normal.x * (inN * inC1 - inC2),
                ray.y * inN + normal.y * (inN * inC1 - inC2),
                ray.z * inN + normal.z * (inN * inC1 - inC2));
        }

        private static Point get3DCoords(int x, int y, int width, int heigh)
        {
            double xPrep = (x - width / 2) * (100.0 / width);
            double yPrep = ((y - heigh / 2) * (-1)) * (100.0 / heigh);
            return new Point(yPrep, xPrep, 100);
        }

        private static double mod(Point point)
        {
            return Math.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);
        }

        private static bool pointInPlane(Plain plain, Point point)
        {
            bool xval = (point.x - Math.Max(plain.MaxPoint.x, plain.MinPoint.x) <= 0.00001) && (point.x - Math.Min(plain.MaxPoint.x, plain.MinPoint.x) >= -0.00001);
            bool yval = (point.y - Math.Max(plain.MaxPoint.y, plain.MinPoint.y) <= 0.00001) && (point.y - Math.Min(plain.MaxPoint.y, plain.MinPoint.y) >= -0.00001);
            bool zval = (point.z - Math.Max(plain.MaxPoint.z, plain.MinPoint.z) <= 0.00001) && (point.z - Math.Min(plain.MaxPoint.z, plain.MinPoint.z) >= -0.00001);

            return xval && yval && zval;
        }

    }
}

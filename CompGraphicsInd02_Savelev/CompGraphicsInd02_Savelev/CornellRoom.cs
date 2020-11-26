using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace CompGraphicsInd02_Savelev
{
    public partial class CornellRoom : Form
    {
        public CornellRoom()
        {
            InitializeComponent();

            Color cube1Col = Color.Khaki;
            MaterialType cube1Material = MaterialType.Matte;
            Point cube1Center = new Point(5, -15, 35);
            double cube1Radius = 5;
            Color cube2Col = Color.MistyRose;
            MaterialType cube2Material = MaterialType.Mirror;
            Point cube2Center = new Point(-10, 0, 50);
            double cube2Radius = 5;
            List<SceneElement> elements = new List<SceneElement>
            {
                //Шарики
                new SceneElement(ElemType.Circle, new Sphere(new Point(-18, -10, 42), 5), Color.Aquamarine, MaterialType.Transparent),
                new SceneElement(ElemType.Circle, new Sphere(new Point(-15, 15, 48), 6), Color.Green, MaterialType.Mirror),
                new SceneElement(ElemType.Circle, new Sphere(new Point(-18, -20, 55), 6), Color.Red, MaterialType.Matte),
                //Комната
                new SceneElement(ElemType.Plain, new Plain(new Point(-25, -25,  70), new Point( 25,  25, 70), new Point(0, 0, -1)), Color.AliceBlue, MaterialType.Mirror),
                new SceneElement(ElemType.Plain, new Plain(new Point(-25, -25, -70), new Point( 25, -25, 70), new Point(0, 1, 0)), Color.Green, MaterialType.Matte),
                new SceneElement(ElemType.Plain, new Plain(new Point(-25,  25, -70), new Point( 25,  25, 70), new Point(0, -1, 0)), Color.OrangeRed, MaterialType.Mirror),
                new SceneElement(ElemType.Plain, new Plain(new Point(-25, -25, -70), new Point(-25,  25, 70), new Point(1, 0, 0)), Color.Beige, MaterialType.Matte),
                new SceneElement(ElemType.Plain, new Plain(new Point( 25, -25, -70), new Point( 25,  25, 70), new Point(-1, 0, 0)), Color.DeepSkyBlue, MaterialType.Matte),
                new SceneElement(ElemType.Plain, new Plain(new Point(-25, -25, -70), new Point( 25,  25, -70), new Point(0, 0, 1)), Color.Blue, MaterialType.Matte),
                //Кубик
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x - cube1Radius, cube1Center.y - cube1Radius, cube1Center.z + cube1Radius), new Point(cube1Center.x + cube1Radius, cube1Center.y + cube1Radius, cube1Center.z + cube1Radius), new Point(0, 0, 1)), cube1Col, cube1Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x - cube1Radius, cube1Center.y - cube1Radius, cube1Center.z - cube1Radius), new Point(cube1Center.x + cube1Radius, cube1Center.y + cube1Radius, cube1Center.z - cube1Radius), new Point(0, 0, -1)), cube1Col, cube1Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x - cube1Radius, cube1Center.y - cube1Radius, cube1Center.z - cube1Radius), new Point(cube1Center.x - cube1Radius, cube1Center.y + cube1Radius, cube1Center.z + cube1Radius), new Point(1, 0, 0)), cube1Col, cube1Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x + cube1Radius, cube1Center.y - cube1Radius, cube1Center.z - cube1Radius), new Point(cube1Center.x + cube1Radius, cube1Center.y + cube1Radius, cube1Center.z + cube1Radius), new Point(-1, 0, 0)), cube1Col, cube1Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x - cube1Radius, cube1Center.y - cube1Radius, cube1Center.z - cube1Radius), new Point(cube1Center.x + cube1Radius, cube1Center.y - cube1Radius, cube1Center.z + cube1Radius), new Point(0, -1, 0)), cube1Col, cube1Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube1Center.x - cube1Radius, cube1Center.y + cube1Radius, cube1Center.z - cube1Radius), new Point(cube1Center.x + cube1Radius, cube1Center.y + cube1Radius, cube1Center.z + cube1Radius), new Point(0, 1, 0)), cube1Col, cube1Material),
                //Кубик второй
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x - cube2Radius, cube2Center.y - cube2Radius, cube2Center.z + cube2Radius), new Point(cube2Center.x + cube2Radius, cube2Center.y + cube2Radius, cube2Center.z + cube2Radius), new Point(0, 0, 1)), cube2Col, cube2Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x - cube2Radius, cube2Center.y - cube2Radius, cube2Center.z - cube2Radius), new Point(cube2Center.x + cube2Radius, cube2Center.y + cube2Radius, cube2Center.z - cube2Radius), new Point(0, 0, -1)), cube2Col, cube2Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x - cube2Radius, cube2Center.y - cube2Radius, cube2Center.z - cube2Radius), new Point(cube2Center.x - cube2Radius, cube2Center.y + cube2Radius, cube2Center.z + cube2Radius), new Point(1, 0, 0)), cube2Col, cube2Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x + cube2Radius, cube2Center.y - cube2Radius, cube2Center.z - cube2Radius), new Point(cube2Center.x + cube2Radius, cube2Center.y + cube2Radius, cube2Center.z + cube2Radius), new Point(-1, 0, 0)), cube2Col, cube2Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x - cube2Radius, cube2Center.y - cube2Radius, cube2Center.z - cube2Radius), new Point(cube2Center.x + cube2Radius, cube2Center.y - cube2Radius, cube2Center.z + cube2Radius), new Point(0, -1, 0)), cube2Col, cube2Material),
                new SceneElement(ElemType.Cube, new Plain(new Point(cube2Center.x - cube2Radius, cube2Center.y + cube2Radius, cube2Center.z - cube2Radius), new Point(cube2Center.x + cube2Radius, cube2Center.y + cube2Radius, cube2Center.z + cube2Radius), new Point(0, 1, 0)), cube2Col, cube2Material)
            };
            List<LightSource> lightSources = new List<LightSource>
            {
                //new LightSource(new Point(10, -5, 0), 0.7),
                //new LightSource(new Point(20, 0, 5), 0.3)
                new LightSource(new Point(0, 0, 0), 0.1),
                new LightSource(new Point(20, -18, 42), 0.3),
                new LightSource(new Point(20, 10, 15), 0.6)
            };

            room.Image = RayTracing.rayTracing(room.Width, room.Height, elements, lightSources);
        }
    }
}

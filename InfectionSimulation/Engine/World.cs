using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfectionSimulation
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 300;
        private const int height = 300;
        private Size size = new Size(width, height);
        private List<GameObject> objects = new List<GameObject>();

        private List<GameObject>[,] spatialObjects = new List<GameObject>[width, height];
        private Pen pen = new Pen(Color.White);

        public IEnumerable<GameObject> GameObjects {
            get
            {
                return objects.ToArray();//AA: Para que sirve esto? Un array de objetos enumerables?
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Point Center { get { return new Point(width / 2, height / 2); } }

        public bool IsInside(Point p)//AA: Determina si el punto esta dentro del mundo de juego. Debería ser limitado desde antes. 
        {
            return p.X >= 0 && p.X < width
                && p.Y >= 0 && p.Y < height;
        }
        
        public Point RandomPoint()
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public float Random()
        {
            return (float)rnd.NextDouble();//AA: No se para que servirá
        }

        public int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public void Add(GameObject obj)
        {
            objects.Add(obj);
            GetBucketAt(obj.Position).Add(obj);
        }

        public void Remove(GameObject obj)
        {
            objects.Remove(obj);
            GetBucketAt(obj.Position).Remove(obj);
        }

        public List<GameObject> GetBucketAt(Point pos)
        {
            var bucket = spatialObjects[pos.X, pos.Y];
            if (bucket == null)
            {
                bucket = new List<GameObject>();
                spatialObjects[pos.X, pos.Y] = bucket;
            }
            return bucket;
        }

        public void Update()
        {
            foreach (GameObject obj in GameObjects)
            {
                var oldPosition = obj.Position;

                obj.InternalUpdateOn(this);//AA: El procesamiento pesado que ya noté.
                obj.Position = Mod(obj.Position, size);//AA: Como señale más abajo esto es totalmente eliminable, no tiene razon de ser.
                if (oldPosition != obj.Position)
                {
                    GetBucketAt(oldPosition).Remove(obj);
                    GetBucketAt(obj.Position).Add(obj);
                }
            }
        }

        public void DrawOn(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Black, 0, 0, width, height);//AA: Esto debe implementarse a cada rato??

            foreach (GameObject obj in GameObjects)
            {
                pen.Color = obj.Color;
                graphics.FillRectangle(pen.Brush, obj.Bounds);//AA: Es necesario crear un nuevo Pen x cada obj??
            }
        }

        public double Dist(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));//AA: Calculo excesivamente grande para la distancia entre dos puntos.
        }

        public double Dist(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int Mod(int a, int n)//AA: Un metodo Mod para int y otro para point, esta bien?
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;//AA: Todo esto es un sisntentido que retorna la misma posición que el objeto tiene actualmente.
        }//AA: Segun entendí esto es para obtener el resto en una división.
        private static Point Mod(Point p, Size s)
        {
            //AA: Anchura y altura = 300
            return new Point(Mod(p.X, s.Width), Mod(p.Y, s.Height));//Esto crearía un nuevo punto en 0,0 sin motivo aparente
        }

        public IEnumerable<GameObject> ObjectsAt(Point pos)
        {
            return GetBucketAt(pos);//AA: Desconcozco si esto es necesario u optimizable.
        }

    }
}

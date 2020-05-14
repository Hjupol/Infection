using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfectionSimulation
{
    abstract class GameObject
    {
        private Point position = new Point(0,0);
        private double rotation = 0;//AA: Es necesario un double? Solo es rotación.
        private Color color = Color.Black;
        private long lastUpdate = 0;//AA: Se trata de un long?

        public virtual long UpdateInterval { get { return 10; } }//A: Por qué es necesariamente 10?

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public double Rotation//AA: double?
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(Position, new Size(1, 1)); }
        }

        public void InternalUpdateOn(World world)
        {
            /*long now = Environment.TickCount;//AA: La cantidad de ticks desde que el prog se inicia, un calculo con esto seguramente sería costoso.
            if (now - lastUpdate > UpdateInterval)//AA: El calculo pesado propiamente dicho.
            {
                lastUpdate = now;//AA: En un caso normal guardaría una variable muy grande, para que?
                //AA: Corrección, esto se utiliza para que la actualización se realice cada 10 ticks.
                UpdateOn(world);
            }*/
            //AA: Elimine el intervalo limitante para verificar si funciona mejor o peor.
            UpdateOn(world);

        }

        public virtual void UpdateOn(World world)
        {
            // Do nothing
        }

        public void Turn(int angle)
        {
            Rotation += Math.PI * angle / 180.0;//AA: Rotaría el objeto, no tengo tanto conocimiento para saber si esto es performante o existe una manera más sencilla.
        }

        public void Forward(int dist)
        {
            //(direction degreeCos @ direction degreeSin) *distance + location
            //AA: No entiendo el comentario de acá arriba.
            Position = new Point((int)Math.Round(Math.Cos(rotation) * dist + Position.X),
                                 (int)Math.Round(Math.Sin(rotation) * dist + Position.Y));
            //AA: Utiliza el calculo del seno y coseno para hacer avanzar el punto ????
        }

        public void LookTo(Point p)
        {
            Rotation = Math.Atan2(p.Y - Position.Y, p.X - Position.X);
            //AA: No se si la tangente es efectiva para calcular esto, de cualquier manera es otro metodo para rotar el punto
        }
    }
}

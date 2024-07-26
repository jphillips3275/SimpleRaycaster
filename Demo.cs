using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Engine.Classes;
using System.Security.Cryptography.Xml;

namespace Engine
{
    internal class Demo : Classes.Engine
    {       //base means we're using the constructor of the base (engine(vector, string)) class
        Sprite2D player;
        Vector lastPos = new Vector();
        Vector CameraLastPos = new Vector();
        List<Line> direction = new List<Line>();

        bool left;
        bool right;
        bool up;
        bool down;

        string[,] Map =
        {
            {"-","-","-","-","-","-","-","-" },
            {"-","g","g","g","g","g","g","g" },
            {"-","g",".","g",".",".",".","g" },
            {"-","g",".","g",".","g",".","g" },
            {"-","g",".",".",".",".",".","g" },
            {"-","g",".",".",".",".",".","g" },
            {"-","g",".","g",".",".",".","g" },
            {"-","g","g","g","g","g","g","g" },
        };

        public Demo() : base(new Classes.Vector(1230,515),"Demo") { }


        public override void OnLoad()
        {
            BackgroundColor = Color.Black;
            CameraPos.x = 67;
            CameraPos.y = 0;

            //player = new Shape2D(new Vector(10, 10), new Vector(10, 10), "Test");
            //player = new Sprite2D(new Vector(10, 10), new Vector(50, 50), "Player", "Player");
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "g")
                    {
                        if (i != 0)
                        {
                            if (Map[j-1, i] == "g")
                            {
                                new Sprite2D(new Vector(i * 50, j * 50), new Vector(50, 50), "FloorTile2", "Ground");
                            }
                            else
                            {
                                new Sprite2D(new Vector(i * 50, j * 50), new Vector(50, 50), "FloorTile", "Ground");
                            }
                        }
                    }
                }
            }
            player = new Sprite2D(new Vector(160, 270), new Vector(30, 30), "Player1", "Player");
            for (float i = (float)-.60; i < (float).60; i+=(float).01)
            {
                direction.Add(new Line(player.Position, player.Scale, i));
            }

            Log.Info($"{direction.Count} rays cast");

            // we know there are 61 rays cast, the 3d should start at 615
            
            int x = 0;
            foreach (Line line in direction)
            {
                AllShapes.Add(new Shape2D(new Vector(615+((615/121)*x), (direction[x].Length)/2), new Vector((615 / 121), ((615/121)*515)/direction[x].Length*10), "Ray", x));
                x++;
            }
            
        }
        public override void OnDraw()
        {
            
        }

        float MoveSpeed = 3f;
        float Angle = 0;
        float CamreaAngle = 0;
        float DeltaX = (float)Math.Cos(0) * 5;
        float DeltaY = (float)Math.Sin(0) * 5;
        public override void OnUpdate()
        {

            if (up)
            {
                //player.Position.y -= MoveSpeed;
                player.Position.x += DeltaX/2;
                player.Position.y += DeltaY/2;
                foreach (Line l in direction) { l.ChangeAngle(0); }
                foreach (Shape2D shape in AllShapes) 
                {
                    CamreaAngle = Angle - direction[shape.DirectionIndex].Angle;
                    if (CamreaAngle < 0) { CameraAngle += 2 * (float)Math.PI; }
                    if (CameraAngle > 2*Math.PI) { CameraAngle -= 2 * (float)Math.PI; }
                    direction[shape.DirectionIndex].Length = (float)(direction[shape.DirectionIndex].Length * Math.Cos(CameraAngle));

                    shape.UpdateShape(new Vector(shape.Position.x, (float)direction[shape.DirectionIndex].Length/2), new Vector(shape.Scale.x, ((615 / 121) * 515) / (float)direction[shape.DirectionIndex].Length * 10)); 
                }


            }
            if (down)
            {
                player.Position.x -= DeltaX/2;
                player.Position.y -= DeltaY/2;
                //player.Position.y += MoveSpeed;
                foreach (Line l in direction) { l.ChangeAngle(0); }
                foreach (Shape2D shape in AllShapes)
                {
                    CamreaAngle = Angle - direction[shape.DirectionIndex].Angle;
                    if (CamreaAngle < 0) { CameraAngle += 2 * (float)Math.PI; }
                    if (CameraAngle > 2 * Math.PI) { CameraAngle -= 2 * (float)Math.PI; }
                    direction[shape.DirectionIndex].Length = (float)(direction[shape.DirectionIndex].Length * Math.Cos(CameraAngle));

                    shape.UpdateShape(new Vector(shape.Position.x, (float)direction[shape.DirectionIndex].Length / 2), new Vector(shape.Scale.x, ((615 / 121) * 515) / (float)direction[shape.DirectionIndex].Length * 10));
                }
            }
            if (left)
            {
                //player.Position.x -= MoveSpeed;
                Angle -= (float)0.05;
                if (Angle < 0) { Angle += 2 * (float)Math.PI; }
                DeltaX = (float)Math.Cos(Angle) * 5;
                DeltaY = (float)Math.Sin(Angle) * 5;
                foreach (Line l in direction) { l.ChangeAngle((float)-0.05); }
                foreach (Shape2D shape in AllShapes)
                {
                    CamreaAngle = Angle - direction[shape.DirectionIndex].Angle;
                    if (CamreaAngle < 0) { CameraAngle += 2 * (float)Math.PI; }
                    if (CameraAngle > 2 * Math.PI) { CameraAngle -= 2 * (float)Math.PI; }
                    direction[shape.DirectionIndex].Length = (float)(direction[shape.DirectionIndex].Length * Math.Cos(CameraAngle));

                    shape.UpdateShape(new Vector(shape.Position.x, (float)direction[shape.DirectionIndex].Length / 2), new Vector(shape.Scale.x, ((615 / 121) * 515) / (float)direction[shape.DirectionIndex].Length * 10));
                }
                Log.Info($"{Angle}");
            }
            if (right)
            {
                //player.Position.x += MoveSpeed;
                Angle += (float)0.05;
                if (Angle > 2*Math.PI) { Angle -= 2 * (float)Math.PI; }
                DeltaX = (float)Math.Cos(Angle) * 5;
                DeltaY = (float)Math.Sin(Angle) * 5;
                foreach (Line l in direction) { l.ChangeAngle((float)0.05); }
                foreach (Shape2D shape in AllShapes)
                {
                    CamreaAngle = Angle - direction[shape.DirectionIndex].Angle;
                    if (CamreaAngle < 0) { CameraAngle += 2 * (float)Math.PI; }
                    if (CameraAngle > 2 * Math.PI) { CameraAngle -= 2 * (float)Math.PI; }
                    direction[shape.DirectionIndex].Length = (float)(direction[shape.DirectionIndex].Length * Math.Cos(CameraAngle));

                    shape.UpdateShape(new Vector(shape.Position.x, (float)direction[shape.DirectionIndex].Length / 2), new Vector(shape.Scale.x, ((615 / 121) * 515) / (float)direction[shape.DirectionIndex].Length * 10));
                }
                Log.Info($"{Angle}");
            }


            if (player.IsColliding("Ground"))
            {
                player.Position.x = lastPos.x;
                player.Position.y = lastPos.y;
            } else
            {
                lastPos.x = player.Position.x;
                lastPos.y = player.Position.y;
            }
        }

        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.S) { down = true; }
            if (e.KeyCode == Keys.D) { right = true; }
        }

        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.S) { down = false; }
            if (e.KeyCode == Keys.D) { right = false; }
        }
    }
}

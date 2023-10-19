using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawLuckyWheel.Properties;

namespace DrawLuckyWheel
{
    public partial class Form1 : Form
    {
        bool wheelIsMoving;
        float wheelSpins;
        Timer wheelTimer;
        LuckyCircle fortuneWheel;
        SoundPlayer sound;

        public Form1()
        {
            InitializeComponent();
            wheelTimer = new Timer();
            wheelTimer.Interval = 30; // speed , small value more speed vice versa
            wheelTimer.Tick += wheelTimer_Tick; // telling to run wheel_tick method every 30 millisecond
            fortuneWheel = new LuckyCircle(); // Renamed to "LuckyCircle" for clarity
            sound = new SoundPlayer("sound.wav");

        }

        public class LuckyCircle
        {
            public Bitmap image;
            public Bitmap tempImage;
            public float angle;
            public int[] stateValues;
            public int state;

            public LuckyCircle()
            {
                tempImage = new Bitmap(Properties.Resources.lucky_wheel);
                image = new Bitmap(Properties.Resources.lucky_wheel);
                stateValues = new int[] { 12, 11, 10, 09, 08, 07, 06, 05, 04, 03, 02, 01 };
                angle = 0.0f;
            }
        }

        public static Bitmap RotateImage(Image image, float angle)
        {
            return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
            // new PointF tell it to stick to middle when rotating wrong angls will make the wheel revolve in circle with rotaion 
            // replce 2 with 3 and see what happens
        }

        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            rotatedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedImage);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));
            return rotatedImage;
        }

        private void RotateImage(PictureBox pb, Image img, float angle)
        {
            if (img == null || pb.Image == null)
                return;

            Image oldImage = pb.Image;
            pb.Image = RotateImage(img, angle);
            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void wheelTimer_Tick(object sender, EventArgs e)
        {
            if (wheelIsMoving && wheelSpins > 0)
            {
                fortuneWheel.angle += wheelSpins / 10;
                fortuneWheel.angle = fortuneWheel.angle % 360;
                RotateImage(pictureBox1, fortuneWheel.image, fortuneWheel.angle);
                wheelSpins--;
            }
            else
            {
                sound.Stop();
            }

            fortuneWheel.state = Convert.ToInt32(Math.Ceiling(fortuneWheel.angle / 30));

            if (fortuneWheel.state == 0)
            {
                fortuneWheel.state = 0;
            }
            else
            {
                fortuneWheel.state -= 1;
            }

            label1.Text = Convert.ToString(fortuneWheel.stateValues[fortuneWheel.state]);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            wheelIsMoving = true;
            Random rand = new Random();
            wheelSpins = rand.Next(150, 200);



            if (wheelSpins > 0)
            { sound.Play(); }



            wheelTimer.Start();

        }
    }
}

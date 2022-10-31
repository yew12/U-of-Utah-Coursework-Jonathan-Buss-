using System.Numerics;

namespace TowardAgarioStepOne
{
    public partial class TowardAgarioStepOne : Form
    {
        private Vector2 circleCenter; //fields - step#3

        private Vector2 direction; //field - step#3
        public TowardAgarioStepOne()
        {
            this.Paint += Draw_Scene; //step#4

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000 / 30;  // 1000 milliseconds in a second divided by 30 frames per second
            timer.Tick += (a, b) => this.Invalidate();
            timer.Start();

            InitializeComponent();
        }

        private void Draw_Scene(object? sender, PaintEventArgs e) //step #5
        {
            Move_Circle(); //step #5

            SolidBrush brush = new(Color.Gray);
            SolidBrush brush2 = new(Color.Firebrick);
            Pen pen = new(new SolidBrush(Color.Black));

            e.Graphics.DrawRectangle(pen, 10, 10, 510, 510);
            e.Graphics.FillRectangle(brush, 10, 10, 510, 510);
            e.Graphics.FillEllipse(brush2, new Rectangle((int)circleCenter.X, (int)circleCenter.Y, 30, 30));

        }

        private void Move_Circle() //step #5
        {

            int randomNumber1 = new Random().Next(0, 500);
            int randomNumber2 = new Random().Next(0, 500);

            circleCenter.Y = randomNumber1;
            circleCenter.X = randomNumber2;
            

            if (circleCenter.X > 500)
            {
                int randomNumberX = new Random().Next(0, 500);
                //giving it a new location (new direction)
                direction.X = randomNumberX;

            }
            if (circleCenter.Y > 500)
            {
                int randomNumberY = new Random().Next(0, 500);

                direction.Y = randomNumberY;
            }


            this.circleCenter += this.direction;

            this.PrintCenterofCircle.Text = $"X: {circleCenter.X} \nY: {circleCenter.Y}";
        }
    }
}
using System;
using System.Windows.Forms;
using static System.Windows.Forms.Control;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GdiApi
{
    public delegate void RenderEventHandler(Graphics graphics, TimeSpan delta);
    public delegate void UpdateEventHandler(Graphics graphics, TimeSpan delta);
    public delegate void EventHandler();
    public delegate void MouseEventHandler(MouseEventArgs mea);
    public delegate void KeyPressEventHandler(KeyPressEventArgs kpea);
    public delegate void KeyEventHandler(KeyEventArgs kea);
    public delegate void ClosingEventHandler(FormClosingEventArgs fcea);

    public class Context
    {
        private ContextForm Form;

        public event RenderEventHandler Render;
        public event UpdateEventHandler Update;
        public event EventHandler Load;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseClick;
        public event KeyPressEventHandler KeyPress;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event ClosingEventHandler Closing;
        public event EventHandler Resize;

        public object Title
        {
            get => Form.Text;
            set => Form.Text = value.ToString();
        }
        public Brush ClearBrush { get; set; } = new SolidBrush(Color.White);
        public DateTime LastFrameRender { get; private set; }
        public Point MouseLocation { get; private set; }
        public Size Size => Form.Size;
        public bool MouseClicked { get; private set; }
        public ControlCollection Controls => Form.Controls;
        public bool ClearScreen { get; set; } = true;
        public bool ManageFrameDraw { get; set; } = true;

        public Context() : this(new Size(500, 500)) { }
        public Context(Size size) : this(size, "GdiApi Context") {  }
        public Context(Size size, string title) : this (size, title, true) {  }
        public Context(Size size, string title, bool center) : this(size, title, center, FormBorderStyle.Sizable) {  }
        public Context(Size size, string title, bool center, FormBorderStyle border)
        {
            Form = new ContextForm()
            {
                ClientSize = size,
                Text = title,
                FormBorderStyle = border,
                KeyPreview = true,
            };

            if (center)
            {
                Form.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                Form.StartPosition = FormStartPosition.Manual;
            }

            var aProp = typeof(System.Windows.Forms.Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            aProp.SetValue(Form, true, null);

            Form.Paint += Paint;
            Form.Load += Form_Load;
            Form.MouseMove += Form_MouseMove;
            Form.MouseClick += Form_MouseClick;
            Form.MouseDown += Form_MouseDown;
            Form.MouseUp += Form_MouseUp;
            Form.KeyPress += Form_KeyPress;
            Form.KeyDown += Form_KeyDown;
            Form.KeyUp += Form_KeyUp;
            Form.FormClosing += Form_FormClosing;
            Form.Resize += Form_Resize;
        }

        public void Begin(bool async)
        {
            if (async)
            {
                Form.Show();
            }
            else
            {
                Form.ShowDialog();
            }
        }

        private void Form_Resize(object sender, EventArgs e) => Resize?.Invoke();
        private void Form_FormClosing(object sender, FormClosingEventArgs e) => Closing?.Invoke(e);
        private void Form_KeyUp(object sender, KeyEventArgs e) => KeyUp?.Invoke(e);
        private void Form_KeyDown(object sender, KeyEventArgs e) => KeyDown?.Invoke(e);
        private void Form_KeyPress(object sender, KeyPressEventArgs e) => KeyPress?.Invoke(e);
        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            MouseUp?.Invoke(e);
            MouseClicked = false;
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown?.Invoke(e);
            MouseClicked = true;
        }
        private void Form_MouseClick(object sender, MouseEventArgs e) => MouseClick?.Invoke(e);
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke(e);
            MouseLocation = e.Location;
        }
        private void Form_Load(object sender, EventArgs e) => Load?.Invoke();
        private void Paint(object sender, PaintEventArgs e)
        {
            if (ManageFrameDraw)
            {
                //No anti aliasing
                e.Graphics.SmoothingMode = SmoothingMode.None;
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.None;

                //Get delta
                var delta = DateTime.Now - LastFrameRender;

                //Update
                Update?.Invoke(e.Graphics, delta);

                //Clear screen
                if (ClearScreen)
                {
                    e.Graphics.FillRectangle(ClearBrush, new Rectangle(0, 0, Form.ClientSize.Width, Form.ClientSize.Height));
                }

                //Render
                Render?.Invoke(e.Graphics, delta);

                //End frame
                LastFrameRender = DateTime.Now;
                Form.Invalidate();
            }
        }
    }
}

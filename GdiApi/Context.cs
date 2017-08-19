using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GdiApi
{
    public delegate void RenderEventHandler(Graphics graphics, TimeSpan delta);
    public delegate void UpdateEventHandler(Graphics graphics, TimeSpan delta);
    public delegate void LoadEventHandler();

    public class Context
    {
        private ContextForm Form;

        public event RenderEventHandler RenderEvent;
        public event UpdateEventHandler UpdateEvent;
        public event LoadEventHandler LoadEvent;

        public object Title
        {
            get => Form.Text;
            set => Form.Text = value.ToString();
        }
        public Brush ClearBrush { get; set; } = new SolidBrush(Color.White);
        public DateTime LastFrameRender { get; private set; }

        public Context() : this(new Size(500, 500)) { }
        public Context(Size size) : this(size, "GdiApi Context") {  }
        public Context(Size size, string title) : this (size, title, true) {  }
        public Context(Size size, string title, bool center)
        {
            Form = new ContextForm()
            {
                ClientSize = size,
                Text = title,
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
            Form.Load += Load;

            Form.ShowDialog();
        }

        private void Load(object sender, EventArgs e)
        {
            LoadEvent?.Invoke();
            Load();
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            var delta = DateTime.Now - LastFrameRender;

            Update(e.Graphics, delta);
            UpdateEvent?.Invoke(e.Graphics, delta);

            e.Graphics.FillRectangle(ClearBrush, new Rectangle(0, 0, Form.ClientSize.Width, Form.ClientSize.Height));

            Render(e.Graphics, delta);
            RenderEvent?.Invoke(e.Graphics, delta);

            LastFrameRender = DateTime.Now;
            Form.Invalidate();
        }

        /// <summary>
        /// When the context loads. No need to call base.
        /// </summary>
        public virtual void Load()
        {

        }

        /// <summary>
        /// Called every frame render, before Context.Render. No need to call base.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="delta"></param>
        public virtual void Update(Graphics graphics, TimeSpan delta)
        {

        }

        /// <summary>
        /// Called every frame render, after Context.Update. No need to call base.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="delta"></param>
        public virtual void Render(Graphics graphics, TimeSpan delta)
        {

        }
    }
}

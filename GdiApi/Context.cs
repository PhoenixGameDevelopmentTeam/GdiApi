using System;
using System.Windows.Forms;
using System.Drawing;

namespace GdiApi
{
    public delegate void RenderEventHandler(Graphics graphics, TimeSpan delta);
    public delegate void UpdateEventHandler(Graphics graphics, TimeSpan delta);

    public class Context
    {
        private Form Form;

        public event RenderEventHandler RenderEvent;
        public event UpdateEventHandler UpdateEvent;

        public DateTime LastFrameRender { get; private set; }

        public Context(Size size)
        {
            Form = new Form();
            Form.Paint += Paint;
            Form.Size = size;
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            var timeSinceLastFrame = LastFrameRender - DateTime.Now;

            Update(e.Graphics, timeSinceLastFrame);
            UpdateEvent?.Invoke(e.Graphics, timeSinceLastFrame);

            Render(e.Graphics, timeSinceLastFrame);
            RenderEvent?.Invoke(e.Graphics, timeSinceLastFrame);

            LastFrameRender = DateTime.Now;
            Form.Invalidate();
        }

        /// <summary>
        /// Called every frame render, before Context.Render. No need to call base.
        /// </summary>
        public virtual void Update(Graphics graphics, TimeSpan delta)
        {

        }

        /// <summary>
        /// Called every frame render, after Context.Render. No need to call base.
        /// </summary>
        /// <param name="graphics">The graphics used to draw.</param>
        public virtual void Render(Graphics graphics, TimeSpan delta)
        {

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;


namespace UILayer.Controllers
{
    public class CaptchaActionResault : ActionResult
    {

        //  private readonly string _agha;
        //  public CaptchaActionResault(string agha)
        //{
        //    _agha = agha;
        //}
        Bitmap _bitmap;
        public string capcthText { get; set; }
        public CaptchaActionResault(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }
        public CaptchaActionResault()
        {
            var CaptchaImage = new Utility.Captcha();
            _bitmap = CaptchaImage.CreateImage();
            capcthText = CaptchaImage.capcthaText;
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            //Color BackColor = Color.White;
            //String FontName = "Times New Roman";
            //int FontSize = 25;
            //int Height = 50;
            //int Width = 700;

            //using (Bitmap bitmap = new Bitmap(Width, Height))
            //using (Graphics graphics = Graphics.FromImage(bitmap))
            //{
            //    Color color = Color.Gray;
            //    Font font = new Font(FontName, FontSize);

            //    SolidBrush BrushBackColor = new SolidBrush(BackColor);
            //    Pen BorderPen = new Pen(color);

            //    Rectangle displayRectangle = new Rectangle(new Point(0, 0), new Size(Width - 1, Height - 1));

            //    graphics.FillRectangle(BrushBackColor, displayRectangle);
            //    graphics.DrawRectangle(BorderPen, displayRectangle);

            //    graphics.DrawString(_agha, font, Brushes.Red, 0, 0);

            context.HttpContext.Response.ContentType = "image/jpg";
           // _bitmap.Save(context.HttpContext.Response. , ImageFormat.Jpeg);
        }



    }
}

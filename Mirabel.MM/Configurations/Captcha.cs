using System;
using Microsoft.AspNetCore.Http;
using Mirabel.MM.Configurations;
using Mirabel.MM.Models;
using Mirabel.MM.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirabel.MM.Configurations
{
    public class Captcha
    {
        const string Letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";

        public static string GenerateCaptchaCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public static bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context)
        {
            var isValid = userInputCaptcha == context.Session.GetString("CaptchaCode");
            context.Session.Remove("CaptchaCode");
            return isValid;
        }

        public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();

                graph.Clear(GetRandomLightColor());

                DrawCaptchaCode();

                MemoryStream ms = new MemoryStream();

                baseMap.Save(ms, ImageFormat.Png);

                return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

                int GetFontSize(int imageWidth, int captchCodeCount)
                {
                    var averageSize = imageWidth / captchCodeCount;

                    return Convert.ToInt32(averageSize);
                }

                Color GetRandomDeepColor()
                {
                    int redlow = 160, greenLow = 100, blueLow = 160;
                    return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                }

                Color GetRandomLightColor()
                {
                    int low = 180, high = 255;

                    int nRend = rand.Next(high) % (high - low) + low;
                    int nGreen = rand.Next(high) % (high - low) + low;
                    int nBlue = rand.Next(high) % (high - low) + low;

                    return Color.FromArgb(nRend, nGreen, nBlue);
                }

                void DrawCaptchaCode()
                {
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    int fontSize = GetFontSize(width, captchaCode.Length);
                    Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        fontBrush.Color = GetRandomDeepColor();

                        int shiftPx = fontSize / 6;

                        float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                        int maxY = height - fontSize;
                        if (maxY < 0) maxY = 0;
                        float y = rand.Next(0, maxY);

                        graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                    }
                }

            }
        }
    }
}

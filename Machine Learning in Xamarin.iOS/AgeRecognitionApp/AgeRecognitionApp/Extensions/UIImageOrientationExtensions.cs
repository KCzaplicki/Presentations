using CoreImage;
using ImageIO;
using UIKit;

namespace AgeRecognitionApp.Extensions
{
    public static class UIImageOrientationExtensions
    {
        public static CGImagePropertyOrientation ToCGImagePropertyOrientation(this UIImageOrientation self)
        {
            switch (self)
            {
                case UIImageOrientation.Up:
                    return CGImagePropertyOrientation.Up;
                case UIImageOrientation.UpMirrored:
                    return CGImagePropertyOrientation.UpMirrored;
                case UIImageOrientation.Down:
                    return CGImagePropertyOrientation.Down;
                case UIImageOrientation.DownMirrored:
                    return CGImagePropertyOrientation.DownMirrored;
                case UIImageOrientation.Left:
                    return CGImagePropertyOrientation.Left;
                case UIImageOrientation.LeftMirrored:
                    return CGImagePropertyOrientation.LeftMirrored;
                case UIImageOrientation.Right:
                    return CGImagePropertyOrientation.Right;
                case UIImageOrientation.RightMirrored:
                    return CGImagePropertyOrientation.RightMirrored;
            }

            return CGImagePropertyOrientation.Up;
        }

        public static CIImageOrientation ToCIImageOrientation(this UIImageOrientation self)
        {
            switch (self)
            {
                case UIImageOrientation.Up:
                    return CIImageOrientation.TopLeft;
                case UIImageOrientation.UpMirrored:
                    return CIImageOrientation.TopRight;
                case UIImageOrientation.Down:
                    return CIImageOrientation.BottomLeft;
                case UIImageOrientation.DownMirrored:
                    return CIImageOrientation.BottomRight;
                case UIImageOrientation.Left:
                    return CIImageOrientation.LeftTop;
                case UIImageOrientation.LeftMirrored:
                    return CIImageOrientation.LeftBottom;
                case UIImageOrientation.Right:
                    return CIImageOrientation.RightTop;
                case UIImageOrientation.RightMirrored:
                    return CIImageOrientation.RightBottom;
            }

            return CIImageOrientation.TopLeft;
        }

    }
}

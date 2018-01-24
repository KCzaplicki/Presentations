using CoreGraphics;

namespace AgeRecognitionApp.Extensions
{
    public static class CGPointExtensions
    {
        public static CGPoint Scaled(this CGPoint self, CGSize size)
        {
            return new CGPoint(self.X * size.Width, self.Y * size.Height);
        }
    }
}

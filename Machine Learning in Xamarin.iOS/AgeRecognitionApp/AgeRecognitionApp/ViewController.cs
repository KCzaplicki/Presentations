using System;
using Foundation;
using UIKit;
using CoreML;
using Vision;
using CoreImage;
using CoreFoundation;
using CoreGraphics;
using AgeRecognitionApp.Extensions;

namespace AgeRecognitionApp
{
    public partial class ViewController : UIViewController, IUIImagePickerControllerDelegate
    {
        CIImage InputImage;
        UIImage RawImage;
        VNDetectFaceRectanglesRequest FaceRectangleRequest;
        VNCoreMLRequest AgeRecognitionRequest;

        protected ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            FaceRectangleRequest = new VNDetectFaceRectanglesRequest(HandleRectangles);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public UIImage OverlayRectangles(UIImage uiImage, CGSize imageSize, VNFaceObservation[] observations)
        {
            const float margin = 60;
            nfloat fWidth = uiImage.Size.Width;
            nfloat fHeight = uiImage.Size.Height;

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)fWidth, (nint)fHeight, 8, 4 * (nint)fWidth, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst))
            {
                ctx.DrawImage(new CGRect(0, 0, fWidth, fHeight), uiImage.CGImage);
                var observation = observations[0];

                var topLeft = new CGPoint(observation.BoundingBox.Left, observation.BoundingBox.Top).Scaled(imageSize);
                var topRight = new CGPoint(observation.BoundingBox.Left, observation.BoundingBox.Bottom).Scaled(imageSize);
                var bottomLeft = new CGPoint(observation.BoundingBox.Right, observation.BoundingBox.Top).Scaled(imageSize);
                var bottomRight = new CGPoint(observation.BoundingBox.Right, observation.BoundingBox.Bottom).Scaled(imageSize);

                ctx.SetStrokeColor(UIColor.Red.CGColor);
                ctx.SetLineWidth(5);

                var path = new CGPath();
                path.AddLines(new CGPoint[] { topLeft, topRight, bottomRight, bottomLeft });
                path.CloseSubpath();
                ctx.AddPath(path);
                ctx.DrawPath(CGPathDrawingMode.Stroke);

                var ciImage = new CIImage(uiImage);
                var observationImage = ciImage.ImageByCroppingToRect(new CGRect(
                    topLeft.X - margin,
                    topLeft.Y - margin,
                    bottomRight.X - topLeft.X + 2 * margin,
                    bottomRight.Y - topLeft.Y + 2 * margin));

                var handler = new VNImageRequestHandler(observationImage, new VNImageOptions());
                DispatchQueue.DefaultGlobalQueue.DispatchAsync(() => {
                    NSError err;
                    handler.Perform(new VNRequest[] { AgeRecognitionRequest }, out err);
                });

                return UIImage.FromImage(ctx.ToImage());
            }
        }

        [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            picker.DismissViewController(true, null);
            var uiImage = info[UIImagePickerController.OriginalImage] as UIImage;
            var ciImage = new CIImage(uiImage);
            ImageView.Image = uiImage;

            var handler = new VNImageRequestHandler(ciImage, uiImage.Orientation.ToCGImagePropertyOrientation(), new VNImageOptions());
            DispatchQueue.DefaultGlobalQueue.DispatchAsync(() => {
                NSError error;
                handler.Perform(new VNRequest[] { FaceRectangleRequest }, out error);
            });

            var bundle = NSBundle.MainBundle;
            var assetPath = bundle.GetUrlForResource("AgeNet", "mlmodelc");
            NSError mlErr, vnErr;
            var mlModel = MLModel.Create(assetPath, out mlErr);
            var model = VNCoreMLModel.FromMLModel(mlModel, out vnErr);
            AgeRecognitionRequest = new VNCoreMLRequest(model, HandleClassification);
        }

        partial void ChooseImage(UIButton sender)
        {
            var picker = new UIImagePickerController()
            {
                Delegate = this,
                SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum
            };

            PresentViewController(picker, true, null);
        }

        void HandleRectangles(VNRequest request, NSError error)
        {
            var observations = request.GetResults<VNFaceObservation>();
            var imageSize = InputImage.Extent.Size;

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                ImageView.Image = OverlayRectangles(RawImage, imageSize, observations);
            });
        }

        void HandleClassification(VNRequest request, NSError error)
        {

            var observations = request.GetResults<VNClassificationObservation>();
            var best = observations[0];

            DispatchQueue.MainQueue.DispatchAsync(() => {
                AgeLabel.Text = $"Age: {best.Identifier} Confidence: {best.Confidence * 100f:#.00}%";
            });
        }
    }
}

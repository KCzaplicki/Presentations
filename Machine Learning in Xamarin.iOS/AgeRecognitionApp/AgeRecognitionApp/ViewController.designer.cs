// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AgeRecognitionApp
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AgeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Action ("ChooseImage:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChooseImage (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AgeLabel != null) {
                AgeLabel.Dispose ();
                AgeLabel = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }
        }
    }
}
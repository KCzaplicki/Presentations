using System.Collections.Generic;
using CoreGraphics;
using CoreImage;
using Foundation;

namespace AgeRecognitionApp.Extensions
{
    public static class DictionaryExtensions
    {
        public static NSDictionary<NSString, CIVector> ToNSDictionary(this Dictionary<string, CGPoint> self)
        {

            var keys = new List<NSString>();
            var values = new List<CIVector>();

            foreach (string key in self.Keys)
            {
                keys.Add(new NSString(key));
                values.Add(new CIVector(self[key]));
            }

            return new NSDictionary<NSString, CIVector>(keys.ToArray(), values.ToArray());
        }

        public static NSDictionary<NSString, NSNumber> ToNSDictionary(this Dictionary<NSString, NSNumber> self)
        {

            var keys = new List<NSString>();
            var values = new List<NSNumber>();

            foreach (NSString key in self.Keys)
            {
                keys.Add(key);
                values.Add(self[key]);
            }

            return new NSDictionary<NSString, NSNumber>(keys.ToArray(), values.ToArray());
        }
    }
}

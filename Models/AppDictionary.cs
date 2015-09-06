using HttpLauncher.Utils;
using System;
using System.Runtime.Serialization;

namespace HttpLauncher.Models
{
    [Serializable]
    public class AppDictionary : SerializableDictionary<string, AppInfo>
    {
        public AppDictionary() { }
        public AppDictionary(int capacity) : base(capacity) { }
        protected AppDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

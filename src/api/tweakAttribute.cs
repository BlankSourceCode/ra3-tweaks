using System;

namespace RA3Tweaks
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TweakAttribute : Attribute
    {
        public string ClassName;
        public string MethodName;
        public bool InsertAtStart = true;
        public string ReplaceName;

        public TweakAttribute(string className, string methodName)
        {
            this.ClassName = className;
            this.MethodName = methodName;
        }

        public TweakAttribute(string className, string methodName, bool insertAtStart) :
            this(className, methodName)
        {
            this.InsertAtStart = insertAtStart;
        }

        public TweakAttribute(string className, string methodName, string replaceName) :
            this(className, methodName)
        {
            this.ReplaceName = replaceName;
        }
    }
}
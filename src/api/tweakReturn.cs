using System;

namespace RA3Tweaks
{
    public struct TweakReturn<T>
    {
        public bool PreventDefault;
        public T ReturnValue;

        public TweakReturn(bool preventDefault, T returnValue)
        {
            this.PreventDefault = preventDefault;
            this.ReturnValue = returnValue;
        }
    }

    public struct TweakReturnVoid
    {
        public bool PreventDefault;

        public TweakReturnVoid(bool preventDefault)
        {
            this.PreventDefault = preventDefault;
        }
    }
}
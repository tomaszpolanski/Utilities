using System;

namespace Utilities.Functional
{
    public class Unit
    {
        public static readonly Unit Default = new Unit();

        public static Unit AsUnit(Action action)
        {
            action.Invoke();
            return Default;
        }

        private Unit() { }
    }
}

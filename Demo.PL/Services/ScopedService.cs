using System;

namespace Demo.PL.Services
{
    public class ScopedService : IScopedService
    {
        public Guid Guid { get ; set; } // NULL

        public ScopedService()
        {
            Guid = Guid.NewGuid(); 
        }

        public string GetGuid()
        {
            return Guid.ToString();
        }

        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}

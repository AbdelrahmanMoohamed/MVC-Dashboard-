using System;

namespace Demo.PL.Services
{
    public class SingeltonService : ISingeltonService
    {
        public Guid Guid { get ; set; }

        public SingeltonService()
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

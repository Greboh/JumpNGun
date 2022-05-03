using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class Factory
    {
        public abstract GameObject Create(Enum type);
    }
}

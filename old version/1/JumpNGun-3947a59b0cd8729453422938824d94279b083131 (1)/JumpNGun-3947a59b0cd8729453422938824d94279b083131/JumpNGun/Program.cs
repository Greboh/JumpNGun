using System;

namespace JumpNGun
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            GameWorld.Instance.Run();
        }
    }
}

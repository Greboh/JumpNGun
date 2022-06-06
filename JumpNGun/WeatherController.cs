using System;
using System.Collections.Generic;
using System.Text;



namespace JumpNGun
{
    class WeatherController
    {
        private static WeatherController _instance;

        public static WeatherController Instance
        {
            get { return _instance ??= new WeatherController(); }
        }

    }
}

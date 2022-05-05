namespace JumpNGun
{
    public class Director
    {
        private IBuilder _builder;

        public Director(IBuilder builder)
        {
            _builder = builder;
        }

        public GameObject Construct()
        {
            _builder.BuildGameObject();
            
            return _builder.GetResult();
        }
    }
}
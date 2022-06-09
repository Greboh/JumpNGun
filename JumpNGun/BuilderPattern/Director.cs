namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class Director
    {
        private IBuilder _builder; // Reference to our IBuilder
        public Director(IBuilder builder)
        {
            _builder = builder;
        }
        

        /// <summary>
        /// Constructs the GameObject
        /// </summary>
        /// <returns>Returns the constructed GameObject</returns>
        public GameObject Construct()
        {
            _builder.BuildGameObject();
            
            return _builder.GetResult();
        }
    }
}
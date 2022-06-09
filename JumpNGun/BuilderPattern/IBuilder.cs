namespace JumpNGun
{
    /// <summary>
    /// Interfaced er lavet af Nichlas Hoberg
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Builds the GameObject with all required components
        /// </summary>
        void BuildGameObject();

        /// <summary>
        /// Returns the result of the builder
        /// </summary>
        /// <returns></returns>
        GameObject GetResult();
    }
}
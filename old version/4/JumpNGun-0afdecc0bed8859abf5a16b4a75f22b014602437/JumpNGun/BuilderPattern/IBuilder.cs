namespace JumpNGun
{
    public interface IBuilder
    {
        /// <summary>
        /// Builds the GameObject with all required components etc.
        /// </summary>
        void BuildGameObject();

        /// <summary>
        /// Returns the result of the builder
        /// </summary>
        /// <returns></returns>
        GameObject GetResult();
    }
}
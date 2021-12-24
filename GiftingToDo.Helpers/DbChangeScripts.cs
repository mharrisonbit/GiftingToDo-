namespace GiftingToDo.Helpers
{
    public class DbChangeScripts
    {
        /// <summary>
        /// this is going to update the app based on the version that was passed to the method.
        /// </summary>
        /// <param name="versionNum"></param>
        public void DbUpdate(double versionNum)
        {
            if (versionNum <= Constants.CurrentDBVersion)
            {

            }
        }
    }
}

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
            switch (versionNum)
            {
                case (1.0):
                    //do stuff
                    break;
                default:
                    break;
            }
        }
    }
}

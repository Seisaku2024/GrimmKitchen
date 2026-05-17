using UnityEngine.Localization.Settings;
namespace LocalizationCodes
{
    public static class TextStringTable
    {
        const string TableName = "TextStringTable";
        //public static string Yes => GetLocalizedString("Yes");
        //public static string No => GetLocalizedString("No");
        //public static string Message => GetLocalizedString("Message");
        //public static string Message2 => GetLocalizedString("Message2");
        //public static string Wepon => GetLocalizedString("Wepon");
        public static string GetLocalizedString(string key)
        {
            var table =
                LocalizationSettings.StringDatabase.
                GetTable(TableName);
            return table.GetEntry(key).GetLocalizedString();
        }

    }
}

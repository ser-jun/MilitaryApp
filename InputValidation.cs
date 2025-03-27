namespace MilitaryApp
{
    public static class InputValidation
    {
        public static bool ValidationUpdateDeleteMethod<T>(T selecteItem, out string errormessage)
        {
            if (selecteItem == null)
            {
                errormessage = "Выберите элемент";
                return false;
            }
            errormessage = string.Empty;
            return true;
        }
        public static bool ValidationAddMethod(string name, string type, int quantity, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type) || quantity >= 99 || quantity <= 0)
            {
                errorMessage = "Введите корректные данные";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public static bool ValidationAddMethodForInfrastructure(string name, int year, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name) || year >= 2025 || year <= 1900)
            {
                errorMessage = "Введите корректные данные";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public static bool CheckCommanderUnit(string position, int unitId, out string errorMessage)
        {
            if (position.Trim().ToLower() == "командир части" && unitId != null)
            {
                errorMessage = "У данной части есть командир";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public static bool CheckAddMethodPersonnel(string name, string lastName, string position, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(position))
            {
                errorMessage = "Заполните все поля";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public static bool CheckAddMethodStructure(string name, string structureType, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(structureType))
            {
                errorMessage = "Заполните все поля";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}

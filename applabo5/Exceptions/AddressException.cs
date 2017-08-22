using System;

namespace HomeSnailHome.Exceptions
{
    public class AddressException : Exception
    {
        public string ErrorMessage { get; set; }

        public AddressException(int code)
        {
            switch (code)
            {
                case 1:
                    ErrorMessage = "Veuillez introduire une numéro de maison";
                    break;
                case 2:
                    ErrorMessage = "Boite postale non valide, veuillez introduire un nombre";
                    break;
                case 3:
                    ErrorMessage = "Veuillez introduire un Code Postal";
                    break;
                case 4:
                    ErrorMessage = "Code Postal non valide, celui-ci doit être un nombre";
                    break;
                case 5:
                    ErrorMessage = "Veuillez introduire un nom de rue";
                    break;
                case 6:
                    ErrorMessage = "Veuillez introduire un nom de localité";
                    break;
                case 7:
                    ErrorMessage = "Veuillez introduire un nom de pays";
                    break;
                case 8:
                    ErrorMessage = "Code Postal non valide, veuillez introduire un Code Postal belge valide";
                    break;
                case 9:
                    ErrorMessage = "Veuillez sélectionner une localité";
                    break;
            }
        }
    }
}

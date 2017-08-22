using System;

namespace AppLabo5.Exceptions
{
    public class AddUserException : Exception
    {
        public string ErrorMessage { get; set; }

        public AddUserException(int code)
        {
            switch (code)
            {
                case 1:
                    ErrorMessage = "Veuillez introduire une adresse email";
                    break;
                case 2:
                    ErrorMessage = "Adresse email non valide";
                    break;
                case 3:
                    ErrorMessage = "Adresse email déja utilisée";
                    break;
                case 4:
                    ErrorMessage = "Veuillez introduire un mot de passe";
                    break;
                case 5:
                    ErrorMessage = "Veuillez confirmer le mot de passe";
                    break;
                case 6:
                    ErrorMessage = "Mot de passe trop court (Minimum 8 caractères)";
                    break;
                case 7:
                    ErrorMessage = "Mot de passe non valide, celui-ci doit comprendre au minimum un caractère spécial (%, _, # ou @), un chiffre, une minuscule et une majuscule";
                    break;
                case 8:
                    ErrorMessage = "Mots de passe différents";
                    break;
                case 9:
                    ErrorMessage = "Veuillez introduire un nom";
                    break;
                case 10:
                    ErrorMessage = "Veuillez introduire un prénom";
                    break;
                case 11:
                    ErrorMessage = "Vous devez avoir au moins 15 ans afin d'utiliser cette application";
                    break;
            }
        }
    }
}

using System;

namespace HomeSnailHome.Exceptions
{
    public class SearchHousingException : Exception
    {
        public string ErrorMessage { get; set; }

        public SearchHousingException(int code)
        {
            switch (code)
            {
                case 1:
                    ErrorMessage = "Période de recherche non valide, la fin ne peut être antérieure au début";
                    break;
                case 2:
                    ErrorMessage = "Période de recherche non valide, le début et la fin ne peuvent concerner le même jour";
                    break;
                case 3:
                    ErrorMessage = "Veuillez seélectionner un type de lit";
                    break;
            }
        }
    }
}
